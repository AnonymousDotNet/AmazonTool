# 贡献指南（CONTRIBUTING）

感谢你对 AmazonTool 项目的关注与支持！  
我们欢迎各种形式的贡献，包括代码、文档、测试、建议等。

---

## 如何贡献

### 1. Fork 本仓库
点击页面右上角的 **Fork** 按钮，并克隆到本地：

```sh
git clone https://github.com/YOUR_USERNAME/AmazonTool.git
cd AmazonTool
```

### 2. 创建新分支
请使用有意义的分支名：

```sh
git checkout -b feature/your-feature-name
```

### 3. 进行修改
- 保持代码整洁、注释清晰。
- 涉及用户界面或功能变更时，建议补充或更新相关测试。
- 重要特性或修复请同步更新文档（如 `README.md`, `README.en.md`, `README.zh.md` 等）。
- 提交前请确保不破坏现有构建或测试。

### 4. 提交并推送
请使用清晰简洁的提交信息：

```sh
git add .
git commit -m "新增: 功能描述"
git push origin feature/your-feature-name
```

### 5. 提交 Pull Request
在你的 Fork 仓库页面点击 **New Pull Request**。  
请详细描述你的变更，关联相关 Issue（如有），并提交审核。

---

## 代码风格与规范

- **语言**：C#（客户端和后端）、Markdown（文档）
- **格式化**：遵循 `.editorconfig` 规范（如有）
- **命名**：变量、类、函数请使用有意义的英文名称
- **注释**：对复杂逻辑适当添加注释说明
- **测试**：建议为新增或变更功能补充/更新测试
- **文档**：如有用户可见或 API 变更，请同步更新相关文档

---

## Bug 反馈与功能建议

- 请通过 [GitHub Issues](https://github.com/AnonymousDotNet/AmazonTool/issues) 提交
- 请包含以下内容（如适用）：
  - 复现步骤（对于 Bug）
  - 期望行为
  - 截图或日志（如有）
  - 环境信息（操作系统、.NET 版本等）

---

## 行为准则

本项目遵循 [Contributor Covenant](https://www.contributor-covenant.org/) 行为准则。  
请在交流中保持尊重、包容。

---

## 获取帮助

- 如有疑问可新建 Issue，或参与讨论
- 也可通过 [http://anonymousdotnet.cn](http://anonymousdotnet.cn) 联系维护者

---

感谢你的贡献，让 AmazonTool 变得更好！