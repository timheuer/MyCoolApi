// Copyright (c) Microsoft Corporation. All rights reserved.

// Make tests to run in parallel
[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]
// Ensure that class cleanup is executed at the end of the class and not at the end of the assembly (legacy default behavior)
[assembly: ClassCleanupExecution(ClassCleanupBehavior.EndOfClass)]
