# AmaTool.Api - 后端 API 服务

本项目为 AmaTool 的后端 Web API，负责任务调度、用户管理、日志收集等功能。

## 功能简介

- 提供 RESTful API 接口
- 任务调度与执行管理
- 用户认证与权限控制
- 日志与任务追踪

## 环境依赖

- .NET 8.0 SDK
- PostgreSQL 或 SQL Server 数据库

## 快速部署

### 本地运行

1. 配置数据库连接（`appsettings.json`）
2. dotnet CLI 运行：
   ```bash
   cd AmaTool.Api
   dotnet run
   ```
3. Swagger 文档地址： `http://localhost:5000/swagger`

### Docker 部署

```bash
docker build -t amatool-api .
docker run -d -p 5000:80 --env-file .env amatool-api
```

## API 文档

- 启动服务后访问 `/swagger` 获取详细接口说明

## 迁移与初始化

- 使用 Entity Framework Core 管理数据库迁移
- 首次启动前可执行：
  ```bash
  dotnet ef database update
  ```

## 参与贡献

- 按照主项目代码规范提交 PR
- 强烈建议增加单元测试覆盖
