# MyCoolApi

MyCoolApi is a .NET 9.0 ASP.NET Core Web API project demonstrating mathematical operations, greeting functionality, string casing conversions, weather forecasting, and OS information endpoints. The project includes comprehensive testing with the new Microsoft Testing Platform (MTP), Azure deployment infrastructure with Bicep templates, and Docker containerization support.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites and Setup
- Install .NET 10.x preview SDK (used by GitHub Actions and development)
- Note: The project targets .NET 9.0 runtime but uses .NET 10.x preview SDK for building
- CRITICAL: `dotnet restore` may fail due to SSL/certificate issues with NuGet in certain environments (such as sandboxed environments with certificate validation problems)
- If restore fails with SSL certificate errors, this is a known limitation of the environment and should be documented but not fixed

### Build and Test Commands
- **NEVER CANCEL builds or long-running commands**. Set timeout to 120+ minutes minimum.
- Build: `dotnet build --configuration Release --no-restore` -- takes 1-3 minutes normally, may fail if dependencies not restored. NEVER CANCEL.
- Test: `dotnet test --configuration Release --no-build -- --report-trx --coverage` -- takes 2-5 minutes. NEVER CANCEL.
- Restore: `dotnet restore` -- takes 15-30 seconds normally, may fail with SSL errors. NEVER CANCEL.

### Running the Application
- Development HTTP: `dotnet run --project MyCoolApi --launch-profile http`
- Development HTTPS: `dotnet run --project MyCoolApi --launch-profile https`
- Application runs on http://localhost:5250 (HTTP) and https://localhost:7271 (HTTPS)
- Swagger UI available at `/swagger` endpoint
- API endpoints are accessible for manual testing

### Key Projects Structure
```
MyCoolApi/                  # Main ASP.NET Core Web API (.NET 9.0)
├── Program.cs             # Main application entry point with all endpoint definitions
├── MathHelpers.cs         # Math operations: Add, Subtract, Multiply, Fibonacci, Factorial, etc.
├── HelloBuilders.cs       # Greeting functionality with proper casing
├── StringCasingHelpers.cs # String conversion using Humanizer library
└── Properties/launchSettings.json # Development launch profiles

MyCoolApi.Tests/           # Test project using Microsoft Testing Platform (.NET 9.0)
├── MathTests.cs          # Integration and unit tests for math operations  
├── HelloTests.cs         # Tests for greeting endpoints
├── StringCasingTests.cs  # Tests for string casing conversion
├── WeatherForecastTests.cs # Tests for weather forecast endpoint
├── EnvironmentTests.cs   # Tests for environment/OS information
├── ErrorHandlingTests.cs # Tests for error scenarios
├── OSTests.cs           # Platform-specific OS tests
└── MyCoolApiApp.cs      # Test application factory for integration tests

UtilsLib/                 # Supporting library (.NET 9.0)
├── Api/Person.cs         # Simple Person record

ClassLibrary1/            # Additional class library (.NET 10.0)
ClassLib2/                # Additional class library (.NET 10.0)

infra/                    # Azure deployment infrastructure
├── main.bicep           # Main Bicep template
├── resources.bicep      # Azure resources (App Service, etc.)
└── *.json               # Parameter files

.github/workflows/        # CI/CD pipelines
├── MyCoolApi.yaml       # Main build workflow (uses .NET 9.0)
└── copilot-setup-steps.yml # Copilot setup (uses .NET 10.x preview)
```

## API Endpoints and Testing

### Available Endpoints
- `GET /add/{num1},{num2}` - Add two numbers
- `GET /multiply/{num1},{num2}` - Multiply two numbers  
- `GET /divide/{num1}` - Divide number by 2
- `GET /doubleit/{num1}` - Multiply number by 2
- `GET /fibonacci/{num}` - Get Fibonacci sequence
- `GET /hello/{name}` - Say hello with proper name casing
- `GET /bye/{name}` - Say goodbye with proper name casing
- `GET /env` - Get OS description
- `GET /casing/{input}/{caseType}` - Convert string casing (pascal, camel, snake, kebab, sentence, title, upper, lower)
- `GET /weatherforecast` - Get sample weather data

### Manual Validation Scenarios
After making changes, ALWAYS test these scenarios:
1. **Math Operations**: 
   - Test `GET /add/5,3` should return `8`
   - Test `GET /multiply/4,5` should return `20`
   - Test `GET /divide/12` should return `6.0`
   - Test `GET /fibonacci/5` should return `[0,1,1,2,3]`
2. **String Operations**: 
   - Test `GET /hello/john` should return `"Hello John"`
   - Test `GET /bye/mary` should return `"Bye Mary!"`
3. **Casing Conversion**: 
   - Test `GET /casing/hello_world/pascal` should return `"HelloWorld"`
   - Test `GET /casing/HelloWorldTest/snake` should return `"hello_world_test"`
   - Test `GET /casing/hello-world/camel` should return `"helloWorld"`
4. **Error Handling**: 
   - Test `GET /casing/hello/invalid` should return 400 Bad Request with error message
5. **Environment Info**: 
   - Test `GET /env` should return OS description string
6. **Weather**: 
   - Test `GET /weatherforecast` should return array of 5 weather objects with Date, TemperatureC, Summary properties

### Test Execution
- Run full test suite: `dotnet test` -- takes 2-5 minutes. NEVER CANCEL.
- Run specific test class: `dotnet test --filter MathTests`
- Run specific test method: `dotnet test --filter "StringCasingTests.ConvertCase_ToPascalCase_Success"`
- Test project uses Microsoft Testing Platform (MTP) with executable output
- Coverage reporting is enabled but may not work in all environments
- Tests include both unit tests (direct method calls) and integration tests (HTTP client calls using WebApplicationFactory)
- Integration tests create temporary in-memory test server for each test

## Development Workflow

### Making Changes
1. Always run existing tests first: `dotnet test` to establish baseline
2. Make minimal code changes to fix issues
3. Build: `dotnet build --configuration Release`
4. Test: `dotnet test --configuration Release --no-build`
5. Run the application: `dotnet run --project MyCoolApi`
6. Manually test the relevant endpoints listed above
7. Check CI workflow passes (GitHub Actions)

### Common Issues and Solutions
- **NuGet SSL Errors**: Common in sandboxed environments. Document as limitation, don't attempt to fix.
- **Port Conflicts**: Use `dotnet run --urls "http://localhost:5251"` to change port
- **Missing Dependencies**: If build fails, check that all PackageReference entries are correct
- **Test Failures**: Check that test application factory is properly configured in MyCoolApiApp.cs

### CI/CD Pipeline
- GitHub Actions workflow in `.github/workflows/MyCoolApi.yaml`
- Builds on Ubuntu with .NET 9.x SDK
- Runs restore, build (Release), and test with coverage
- Triggered on push to main, PRs, and manual workflow dispatch
- NEVER CANCEL: CI builds can take 5-10 minutes. Always wait for completion.

## Important Files Reference

### Key Configuration Files
```yaml
# .github/workflows/MyCoolApi.yaml - CI/CD workflow
# Uses .NET 9.x, runs on Ubuntu
# Commands: dotnet restore → dotnet build --configuration Release --no-restore → dotnet test --configuration Release --no-build

# MyCoolApi/MyCoolApi.csproj - Main project
# .NET 9.0, includes Humanizer, Swagger, OpenAPI packages

# MyCoolApi.Tests/MyCoolApi.Tests.csproj - Test project  
# .NET 9.0, uses Microsoft Testing Platform (MTP), includes AspNetCore.Mvc.Testing

# playground.http - Sample HTTP requests for manual testing
# Contains example requests for all API endpoints
```

### Dependencies
- **Humanizer**: String manipulation and casing conversion
- **Swashbuckle.AspNetCore**: OpenAPI/Swagger documentation
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing framework
- **MSTest with Microsoft Testing Platform**: Modern testing framework

## Common tasks
The following are outputs from frequently run commands. Reference them instead of viewing, searching, or running bash commands to save time.

### Repository Root Structure
```
$ ls -la
.devcontainer/          # VS Code development container configuration  
.github/               # GitHub Actions workflows and templates
.gitignore            # Standard Visual Studio .gitignore
.dockerignore         # Docker ignore patterns
MyCoolApi/            # Main ASP.NET Core Web API project (.NET 9.0)
MyCoolApi.Tests/      # Test project with Microsoft Testing Platform (.NET 9.0)
MyCoolApi.slnx        # Solution file (XML format)
UtilsLib/             # Utility library (.NET 9.0) with Person record  
ClassLibrary1/        # Supporting class library (.NET 10.0)
ClassLib2/            # Supporting class library (.NET 10.0)
infra/                # Azure Bicep deployment templates
docs/readme.md        # Basic project documentation
playground.http       # HTTP requests for manual API testing
testEnvironments.json # Test environment configuration
LICENSE.txt           # MIT License
```

### Main Project Files (MyCoolApi/)
```
Program.cs                 # Main application with all API endpoint definitions
MathHelpers.cs            # Math operations: Add, Subtract, Multiply, Fibonacci, etc.
HelloBuilders.cs          # Name greeting with proper casing using CultureInfo.TextInfo.ToTitleCase
StringCasingHelpers.cs    # String case conversion using Humanizer library
Properties/launchSettings.json # Development launch profiles (HTTP:5250, HTTPS:7271)
appsettings.json          # Application configuration (Logging, AllowedHosts)
MyCoolApi.csproj         # Project file (.NET 9.0, Humanizer, Swagger, OpenAPI packages)
```

### Test Project Files (MyCoolApi.Tests/)
```
MathTests.cs             # Math endpoint integration tests + unit tests  
HelloTests.cs           # Greeting endpoint integration tests
StringCasingTests.cs    # String casing conversion tests (unit + integration)
WeatherForecastTests.cs # Weather API tests with JSON deserialization
EnvironmentTests.cs     # Environment and OS information tests
ErrorHandlingTests.cs   # Error scenario validation
OSTests.cs             # Platform-specific OS detection tests
MyCoolApiApp.cs        # WebApplicationFactory<Program> for integration testing
MyCoolApi.Tests.csproj # Test project (.NET 9.0, MSTest, AspNetCore.Mvc.Testing, MTP)
```

### Expected Build Times and Commands
```bash
# Basic project build (simple libraries without external dependencies)
$ dotnet build ClassLib2/ClassLib2.csproj --no-restore
# Expected: 1-2 seconds, "Build succeeded"

# Full solution restore (in normal environment)
$ dotnet restore  
# Expected: 15-30 seconds with network access
# Known issue: SSL certificate errors in sandboxed environments

# Full solution build
$ dotnet build --configuration Release --no-restore
# Expected: 1-3 minutes, may fail if restore didn't complete
# Warning: "You are using a preview version of .NET" (expected with .NET 10.x SDK)

# Full test suite
$ dotnet test --configuration Release --no-build
# Expected: 2-5 minutes for complete test run
# Includes integration tests that start in-memory web servers
```

## Troubleshooting

### Build Issues
- If `dotnet restore` fails with SSL errors: Known limitation in certain environments, document but don't fix
- If build fails after restore success: Check project references and PackageReference versions
- If projects target different .NET versions: ClassLib2 and ClassLibrary1 use .NET 10.0, others use .NET 9.0

### Test Issues  
- If tests fail to start: Check that MyCoolApiApp.cs correctly inherits from WebApplicationFactory<Program>
- If integration tests fail: Ensure the test application can resolve all dependencies
- If tests are slow: Microsoft Testing Platform is slower than traditional MSTest runner but provides better output

### Runtime Issues
- Default ports: HTTP 5250, HTTPS 7271
- Swagger available in development at `/swagger`
- Check launchSettings.json for all available launch profiles
- Docker support available but requires proper SSL certificate handling