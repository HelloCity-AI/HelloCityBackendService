[![中文说明](https://img.shields.io/badge/文档-中文-blue?style=flat-square)](./README.zh-CN.md)

# Hello City Server

A modern backend project based on .NET 8, ASP.NET Core, and a layered architecture (Api/Services/Models/IServices), supporting PostgreSQL database, suitable for rapid development and deployment.

## Requirements

- .NET SDK: **8.0 or above**
- PostgreSQL: **16 or above** (recommended to use Docker Compose)
- Docker Desktop: **4.43.1 or above**
- Recommended OS: Windows, macOS, Linux

## Getting Started

1. **Clone the project and enter the directory:**

   ```bash
   git clone <repo-url/ssh>
   cd HelloCityBackendService
   ```

2. **Start the database:**

   ```bash
   docker compose up
   ```

   > The API service in compose.yaml is currently commented out, so only the database service will be started.
   > If you want to start the API with Docker, uncomment the relevant lines in compose.yaml, refer to step 6, and restart compose.
   > For development and debugging, it is recommended to use dotnet run to start the API directly.

3. **Initialize the database table and insert test data:**

   Use DBeaver, Navicat, or other database clients to connect to Postgres, select the `HelloCityDb` database, and execute the following SQL:

   ```sql
   CREATE TABLE IF NOT EXISTS test (
     "Id" SERIAL PRIMARY KEY,
     "Email" VARCHAR(255) NOT NULL,
     "Password" VARCHAR(255) NOT NULL
   );

   INSERT INTO test ("Email", "Password") VALUES
     ('huachi@123.com', 'dadsa'),
     ('huachi@123.com', 'dadsa'),
     ('huachi@123.com', 'dadsa');
   ```

   > Default database connection info (see `HelloCity.Api/appsettings.Development.json` and `compose.yaml`):
   >
   > - Host: `localhost`
   > - Port: `5432`
   > - Database: `HelloCityDb`
   > - Username: `root`
   > - Password: `root123`

4. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

5. **Run the API (recommended for development/debugging):**

   ```bash
   cd HelloCity.Api
   dotnet run
   ```

   - Default listening port: `http://localhost:5000`.
   - This is the recommended way for development/debugging (hot reload, friendly logs).

6. **Run the API with Docker (optional):**

   - Since the API service is commented out in compose.yaml, uncomment the relevant lines if you want to use Docker.
   - When using Docker, host port `5050` maps to container port `8080`, i.e.:
     - Access at `http://localhost:5050`
   - Make sure ASP.NET Core listens on port 8080 in the container (`EXPOSE 8080` is set in Dockerfile).
   - **Note: If using Docker, you must hardcode the listening port to 8080 in `Program.cs` before `builder.Build()`. Recommended Kestrel config:**

     ```csharp
     builder.WebHost.ConfigureKestrel(options =>
     {
         options.ListenAnyIP(8080);
     });
     ```

7. **Verify API and database connection:**

   - If using dotnet run, visit [http://localhost:5000/api/TestUser](http://localhost:5000/api/TestUser)
   - If using Docker, visit [http://localhost:5050/api/TestUser](http://localhost:5050/api/TestUser)
   - You should see the test data you inserted.

## How to run unit test

This project integrates xUnit unit tests, with test code located in the `HelloCity.Tests` directory. You can run and view unit test results as follows:

1. **Restore dependencies** (if not already done):

   ```bash
   dotnet restore
   ```

2. **Run all unit tests**:

   ```bash
   dotnet test
   ```

   You will see detailed output for passed/failed tests.

3. **View test code example (A minimal test code was created already for learning purpose)**

   For example, `UnitTest.cs` tests the `UnitTestService.SumInt` method:

   ```csharp
   [Theory]
   [InlineData(1, 2, 3)]
   [InlineData(-1, -2, -3)]
   public void SumInt_ReturnsCorrectSum(int a, int b, int expected)
   {
       var unitTest = new UnitTestService();
       var result = unitTest.SumInt(a, b);
       Assert.Equal(expected, result);
   }
   ```

   You can add more test cases in the `HelloCity.Tests` directory to cover your business logic.

4. **Generate code coverage report (optional)**

   If you want to check code coverage, run:

   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

   The coverage report will be generated under the `HelloCity.Tests/TestResults` directory.

5. **Generate and view HTML coverage report (optional)**

   (1) **Install report generator tool globally** (run once):

   ```bash
   dotnet tool install --global dotnet-reportgenerator-globaltool
   ```

   Make sure `$HOME/.dotnet/tools` is in your PATH (for macOS/Linux, add to your shell config):

   ```bash
   export PATH="$PATH:$HOME/.dotnet/tools"
   ```

   (2) **Generate HTML report**:

   ```bash
   reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
   ```

   (3) **Open HTML report**:

   - macOS: `open coverage-report/index.html`
   - Windows: `start coverage-report/index.html`

   **Common issue: `reportgenerator: command not found`**

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

For more detailed instructions, see `HelloCity.Tests/unit-test.md`.

## Tech Stack

- [.NET 8](https://dotnet.microsoft.com/)
- [ASP.NET Core](https://learn.microsoft.com/aspnet/core)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker Compose](https://docs.docker.com/compose/)
- [Swagger](https://swagger.io/) (auto-generated API docs)
- [xUnit](https://xunit.net/) (unit testing)
- [coverlet](https://github.com/coverlet-coverage/coverlet) (code coverage)
- [ReportGenerator](https://danielpalme.github.io/ReportGenerator/) (coverage HTML report)

## Project Structure

- `HelloCity.Api/`: Web API entry, controllers, configs, startup files, etc.
- `HelloCity.IServices/`: Interface definitions
- `HelloCity.Services/`: Service implementations
- `HelloCity.Models/`: Data models and config classes
- `HelloCity.Middleware/`: Middleware extensions (if any)
- `HelloCity.Tests/`: Unit tests and code coverage reports
- `compose.yaml`: Docker Compose config (Postgres, etc.)
- `hello-city-server.sln`: Solution file

## Notes

- Swagger is enabled by default in development, visit `/swagger` for API docs.
- Only `TestUser` related APIs and services are for testing; extend with real business logic as needed.
- For development/debugging, prefer dotnet run; Docker is mainly for deployment or integration testing.
- To customize DB connection, ports, etc., edit `HelloCity.Api/appsettings.Development.json` or set environment variables.

For further improvements or questions, feel free to ask!
