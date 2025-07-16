# Code Coverage in .NET with xUnit and Coverlet

This guide documents how to set up code coverage reporting in a .NET 8 project using xUnit and coverlet, how to generate reports, and common pitfalls with solutions. It applies to local development (Rider, VS Code, CLI) and can be extended to CI.

## 1. For Students Who Clone the Repository

If you cloned this repository and the unit test project is already set up, you do NOT need to create a new test project. Simply follow the usage steps below (e.g., `dotnet restore`, `dotnet test`, etc.).  
If you encounter dependency issues, run `dotnet restore` in the test project directory.

## 2. How to Create a Test Project (CLI & IDE)

You can create a test project using either the CLI or a graphical IDE:

- **CLI (Recommended for most users):**

  1. Open a terminal in your solution directory.
  2. Run:
     ```bash
     dotnet new xunit -n YourTestProjectName
     ```
  3. This will create a new xUnit test project with all necessary dependencies.

- **Visual Studio:**

  1. Right-click the solution or project, select "Add" > "New Project".
  2. Search for "xUnit Test Project" or ".NET Unit Test Project" and follow the wizard.
  3. The generated test project will include the necessary dependencies by default.

- **JetBrains Rider:**
  1. Right-click the solution, select "New" > "Project".
  2. Choose ".NET" > "xUnit Test Project".
  3. Complete the wizard to create the project.

Creating test projects via CLI or IDEs will set up dependencies and structure automatically, making it suitable for both command-line and graphical interface users.

## 3. Prerequisites

- .NET SDK 8.0 or later
- CLI access (Terminal, PowerShell, etc.)

## 4. Required Dependencies

Run these in your test project folder (e.g., `HelloCity.Tests`):

```bash
dotnet add package Microsoft.NET.Test.Sdk

dotnet add package xunit

dotnet add package xunit.runner.visualstudio

dotnet add package coverlet.collector
```

`coverlet.collector` enables collecting code coverage data during test runs.

Update your `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.10.0" />
  <PackageReference Include="xunit" Version="2.5.0" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.5.0" />
  <PackageReference Include="coverlet.collector" Version="6.0.0" />
</ItemGroup>
```

## 5. Run Tests with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

This will generate a coverage report under:

```
HelloCity.Tests/TestResults/<GUID>/coverage.cobertura.xml
```

## 6. Install Report Generator (Optional but Recommended)

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

Ensure `$HOME/.dotnet/tools` is in your PATH:

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```

## 7. Minimal Unit Test Example

Here is a minimal example to get you started with unit testing in .NET using xUnit. This example demonstrates how to test a simple sum function.

Suppose you have the following service class (`UnitTestService.cs`):

```csharp
// UnitTestService.cs
namespace HelloCity.Services;

public class UnitTestService
{
    public int SumInt(int a, int b)
    {
        return a + b;
    }
}
```

You can write a corresponding test class (`UnitTest.cs`):

```csharp
// UnitTest.cs
using HelloCity.Services;
using Xunit;

namespace HelloCity.Tests;

public class TestUserServiceTest
{
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(-1, -2, -3)]
    [InlineData(0, 0, 0)]
    [InlineData(1000, 2000, 3000)]
    public void SumInt_ReturnsCorrectSum(int a, int b, int expected)
    {
        var unitTest = new UnitTestService();
        var result = unitTest.SumInt(a, b);
        Assert.Equal(expected, result);
    }
}
```

This test checks that the `SumInt` method returns the correct sum for various input values. You can run this test using the steps described in the next section.

## 8. Generate HTML Report

> Note: Every time you want to view the latest coverage report, you need to re-run the report generation command below to update the report.

```bash
reportgenerator \
  -reports:"**/coverage.cobertura.xml" \
  -targetdir:"coverage-report" \
  -reporttypes:Html
```

To view the report:

- macOS: `open coverage-report/index.html`
- Windows: `start coverage-report/index.html`

You can also generate other formats:

```bash
-reporttypes:"Html;TextSummary;Badges"
```

## 9. Directory Layout Example

```
/HelloCity.Tests
├── TestUserServiceTests.cs
├── TestResults/
│   └── .../coverage.cobertura.xml
└── coverage-report/
    └── index.html
```

## 10. Common Issues & Fixes

### 1) `reportgenerator: command not found`

**macOS / Linux:**
Add to your shell config:

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```

Then restart your terminal or run `source ~/.zshrc`

**Windows (usually auto-configured):**
If not recognized, make sure this path is included in your PATH environment variable:

```
%USERPROFILE%\.dotnet\tools
```

### 2) Coverage is 0% even though tests run

- Your test may not call real logic (mocking too much, or wrong DI)
- Ensure the tested code is actually executed during the test

## 11. Summary: Key Commands

| Task                       | Command                                                                                               |
| -------------------------- | ----------------------------------------------------------------------------------------------------- |
| Install coverage tool      | `dotnet add package coverlet.collector`                                                               |
| Run tests with coverage    | `dotnet test --collect:"XPlat Code Coverage"`                                                         |
| Install report generator   | `dotnet tool install --global dotnet-reportgenerator-globaltool`                                      |
| Generate HTML report       | `reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html` |
| Open HTML report (macOS)   | `open coverage-report/index.html`                                                                     |
| Open HTML report (Windows) | `start coverage-report/index.html`                                                                    |

## 12. Final Notes

- You don't need HTML reports to use coverage – you can still read `coverage.cobertura.xml` manually or upload it to Codecov.
- Rider supports Run All Tests with Coverage via GUI – no CLI needed.
- Keep your business logic and test logic clearly separated to improve testability.

Happy Testing!
