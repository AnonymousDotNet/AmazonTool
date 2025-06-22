# AmazonTool

AmazonTool 是一个聚焦于亚马逊店铺开发者的调试和测试辅助工具，旨在帮助开发者、运营和测试人员便捷地管理店铺授权、店铺数据获取、权限密钥生成及API调试。项目以模块化架构、极简易用为原则，支持桌面客户端与后端服务解耦，方便未来多端拓展。

---

## 📝 项目简介

亚马逊平台的API开发和测试流程冗长、门槛高。AmazonTool 立志于为个人开发者和团队提供一站式、低门槛的本地辅助工具，让店铺授权对接、API测试、数据管理变得简单高效。

---

## 🚀 核心功能路线图（Roadmap）

- [ ] 店铺授权对接：快速录入并授权亚马逊店铺账号。
- [ ] 获取店铺信息：一键拉取并展示店铺基础信息。
- [ ] 权限密钥生成：根据店铺信息自动生成API权限密钥，简化后续接口调用。
- [ ] 持久化测试数据：保存API调试/测试的输入输出和历史记录，便于追溯与复用。

---

## ✨ 主要特性

- 简洁直观的桌面客户端（WinForm/WPF，计划逐步实现和切换）
- 统一后端API服务，方便多端复用
- 轻量级数据库持久化（SQLite/PostgreSQL，视场景灵活切换）
- 简单配置，无需冗余依赖，开箱即用
- 重点环绕“测试/调试”场景，无须繁琐正式集成

---

## 🏗️ 技术选型

- **前端/客户端**：WinForm（第一版本），WPF（后续升级），计划后续拓展Web端
- **后端**：.NET API 服务，HTTP接口
- **数据库**：SQLite 优先，后续支持 PostgreSQL
- **开发语言**：C#
- **其他工具**：Git, Visual Studio, Visual Studio Code

---

## 📦 安装与运行

### 克隆仓库

```sh
git clone https://github.com/AnonymousDotNet/AmazonTool.git
cd AmazonTool
```

### 构建与运行（以 WinForm 客户端为例）

```sh
cd src/AmazonTool.WinForm
dotnet build
dotnet run
```

### 后端API服务启动

```sh
cd src/AmazonTool.Api
dotnet build
dotnet run
```

> 详细环境依赖与配置请见 [INSTALL.md](INSTALL.md) 或各模块内说明文档。

---

## 🖼️ 项目结构

```
AmazonTool/
├── docs/                 # 说明文档与开发笔记
├── src/
│   ├── AmazonTool.WinForm/   # WinForm 客户端源码
│   ├── AmazonTool.WPF/       # WPF 客户端源码（规划中）
│   └── AmazonTool.Api/       # 后端API服务源码
├── tests/                # 单元与集成测试
├── README.md
├── ...
```

---

## 🛠️ 贡献指南

欢迎任何形式的贡献，包括但不限于代码、文档、测试用例、功能建议等。  
请提交 PR 前先阅读 [CONTRIBUTING.md](CONTRIBUTING.md) 并确保所有单元测试通过。

---

## 💡 未来展望与计划

- 支持更多亚马逊API场景，提供自动化测试脚本及结果分析
- 全面支持 WPF/WEB 客户端，提升跨平台体验
- 丰富数据导出和历史回溯功能
- 考虑云端部署与多端协同操作
- 支持多店铺、多账号集中管理
- 集成常用亚马逊运营工具链，打造亚马逊开发者一体化效率平台

---

## 📃 License

本项目采用 [MIT License](LICENSE)。

---

## 🙋 联系与讨论

- 反馈建议：请通过 [Issues](https://github.com/AnonymousDotNet/AmazonTool/issues) 提交
- 个人博客：[http://anonymousdotnet.cn](http://anonymousdotnet.cn)
- Twitter/X：[lld477403216357](https://x.com/lld477403216357)

---

> “Thinking will not overcome fear but action will.”  
> —— 感谢关注和支持，欢迎共建和交流！
