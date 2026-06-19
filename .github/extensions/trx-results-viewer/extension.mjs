import { createServer } from "node:http";
import { readFile, readdir, stat } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";
import { CanvasError, createCanvas, joinSession } from "@github/copilot-sdk/extension";

const extensionDir = path.dirname(fileURLToPath(import.meta.url));
const repoRoot = path.resolve(extensionDir, "..", "..", "..");
const ignoredDirectories = new Set([".git", ".github", "obj", "node_modules"]);
const servers = new Map();
const knownTrxFiles = new Map();
let copilotSession;
let pollTimer;
let automaticOpenInProgress = false;

function workspaceRoot() {
    return repoRoot;
}

function toDisplayPath(filePath) {
    return path.relative(workspaceRoot(), filePath).split(path.sep).join("/");
}

function resolveWorkspaceFile(filePath) {
    if (!filePath || typeof filePath !== "string") {
        throw new CanvasError("trx_file_required", "A TRX file path is required.");
    }

    const normalizedInput = filePath.replace(/[\\/]+/g, path.sep);
    const resolved = path.resolve(workspaceRoot(), normalizedInput);
    const root = workspaceRoot();
    if (resolved !== root && !resolved.startsWith(root + path.sep)) {
        throw new CanvasError("trx_file_outside_workspace", "TRX file paths must stay inside the workspace.");
    }
    if (path.extname(resolved).toLowerCase() !== ".trx") {
        throw new CanvasError("trx_file_invalid_type", "Only .trx files can be opened.");
    }
    return resolved;
}

async function scanTrxFiles(directory = workspaceRoot(), results = []) {
    if (results.length >= 200) {
        return results;
    }

    let entries;
    try {
        entries = await readdir(directory, { withFileTypes: true });
    } catch {
        return results;
    }

    for (const entry of entries) {
        const fullPath = path.join(directory, entry.name);
        if (entry.isDirectory()) {
            if (!ignoredDirectories.has(entry.name)) {
                await scanTrxFiles(fullPath, results);
            }
            continue;
        }

        if (entry.isFile() && entry.name.toLowerCase().endsWith(".trx")) {
            const fileStat = await stat(fullPath);
            results.push({
                name: entry.name,
                path: toDisplayPath(fullPath),
                modifiedMs: fileStat.mtimeMs,
            });
        }
    }

    return results.sort((left, right) => left.path.localeCompare(right.path));
}

async function findTrxFiles() {
    return (await scanTrxFiles()).map(({ name, path }) => ({ name, path }));
}

function testCommandWasRun(input) {
    const toolName = String(input?.toolName ?? "").toLowerCase();
    const command = String(input?.toolArgs?.command ?? "");
    return (toolName.includes("powershell") || toolName.includes("bash") || toolName.includes("terminal"))
        && /\bdotnet\s+test\b|\btest\b/i.test(command);
}

async function refreshKnownTrxFiles() {
    knownTrxFiles.clear();
    for (const file of await scanTrxFiles()) {
        knownTrxFiles.set(file.path, file.modifiedMs);
    }
}

async function openLatestTrxResult(reason = "detected") {
    if (!copilotSession || automaticOpenInProgress) {
        return;
    }

    automaticOpenInProgress = true;
    try {
        const files = await scanTrxFiles();
        const changedFiles = files.filter((file) => {
            const knownModifiedMs = knownTrxFiles.get(file.path);
            return knownModifiedMs === undefined || file.modifiedMs > knownModifiedMs;
        });

        for (const file of files) {
            knownTrxFiles.set(file.path, file.modifiedMs);
        }

        const latest = changedFiles.sort((left, right) => right.modifiedMs - left.modifiedMs)[0];
        if (!latest) {
            return;
        }

        await copilotSession.rpc.canvas.open({
            canvasId: "trx-results-viewer",
            instanceId: "trx-results-latest",
            input: { filePath: latest.path },
        });
        await copilotSession.log(`Opened Test Results for ${latest.path} (${reason}).`, { ephemeral: true });
    } finally {
        automaticOpenInProgress = false;
    }
}

function decodeXml(value = "") {
    return value
        .replace(/&quot;/g, "\"")
        .replace(/&apos;/g, "'")
        .replace(/&lt;/g, "<")
        .replace(/&gt;/g, ">")
        .replace(/&amp;/g, "&");
}

function parseAttributes(attributeText = "") {
    const attributes = {};
    const attributePattern = /([\w:.-]+)\s*=\s*("([^"]*)"|'([^']*)')/g;
    let match;
    while ((match = attributePattern.exec(attributeText)) !== null) {
        attributes[match[1]] = decodeXml(match[3] ?? match[4] ?? "");
    }
    return attributes;
}

function elements(xml, tagName) {
    const escapedTag = tagName.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
    const matches = [];
    const elementPattern = new RegExp(`<(?:[\\w.-]+:)?${escapedTag}\\b([^>]*?)(?:/\\s*>|>([\\s\\S]*?)<\\/(?:[\\w.-]+:)?${escapedTag}\\s*>)`, "gi");
    let match;
    while ((match = elementPattern.exec(xml)) !== null) {
        matches.push({
            attributes: parseAttributes(match[1]),
            content: match[2] ?? "",
        });
    }
    return matches;
}

function firstElement(xml, tagName) {
    return elements(xml, tagName)[0];
}

function textOf(xml, tagName) {
    const element = firstElement(xml, tagName);
    return element ? decodeXml(element.content.replace(/<[^>]+>/g, "").trim()) : "";
}

function durationToMs(duration = "") {
    const match = /^(\d+):(\d+):(\d+(?:\.\d+)?)$/.exec(duration);
    if (!match) {
        return 0;
    }

    const [, hours, minutes, seconds] = match;
    return Math.round(((Number(hours) * 60 * 60) + (Number(minutes) * 60) + Number(seconds)) * 1000);
}

function parseTrx(xml, source) {
    const testRun = firstElement(xml, "TestRun")?.attributes ?? {};
    const resultSummary = firstElement(xml, "ResultSummary")?.attributes ?? {};
    const counters = firstElement(xml, "Counters")?.attributes ?? {};
    const times = firstElement(xml, "Times")?.attributes ?? {};

    const definitions = new Map();
    for (const unitTest of elements(xml, "UnitTest")) {
        const testMethod = firstElement(unitTest.content, "TestMethod")?.attributes ?? {};
        const categories = elements(unitTest.content, "TestCategoryItem")
            .map((category) => category.attributes.TestCategory)
            .filter(Boolean);

        definitions.set(unitTest.attributes.id, {
            id: unitTest.attributes.id,
            name: unitTest.attributes.name ?? testMethod.name ?? "",
            className: testMethod.className ?? "",
            methodName: testMethod.name ?? "",
            storage: unitTest.attributes.storage ?? testMethod.codeBase ?? "",
            categories,
        });
    }

    const results = elements(xml, "UnitTestResult").map((result) => {
        const definition = definitions.get(result.attributes.testId) ?? {};
        const errorInfo = firstElement(result.content, "ErrorInfo")?.content ?? "";
        const stdout = textOf(result.content, "StdOut");
        const stderr = textOf(result.content, "StdErr");
        return {
            id: result.attributes.executionId ?? result.attributes.testId ?? result.attributes.testName,
            testId: result.attributes.testId ?? "",
            name: result.attributes.testName ?? definition.name ?? "",
            className: definition.className ?? "",
            methodName: definition.methodName ?? "",
            outcome: result.attributes.outcome ?? "Unknown",
            duration: result.attributes.duration ?? "",
            durationMs: durationToMs(result.attributes.duration),
            startTime: result.attributes.startTime ?? "",
            endTime: result.attributes.endTime ?? "",
            computerName: result.attributes.computerName ?? "",
            categories: definition.categories ?? [],
            errorMessage: textOf(errorInfo, "Message"),
            stackTrace: textOf(errorInfo, "StackTrace"),
            stdout,
            stderr,
        };
    });

    const derivedCounts = results.reduce((counts, result) => {
        const key = result.outcome.toLowerCase();
        counts[key] = (counts[key] ?? 0) + 1;
        return counts;
    }, {});

    const total = Number(counters.total ?? counters.executed ?? results.length);
    const passed = Number(counters.passed ?? derivedCounts.passed ?? 0);
    const failed = Number(counters.failed ?? derivedCounts.failed ?? 0);
    const notExecuted = Number(counters.notExecuted ?? derivedCounts.notexecuted ?? 0);
    const other = Math.max(0, total - passed - failed - notExecuted);
    const totalDurationMs = results.reduce((totalMs, result) => totalMs + result.durationMs, 0);

    return {
        source,
        run: {
            id: testRun.id ?? "",
            name: testRun.name ?? "",
            user: testRun.runUser ?? "",
            outcome: resultSummary.outcome ?? "",
            startTime: times.start ?? "",
            finishTime: times.finish ?? "",
        },
        counts: {
            total,
            executed: Number(counters.executed ?? results.length),
            passed,
            failed,
            notExecuted,
            other,
            passRate: total > 0 ? Math.round((passed / total) * 1000) / 10 : 0,
        },
        totalDurationMs,
        results: results.sort((left, right) => {
            const rank = { Failed: 0, Error: 1, Timeout: 2, Inconclusive: 3, Passed: 4 };
            return (rank[left.outcome] ?? 5) - (rank[right.outcome] ?? 5) || right.durationMs - left.durationMs;
        }),
    };
}

async function loadTrxFile(filePath) {
    const resolved = resolveWorkspaceFile(filePath);
    const xml = await readFile(resolved, "utf8");
    return parseTrx(xml, toDisplayPath(resolved));
}

function sendJson(res, statusCode, payload) {
    res.writeHead(statusCode, {
        "Content-Type": "application/json; charset=utf-8",
        "Cache-Control": "no-store",
    });
    res.end(JSON.stringify(payload));
}

async function readRequestBody(req) {
    const chunks = [];
    for await (const chunk of req) {
        chunks.push(chunk);
    }
    return Buffer.concat(chunks).toString("utf8");
}

function renderHtml(initialFilePath = "") {
    return `<!doctype html>
<html lang="en">
<head>
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1" />
<title>Test Results</title>
<style>
:root {
  color-scheme: light dark;
  --surface: var(--background-color-default, #ffffff);
  --surface-muted: var(--background-color-muted, #f6f8fa);
  --surface-hover: var(--background-color-muted, #f6f8fa);
  --border: var(--border-color-default, #d0d7de);
  --border-muted: var(--border-color-muted, #d8dee4);
  --text: var(--text-color-default, #1f2328);
  --text-muted: var(--text-color-muted, #656d76);
  --focus: var(--focus-border-color, #0969da);
  --green: var(--true-color-green, #1a7f37);
  --red: var(--true-color-red, #cf222e);
  --yellow: var(--true-color-yellow, #9a6700);
}
@media (prefers-color-scheme: dark) {
  :root {
    --surface: var(--background-color-default, #0d1117);
    --surface-muted: var(--background-color-muted, #161b22);
    --surface-hover: var(--background-color-muted, #21262d);
    --border: var(--border-color-default, #30363d);
    --border-muted: var(--border-color-muted, #21262d);
    --text: var(--text-color-default, #f0f6fc);
    --text-muted: var(--text-color-muted, #8b949e);
    --focus: var(--focus-border-color, #58a6ff);
    --green: var(--true-color-green, #3fb950);
    --red: var(--true-color-red, #f85149);
    --yellow: var(--true-color-yellow, #d29922);
  }
}
* { box-sizing: border-box; }
body {
  margin: 0;
  background: var(--surface);
  color: var(--text);
  font-family: var(--font-sans, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif);
  font-size: var(--text-body-medium, 14px);
  line-height: var(--leading-body-medium, 20px);
}
button, select, input {
  font: inherit;
}
.shell {
  display: grid;
  grid-template-rows: auto auto 1fr;
  min-height: 100vh;
}
.header {
  border-bottom: 1px solid var(--border);
  padding: 16px 20px;
}
.title {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  margin-bottom: 12px;
}
h1 {
  margin: 0;
  font-size: var(--text-title-large, 24px);
  line-height: var(--leading-title-large, 32px);
}
.muted {
  color: var(--text-muted);
}
.toolbar {
  display: grid;
  grid-template-columns: minmax(160px, 1fr) auto auto;
  gap: 8px;
}
.select, .search {
  width: 100%;
  border: 1px solid var(--border);
  border-radius: 8px;
  background: var(--surface);
  color: var(--text);
  padding: 8px 10px;
}
.button {
  border: 1px solid var(--border);
  border-radius: 8px;
  background: var(--surface-muted);
  color: var(--text);
  cursor: pointer;
  padding: 8px 12px;
}
.button:hover {
  background: var(--surface-hover);
  color: var(--text);
}
.upload {
  position: relative;
  overflow: hidden;
}
.upload input {
  cursor: pointer;
  inset: 0;
  opacity: 0;
  position: absolute;
}
.summary {
  align-items: start;
  display: grid;
  grid-template-columns: repeat(5, minmax(0, 1fr));
  gap: 12px;
  padding: 16px 20px;
  border-bottom: 1px solid var(--border);
}
.card {
  border: 1px solid var(--border);
  border-radius: 10px;
  padding: 12px;
}
.summary .card {
  align-content: center;
  align-self: start;
  display: grid;
  height: 86px;
  max-height: 86px;
  min-height: 86px;
}
.card.button:hover {
  background: var(--surface-muted);
  border-color: var(--focus);
  box-shadow: inset 0 0 0 1px var(--focus);
}
.card.button.active {
  background: var(--surface-muted);
  border-color: var(--focus);
  box-shadow: inset 0 0 0 1px var(--focus);
}
.metric {
  font-size: 28px;
  font-weight: var(--font-weight-semibold, 600);
  line-height: 34px;
}
.metric.passed { color: var(--green); }
.metric.failed { color: var(--red); }
.metric.skipped { color: var(--yellow); }
.content {
  background: var(--surface);
  min-height: 0;
  overflow: auto;
  padding: 16px 20px 24px;
}
.list {
  display: grid;
  gap: 8px;
  min-height: 0;
}
.ledger-header {
  align-items: center;
  color: var(--text-muted);
  display: grid;
  font-size: 12px;
  font-weight: var(--font-weight-semibold, 600);
  gap: 12px;
  grid-template-columns: minmax(220px, 1fr) minmax(120px, 18%) 92px 112px;
  letter-spacing: .04em;
  padding: 0 14px 4px 22px;
  text-transform: uppercase;
}
.row {
  background: var(--surface);
  border: 1px solid var(--border-muted);
  border-radius: 12px;
  cursor: pointer;
  overflow: hidden;
  position: relative;
  transition: background-color .12s ease, border-color .12s ease, box-shadow .12s ease, transform .12s ease;
}
.row:hover {
  background: var(--surface-hover);
  border-color: var(--border);
}
.row:focus {
  outline: 2px solid var(--focus);
  outline-offset: 2px;
}
.row.active {
  border-color: var(--focus);
  box-shadow: 0 0 0 1px var(--focus);
}
.row-main {
  align-items: center;
  display: grid;
  gap: 12px;
  grid-template-columns: 4px minmax(220px, 1fr) minmax(120px, 18%) 92px 112px;
  padding: 12px 14px 12px 0;
}
.status-rail {
  align-self: stretch;
  background: var(--text-muted);
  border-radius: 0 999px 999px 0;
}
.status-rail.Passed { background: var(--green); }
.status-rail.Failed, .status-rail.Error, .status-rail.Timeout { background: var(--red); }
.status-rail.NotExecuted { background: var(--yellow); }
.test-title {
  min-width: 0;
}
.row-name {
  font-weight: var(--font-weight-semibold, 600);
  overflow-wrap: anywhere;
}
.row-class {
  font-family: var(--font-mono, ui-monospace, SFMono-Regular, "SF Mono", Consolas, "Liberation Mono", monospace);
  font-size: 12px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
.row-duration {
  font-family: var(--font-mono, ui-monospace, SFMono-Regular, "SF Mono", Consolas, "Liberation Mono", monospace);
  font-size: 12px;
}
.row-meta {
  align-items: center;
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-top: 6px;
}
.pill {
  border: 1px solid var(--border);
  border-radius: 999px;
  display: inline-flex;
  font-size: 12px;
  line-height: 16px;
  padding: 2px 8px;
}
.pill.Passed { color: var(--green); }
.pill.Failed, .pill.Error, .pill.Timeout { color: var(--red); }
.drawer {
  border-top: 1px solid var(--border-muted);
  display: grid;
  gap: 16px;
  grid-template-columns: minmax(0, 320px) minmax(0, 1fr);
  padding: 16px 18px 18px 22px;
}
.drawer > * {
  min-width: 0;
}
.drawer h2 {
  font-size: 16px;
  line-height: 22px;
  margin: 0 0 12px;
  overflow-wrap: anywhere;
}
.output {
  min-width: 0;
  overflow: hidden;
}
.output h3 {
  margin: 0 0 8px;
}
.empty {
  align-items: center;
  color: var(--text-muted);
  display: flex;
  justify-content: center;
  min-height: 240px;
  padding: 32px;
  text-align: center;
}
.tabs {
  display: flex;
  gap: 8px;
  margin: 0 0 12px;
}
.tab.active {
  background: var(--text);
  color: var(--color-white, #ffffff);
}
pre {
  background: var(--surface-muted);
  border: 1px solid var(--border);
  border-radius: 8px;
  max-width: 100%;
  overflow: auto;
  padding: 12px;
  white-space: pre-wrap;
  word-break: break-word;
}
.kv {
  display: grid;
  grid-template-columns: 140px minmax(0, 1fr);
  gap: 8px;
  min-width: 0;
}
.kv span {
  min-width: 0;
  overflow-wrap: anywhere;
}
.kv .code {
  font-family: var(--font-mono, ui-monospace, SFMono-Regular, "SF Mono", Consolas, "Liberation Mono", monospace);
  font-size: 12px;
}
.error {
  border-bottom: 1px solid var(--border);
  color: var(--red);
  padding: 12px 20px;
}
@media (max-width: 820px) {
  .toolbar, .summary { grid-template-columns: 1fr; }
  .ledger-header { display: none; }
  .row-main, .drawer {
    grid-template-columns: 4px minmax(0, 1fr);
  }
  .row-outcome, .row-duration, .row-computer {
    grid-column: 2;
  }
  .drawer {
    display: block;
    padding-left: 18px;
  }
}
</style>
</head>
<body>
<div class="shell">
  <header class="header">
    <div class="title">
      <div>
        <h1>Test Results</h1>
        <div id="source" class="muted">Choose a TRX file from this workspace or upload one.</div>
      </div>
      <button id="refresh" class="button" type="button">Refresh</button>
    </div>
    <div class="toolbar">
      <select id="files" class="select" aria-label="TRX file"></select>
      <label class="button upload">Upload TRX <input id="upload" type="file" accept=".trx,text/xml,application/xml" /></label>
      <input id="search" class="search" type="search" placeholder="Search tests, classes, outcomes..." />
    </div>
  </header>
  <div id="message"></div>
  <section id="summary" class="summary"></section>
  <main class="content">
    <section id="results" class="list"></section>
  </main>
</div>
<script>
const initialFilePath = ${JSON.stringify(initialFilePath)};
let state = { files: [], data: null, selectedIndex: 0, filter: "all", search: "" };

const escapeHtml = (value = "") => String(value)
  .replace(/&/g, "&amp;")
  .replace(/</g, "&lt;")
  .replace(/>/g, "&gt;")
  .replace(/"/g, "&quot;")
  .replace(/'/g, "&#39;");

function formatDuration(ms = 0) {
  if (!ms) return "0 ms";
  if (ms < 1000) return ms + " ms";
  const seconds = ms / 1000;
  if (seconds < 60) return seconds.toFixed(seconds >= 10 ? 1 : 2) + " s";
  const minutes = Math.floor(seconds / 60);
  return minutes + "m " + Math.round(seconds % 60) + "s";
}

function setMessage(message, isError = false) {
  const element = document.getElementById("message");
  element.className = isError ? "error" : "";
  element.textContent = message || "";
}

async function fetchJson(url, options) {
  const response = await fetch(url, options);
  const payload = await response.json();
  if (!response.ok) {
    throw new Error(payload.error || "Request failed");
  }
  return payload;
}

async function refreshFiles() {
  state.files = await fetchJson("/api/files");
  const select = document.getElementById("files");
  select.innerHTML = state.files.length
    ? state.files.map((file) => '<option value="' + escapeHtml(file.path) + '">' + escapeHtml(file.path) + '</option>').join("")
    : '<option value="">No TRX files found in workspace</option>';

  if (initialFilePath && state.files.some((file) => file.path === initialFilePath)) {
    select.value = initialFilePath;
    await loadWorkspaceFile(initialFilePath);
  } else if (state.files.length) {
    select.value = state.files[0].path;
    await loadWorkspaceFile(state.files[0].path);
  } else {
    renderEmpty("No TRX files found. Upload a .trx file to view results.");
  }
}

async function loadWorkspaceFile(filePath) {
  if (!filePath) return;
  setMessage("");
  state.data = await fetchJson("/api/result?file=" + encodeURIComponent(filePath));
  state.selectedIndex = 0;
  document.getElementById("source").textContent = state.data.source;
  render();
}

function renderEmpty(message) {
  document.getElementById("summary").innerHTML = "";
  document.getElementById("results").innerHTML = '<div class="empty">' + escapeHtml(message) + '</div>';
}

function filteredResults() {
  if (!state.data) return [];
  const query = state.search.trim().toLowerCase();
  return state.data.results.filter((result) => {
    const matchesFilter = state.filter === "all" || result.outcome === state.filter;
    const haystack = [result.name, result.className, result.methodName, result.outcome, result.errorMessage, result.categories.join(" ")].join(" ").toLowerCase();
    return matchesFilter && (!query || haystack.includes(query));
  });
}

function renderSummary() {
  const counts = state.data.counts;
  document.getElementById("summary").innerHTML = [
    ["Total", counts.total, "", "all"],
    ["Passed", counts.passed, "passed", "Passed"],
    ["Failed", counts.failed, "failed", "Failed"],
    ["Skipped", counts.notExecuted, "skipped", "NotExecuted"],
    ["Pass rate", counts.passRate + "%", "passed", ""],
  ].map(([label, value, css, filter]) => {
    const active = filter && state.filter === filter;
    const filterAttrs = filter ? ' data-filter="' + filter + '" aria-pressed="' + active + '"' : ' aria-disabled="true"';
    return '<button class="card button ' + (active ? "active" : "") + '" type="button"' + filterAttrs + '><div class="metric ' + css + '">' + value + '</div><div class="muted">' + label + '</div></button>';
  }).join("");
}

function renderList(results) {
  const container = document.getElementById("results");
  if (!results.length) {
    container.innerHTML = '<div class="empty">No matching test results.</div>';
    return;
  }
  state.selectedIndex = Math.min(state.selectedIndex, results.length - 1);
  container.innerHTML =
    '<div class="ledger-header"><span>Test</span><span>Outcome</span><span>Duration</span><span>Host</span></div>' +
    results.map((result, index) => {
      const active = index === state.selectedIndex;
      return '<article class="row ' + (active ? "active" : "") + '" data-index="' + index + '" role="button" tabindex="0" aria-expanded="' + active + '">' +
        '<div class="row-main">' +
        '<span class="status-rail ' + escapeHtml(result.outcome) + '"></span>' +
        '<div class="test-title"><div class="row-name">' + escapeHtml(result.name) + '</div><div class="row-class muted">' + escapeHtml(result.className || "n/a") + '</div></div>' +
        '<div class="row-outcome"><span class="pill ' + escapeHtml(result.outcome) + '">' + escapeHtml(result.outcome) + '</span></div>' +
        '<div class="row-duration muted">' + formatDuration(result.durationMs) + '</div>' +
        '<div class="row-computer muted">' + escapeHtml(result.computerName || "n/a") + '</div>' +
        '</div>' +
        (active ? renderDetails(result) : "") +
        '</article>';
    }).join("");
}

function renderDetails(result) {
  if (!result) return "";
  const outputBlocks = [
    result.errorMessage && ["Error", result.errorMessage],
    result.stackTrace && ["Stack trace", result.stackTrace],
    result.stdout && ["StdOut", result.stdout],
    result.stderr && ["StdErr", result.stderr],
  ].filter(Boolean).map(([title, value]) => '<h3>' + escapeHtml(title) + '</h3><pre>' + escapeHtml(value) + '</pre>').join("");

  return '<div class="drawer">' +
    '<div><h2>' + escapeHtml(result.name) + '</h2>' +
    '<div class="kv">' +
    '<strong>Outcome</strong><span><span class="pill ' + escapeHtml(result.outcome) + '">' + escapeHtml(result.outcome) + '</span></span>' +
    '<strong>Duration</strong><span class="code">' + formatDuration(result.durationMs) + '</span>' +
    '<strong>Class</strong><span class="code">' + escapeHtml(result.className || "n/a") + '</span>' +
    '<strong>Method</strong><span class="code">' + escapeHtml(result.methodName || "n/a") + '</span>' +
    '<strong>Computer</strong><span>' + escapeHtml(result.computerName || "n/a") + '</span>' +
    '<strong>Categories</strong><span>' + escapeHtml((result.categories || []).join(", ") || "n/a") + '</span>' +
    '</div></div>' +
    '<div class="output">' + (outputBlocks || '<h3>Output</h3><pre>No output captured for this test.</pre>') + '</div>' +
    '</div>';
}

function render() {
  if (!state.data) return;
  renderSummary();
  renderList(filteredResults());
}

document.getElementById("files").addEventListener("change", (event) => {
  loadWorkspaceFile(event.target.value).catch((error) => setMessage(error.message, true));
});
document.getElementById("refresh").addEventListener("click", () => {
  refreshFiles().catch((error) => setMessage(error.message, true));
});
document.getElementById("search").addEventListener("input", (event) => {
  state.search = event.target.value;
  state.selectedIndex = 0;
  render();
});
document.getElementById("summary").addEventListener("click", (event) => {
  const button = event.target.closest("[data-filter]");
  if (!button) return;
  const nextFilter = button.dataset.filter;
  state.filter = state.filter === nextFilter && nextFilter !== "all" ? "all" : nextFilter;
  state.selectedIndex = 0;
  render();
});
document.getElementById("results").addEventListener("click", (event) => {
  const row = event.target.closest("[data-index]");
  if (!row) return;
  state.selectedIndex = Number(row.dataset.index);
  render();
});
document.getElementById("results").addEventListener("keydown", (event) => {
  if (event.key !== "Enter" && event.key !== " ") return;
  const row = event.target.closest("[data-index]");
  if (!row) return;
  event.preventDefault();
  state.selectedIndex = Number(row.dataset.index);
  render();
});
document.getElementById("upload").addEventListener("change", async (event) => {
  const file = event.target.files[0];
  if (!file) return;
  try {
    state.data = await fetchJson("/api/parse", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ xml: await file.text(), source: file.name }),
    });
    state.selectedIndex = 0;
    document.getElementById("source").textContent = file.name;
    setMessage("");
    render();
  } catch (error) {
    setMessage(error.message, true);
  }
});

refreshFiles().catch((error) => setMessage(error.message, true));
</script>
</body>
</html>`;
}

async function startServer(instanceId, initialFilePath) {
    const server = createServer(async (req, res) => {
        try {
            const url = new URL(req.url ?? "/", "http://127.0.0.1");
            if (req.method === "GET" && url.pathname === "/api/files") {
                sendJson(res, 200, await findTrxFiles());
                return;
            }

            if (req.method === "GET" && url.pathname === "/api/result") {
                sendJson(res, 200, await loadTrxFile(url.searchParams.get("file")));
                return;
            }

            if (req.method === "POST" && url.pathname === "/api/parse") {
                const body = JSON.parse(await readRequestBody(req));
                sendJson(res, 200, parseTrx(body.xml ?? "", body.source ?? "uploaded.trx"));
                return;
            }

            res.writeHead(200, {
                "Content-Type": "text/html; charset=utf-8",
                "Cache-Control": "no-store",
            });
            res.end(renderHtml(initialFilePath));
        } catch (error) {
            sendJson(res, error instanceof CanvasError ? 400 : 500, {
                error: error instanceof Error ? error.message : "Unexpected TRX viewer error.",
            });
        }
    });
    await new Promise((resolve) => server.listen(0, "127.0.0.1", resolve));
    const address = server.address();
    const port = typeof address === "object" && address ? address.port : 0;
    return { server, url: `http://127.0.0.1:${port}/` };
}

const filePathSchema = {
    type: "object",
    additionalProperties: false,
    properties: {
        filePath: {
            type: "string",
            description: "Workspace-relative path to a .trx file.",
        },
    },
};

copilotSession = await joinSession({
    hooks: {
        onPostToolUse: async (input) => {
            if (testCommandWasRun(input)) {
                setTimeout(() => {
                    openLatestTrxResult("test command completed").catch((error) => {
                        copilotSession?.log(`Could not open Test Results: ${error.message}`, { level: "warning", ephemeral: true });
                    });
                }, 1000);
            }
        },
    },
    canvases: [
        createCanvas({
            id: "trx-results-viewer",
            displayName: "Test Results",
            description: "Displays Visual Studio TRX test result files with summary metrics, filtering, and failure details.",
            inputSchema: filePathSchema,
            actions: [
                {
                    name: "list_trx_files",
                    description: "List TRX files found under the current workspace.",
                    handler: async () => findTrxFiles(),
                },
                {
                    name: "load_trx_file",
                    description: "Parse a workspace-relative TRX file and return its summary and test results.",
                    inputSchema: {
                        type: "object",
                        additionalProperties: false,
                        required: ["filePath"],
                        properties: {
                            filePath: {
                                type: "string",
                                description: "Workspace-relative path to a .trx file.",
                            },
                        },
                    },
                    handler: async (ctx) => loadTrxFile(ctx.input?.filePath),
                },
            ],
            open: async (ctx) => {
                let initialFilePath = ctx.input?.filePath ?? "";
                if (initialFilePath) {
                    initialFilePath = toDisplayPath(resolveWorkspaceFile(initialFilePath));
                }

                let entry = servers.get(ctx.instanceId);
                if (!entry) {
                    entry = await startServer(ctx.instanceId, initialFilePath);
                    servers.set(ctx.instanceId, entry);
                }
                return {
                    title: "Test Results",
                    status: initialFilePath || "Ready",
                    url: entry.url,
                };
            },
            onClose: async (ctx) => {
                const entry = servers.get(ctx.instanceId);
                if (entry) {
                    servers.delete(ctx.instanceId);
                    await new Promise((resolve) => entry.server.close(() => resolve()));
                }
            },
        }),
    ],
});

await refreshKnownTrxFiles();
pollTimer = setInterval(() => {
    openLatestTrxResult("new TRX file detected").catch((error) => {
        copilotSession?.log(`Could not open Test Results: ${error.message}`, { level: "warning", ephemeral: true });
    });
}, 3000);

process.once("SIGTERM", () => {
    if (pollTimer) {
        clearInterval(pollTimer);
    }
});
