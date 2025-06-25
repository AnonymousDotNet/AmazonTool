# AmazonTool

> 📄 简体中文 | [English](./README.en.md)

AmazonTool 是面向亚马逊店铺开发者的调试与测试辅助工具，帮助开发者、运营及测试人员便捷完成店铺授权、数据获取、权限密钥生成和API调试。项目采用模块化、轻量级和易用性设计，支持客户端与后端服务解耦，便于未来多端扩展。

---

## 📝 项目简介

亚马逊平台API开发和测试繁琐且门槛较高。AmazonTool 致力于为个人及团队开发者提供一站式、本地化的辅助工具，让店铺授权对接、API测试与数据管理变得高效而简单。

---

## 🚀 核心功能路线图

- [ ] 店铺授权对接：快速录入并授权亚马逊店铺账号。
- [ ] 获取店铺信息：一键拉取并展示店铺基础数据。
- [ ] 权限密钥生成：根据店铺信息自动生成API权限密钥，简化接口调用。
- [ ] 持久化测试数据：保存API调试/测试的输入输出及历史记录，便于回溯与复用。

---

## ✨ 主要特性

- 简洁直观的桌面客户端（WinForm/WPF，规划逐步实现）
- 统一后端API服务，便于多端复用
- 轻量级数据库持久化（SQLite/PostgreSQL，按需选择）
- 易于配置，开箱即用
- 专注于“测试/调试”场景，无需复杂集成

---

## 🏗️ 技术选型

- **前端/客户端**：WinForm（首版），WPF（后续），计划支持Web端
- **后端**：.NET API服务，HTTP接口
- **数据库**：首选SQLite，支持PostgreSQL
- **开发语言**：C#
- **开发工具**：Git、Visual Studio、Visual Studio Code

---

## 📦 安装与运行

### 克隆仓库

```sh
git clone https://github.com/AnonymousDotNet/AmazonTool.git
cd AmazonTool
```

### 构建并运行（以 WinForm 客户端为例）

```sh
cd src/AmazonTool.WinForm
dotnet build
dotnet run
```

### 启动后端API服务

```sh
cd src/AmazonTool.Api
dotnet build
dotnet run
```

> 详细依赖与配置请参见 [INSTALL.md](INSTALL.zh.md) 或各模块内文档。

---

## 🖼️ 项目结构

```
AmazonTool (解决方案)
│
├── AmazonTool.WinForm       // WinForm客户端
├── AmazonTool.WPF           // WPF客户端（可选）
├── AmazonTool.Api           // 后端API服务
├── AmazonTool.Api.Tests     // 后端测试
├── docs                     // 文档文件夹（可在资源管理器新建）
├── tests                    // 其他测试项目（可在资源管理器新建）
└── README.md                // 项目说明
```
---

## 🛠️ 贡献指南

欢迎任何形式的贡献！请在提交 PR 前阅读 [贡献](CONTRIBUTING.zh.md) or [CONTRIBUTING](CONTRIBUTING.en.md) 并确保所有测试通过。

---

## 💡 未来展望

- 支持更多亚马逊API场景，提供自动化测试脚本与结果分析
- 完善 WPF/Web 客户端，提升跨平台体验
- 丰富数据导出与历史回溯功能
- 探索云端部署与多端协作
- 支持多店铺、多账号统一管理
- 集成常用亚马逊运营工具链，打造一体化开发者平台

---

## 📃 License

本项目采用 [Apache License 2.0](LICENSE)。

---

## 🙋 联系与讨论

- 问题反馈：[AmazonTool Issues](https://github.com/AnonymousDotNet/AmazonTool/issues)
- 个人博客：[http://anonymousdotnet.cn](http://anonymousdotnet.cn)
- Twitter/X：[lld477403216357](https://x.com/lld477403216357)

---

> “思考不能克服恐惧，但行动可以”  
> —— 感谢关注与支持，欢迎共建和交流！
