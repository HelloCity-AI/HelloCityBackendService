[![English Version](https://img.shields.io/badge/Docs-English-green?style=flat-square)](./README.md)

# Hello City Server

一个基于 .NET 8、ASP.NET Core、分层架构（Api/Services/Models/IServices）的现代后端项目，支持 PostgreSQL 数据库，适合快速开发和部署。

## 环境要求

- .NET SDK：**8.0 及以上**
- PostgreSQL：**16 及以上**（推荐用 Docker Compose 启动）
- Docker Desktop：**4.43.1 及以上**
- 推荐操作系统：Windows、macOS、Linux

## 快速开始

1. **克隆项目并进入目录：**

   ```bash
   git clone <repo-url/ssh>
   cd HelloCityBackendService
   ```

2. **启动数据库：**

   ```bash
   docker compose up
   ```

   > 目前 compose.yaml 文件中已注释掉 API 服务的 Docker 启动配置，仅会启动数据库服务。
   > 如需用 Docker 启动 API，请取消 compose.yaml 中相关注释，参考步骤 6 后重新启动 compose。
   > 推荐开发调试时直接用 dotnet run 启动 API。

3. **初始化数据库表和插入测试数据：**

   使用 DBeaver、Navicat 或其他数据库客户端，连接到 Postgres，选择 `HelloCityDb` 数据库，执行以下 SQL：

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

   > 默认数据库连接信息（见 `HelloCity.Api/appsettings.Development.json` 和 `compose.yaml`）：
   >
   > - Host: `localhost`
   > - Port: `5432`
   > - Database: `HelloCityDb`
   > - Username: `root`
   > - Password: `root123`

4. **还原依赖包：**

   ```bash
   dotnet restore
   ```

5. **运行后端 API（推荐开发调试方式）：**

   ```bash
   cd HelloCity.Api
   dotnet run
   ```

   - 默认监听端口为 `http://localhost:5000`。
   - 推荐开发调试时直接使用此方式，热重载、日志友好。

6. **使用 Docker 部署 API（可选）：**

   - 由于 compose.yaml 默认注释了 API 服务，如需用 Docker 部署，请取消相关注释。
   - Docker 部署时，主机端口 `5050` 映射到容器 `8080`，即：
     - 访问地址为 `http://localhost:5050`
   - 需确保 ASP.NET Core 在容器内监听 8080 端口（Dockerfile 已配置 EXPOSE 8080）。
   - **注意：如用 Docker 部署，需在 `Program.cs` 中将监听端口硬编码为 8080，否则端口映射无效。推荐在 `builder.Build()` 之前添加如下 Kestrel 配置：**

     ```csharp
     builder.WebHost.ConfigureKestrel(options =>
     {
         options.ListenAnyIP(8080);
     });
     ```

7. **验证接口与数据库连接：**

   - 如果用 dotnet run，访问 [http://localhost:5000/api/TestUser](http://localhost:5000/api/TestUser)
   - 如果用 Docker，访问 [http://localhost:5050/api/TestUser](http://localhost:5050/api/TestUser)
   - 应返回你刚插入的测试数据。

---

## 主要技术栈

- [.NET 8](https://dotnet.microsoft.com/)
- [ASP.NET Core](https://learn.microsoft.com/aspnet/core)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker Compose](https://docs.docker.com/compose/)
- [Swagger](https://swagger.io/)（自动生成 API 文档）

## 目录结构简述

- `HelloCity.Api/`：Web API 项目入口，控制器、配置、启动文件等
- `HelloCity.IServices/`：接口定义层
- `HelloCity.Services/`：服务实现层
- `HelloCity.Models/`：数据模型与配置项
- `HelloCity.Middleware/`：中间件扩展（如有）
- `compose.yaml`：Docker Compose 配置（Postgres 数据库等）
- `hello-city-server.sln`：解决方案文件

## 其他说明

- 项目内置 Swagger，开发环境自动启用，访问 `/swagger` 可查看接口文档。
- 仅 `TestUser` 相关接口和服务为测试用途，后续可扩展实际业务逻辑。
- 推荐开发调试时优先使用 dotnet run，Docker 主要用于部署或集成测试。
- 如需自定义数据库连接、端口等配置，请修改 `HelloCity.Api/appsettings.Development.json` 或相关环境变量。

如需进一步完善或有其他需求，欢迎继续补充！
