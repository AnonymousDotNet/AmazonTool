# AmaTool.Wpf.Tests - WPF 客户端测试项目

本项目用于 AmaTool.Wpf 的自动化测试，确保 WPF 客户端功能的正确性和稳定性。

## 测试类型

- 单元测试：验证各模块/组件的业务逻辑
- UI 测试（可选）：借助工具如 xUnit + White 等进行界面自动化

## 运行要求

- .NET 8.0 SDK
- 推荐使用 Visual Studio 2022 或 JetBrains Rider

## 如何运行

在项目根目录下执行：

```bash
dotnet test AmaTool.Wpf.Tests
```

或在 IDE 中右键“运行所有测试”。

## 结构说明

- `Tests/`：包含所有测试用例
- `Mocks/`：测试用到的模拟对象或数据
- `TestHelpers/`：通用测试辅助类

## 覆盖率

建议通过如 Coverlet、ReportGenerator 集成测试覆盖率报告。

## 贡献建议

- 保证新增或修改功能有相应测试用例覆盖
- PR 需通过所有自动化测试

---

其他测试项目（如 AmaTool.Api.Tests、AmaTool.Common.Tests 等）可参考此模板，重点部分根据项目类型微调说明即可。