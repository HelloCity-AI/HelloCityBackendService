[![English Version](https://img.shields.io/badge/Docs-English-green?style=flat-square)](./README.md)

# HelloCity Backend Service

一个基于 .NET 8 的现代化后端服务，提供用户管理和清单功能，采用分层架构设计，支持 JWT 认证和 PostgreSQL 数据库。

## 快速开始

### 环境要求

- **.NET SDK**: 8.0 或更高版本
- **PostgreSQL**: 16 或更高版本
- **Docker Desktop**: 4.43.1 或更高版本
- **操作系统**: Windows, macOS, Linux

### 1. 克隆项目

```bash
git clone https://github.com/HelloCity-AI/HelloCityBackendService.git
cd HelloCityBackendService
```

### 2. 启动数据库

```bash
docker compose up
```

> API 服务在 compose.yaml 中已注释，仅启动数据库。开发调试建议使用 `dotnet run`。

### 3. 配置数据库连接

**重要：不要提交真实密码到代码库！**

```bash
# 复制配置文件模板
cp HelloCity.Api/appsettings.Development.json.example HelloCity.Api/appsettings.Development.json
```

然后编辑 `appsettings.Development.json` 填入你的数据库信息。请联系团队获取开发环境的数据库连接信息。

> `appsettings.Development.json` 已加入 .gitignore，永远不会被提交到仓库

### 4. 初始化数据库

使用数据库客户端连接到 PostgreSQL，执行：

```sql
CREATE TABLE IF NOT EXISTS test (
  "Id" SERIAL PRIMARY KEY,
  "Email" VARCHAR(255) NOT NULL,
  "Password" VARCHAR(255) NOT NULL
);

INSERT INTO test ("Email", "Password") VALUES
  ('test@example.com', 'testpass'),
  ('demo@example.com', 'demopass');
```

### 5. 数据库迁移

```bash
# 添加迁移
dotnet ef migrations add InitialCreate --project HelloCity.Models

# 更新数据库
dotnet ef database update --project HelloCity.Api
```

### 6. 运行项目

```bash
# 安装依赖
dotnet restore

# 启动 API 服务
cd HelloCity.Api
dotnet run
```

访问 http://localhost:5000/swagger 查看 API 文档

## 运行测试

### 单元测试

```bash
# 运行所有测试
dotnet test

# 生成覆盖率报告
dotnet test --collect:"XPlat Code Coverage"
```

### 生成 HTML 覆盖率报告

```bash
# 1. 安装报告生成工具（首次运行）
dotnet tool install --global dotnet-reportgenerator-globaltool

# 2. 生成报告
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

# 3. 查看报告
open coverage-report/index.html  # macOS
start coverage-report/index.html # Windows
```

## API 接口

### 用户管理

- **GET** `/api/user-profile/{id}` - 获取用户信息
- **POST** `/api/user-profile` - 创建用户
- **PUT** `/api/user-profile/{id}` - 更新用户信息
- **POST** `/api/user-profile/{userId}/checklist-item` - 为用户创建清单项
- **GET** `/api/user-profile/{userId}/checklist-item` - 获取用户的清单

### 测试接口

- **GET** `/api/TestUser` - 测试数据库连接（需要 JWT 认证）

详细的 API 文档请访问 `/swagger` 页面。

## 项目结构

```
HelloCity/
├── HelloCity.Api/           # API 层 - 控制器、配置和 DTOs
├── HelloCity.Services/      # 业务逻辑层
├── HelloCity.Repository/    # 数据访问层
├── HelloCity.IRepository/   # 仓储接口
├── HelloCity.IServices/     # 服务接口
├── HelloCity.Models/        # 实体模型和数据库上下文
├── HelloCity.Tests/         # 测试项目
└── compose.yaml            # Docker 配置
```

### 设计模式

- **分层架构**: API → Services → Repository → Database
- **依赖注入**: 所有服务通过 DI 容器管理
- **仓储模式**: 数据访问抽象化
- **DTO 模式**: 数据传输对象在 API 层处理

## 技术栈

### 核心框架

- **.NET 8** - 运行时框架
- **ASP.NET Core** - Web API 框架
- **Entity Framework Core** - ORM
- **PostgreSQL** - 数据库

### 开发工具

- **AutoMapper** - 对象映射
- **FluentValidation** - 数据验证
- **Serilog** - 结构化日志
- **Swagger** - API 文档

### 认证授权

- **JWT Bearer** - 令牌认证
- **Auth0** - 身份验证服务

### 测试框架

- **xUnit** - 单元测试框架
- **Moq** - 模拟对象框架
- **FluentAssertions** - 流畅的断言库
- **Coverlet** - 代码覆盖率
- **ReportGenerator** - 覆盖率报告

### 部署工具

- **Docker** - 容器化
- **Docker Compose** - 服务编排

## 开发配置

### 日志查看

日志文件保存在 `Logs/` 目录，按天滚动保留 7 天。

### CORS 配置

默认允许来自 `http://localhost:3000` 的前端访问。

## 开发规范

### 分支命名

```bash
git checkout -b SCRUM-123-feature-name
```

### 代码提交

- 遵循 [Conventional Commits](https://www.conventionalcommits.org/) 规范
- 每次提交前运行测试确保通过

### 配置文件

- 提交 `appsettings.json` 和 `appsettings.Development.json.example`
- 绝不提交 `appsettings.Development.json`（包含真实密码）

## 部署说明

### Docker 部署

```bash
# 取消注释 compose.yaml 中的 API 服务配置
docker compose up
# 访问 http://localhost:5050
```

### 生产环境变量

- `ConnectionStrings__DefaultConnection` - 数据库连接串
- `JWT__Authority` - JWT 验证地址
- `JWT__Audience` - JWT 受众

---

最后更新：2025-08-16
