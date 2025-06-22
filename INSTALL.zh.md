# 安装与运行指南

欢迎使用 AmazonTool！本指南将帮助你在本地环境中构建和运行本项目。

---

## 1. 环境要求

- 操作系统：Windows 10/11（推荐），支持 macOS/Linux（后续支持中）
- .NET 6.0 SDK 或更高版本
- Git
- 可选：Visual Studio 2022 / Visual Studio Code
- 可选：SQLite 或 PostgreSQL（如需持久化功能）

---

## 2. 克隆仓库

```sh
git clone https://github.com/AnonymousDotNet/AmazonTool.git
cd AmazonTool
```

---

## 3. 构建项目

### 3.1 构建 WinForm 客户端

```sh
cd src/AmazonTool.WinForm
dotnet build
```

### 3.2 构建后端 API 服务

```sh
cd src/AmazonTool.Api
dotnet build
```

---

## 4. 运行项目

### 4.1 启动后端 API 服务

```sh
cd src/AmazonTool.Api
dotnet run
```
默认监听端口可在 `appsettings.json` 中配置。

### 4.2 启动 WinForm 客户端

另开终端：

```sh
cd src/AmazonTool.WinForm
dotnet run
```

---

## 5. 数据库配置（可选）

项目默认使用 SQLite（无需手动安装），如需使用 PostgreSQL，请修改后端配置文件 `appsettings.json` 中的连接字符串，并提前创建数据库。

---

## 6. 常见问题

- 如遇依赖缺失、端口冲突等问题，请先确认 .NET SDK 已正确安装，或查阅 [常见问题文档](docs/FAQ.md)。
- 如有其他问题，欢迎通过 [Issues](https://github.com/AnonymousDotNet/AmazonTool/issues) 反馈。

---

## 7. 其他

- 更多开发与部署细节请查阅 [README.md](README.md) 及 [docs/](docs) 目录。
- 欢迎贡献代码与建议，参与方式见 [CONTRIBUTING.md](CONTRIBUTING.md)。

---