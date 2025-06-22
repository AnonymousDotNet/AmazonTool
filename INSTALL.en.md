# Installation & Running Guide

Welcome to AmazonTool! This guide will help you build and run the project locally.

---

## 1. Requirements

- Operating System: Windows 10/11 (recommended), macOS/Linux (coming soon)
- .NET 6.0 SDK or higher
- Git
- Optional: Visual Studio 2022 / Visual Studio Code
- Optional: SQLite or PostgreSQL (for persistence features)

---

## 2. Clone the Repository

```sh
git clone https://github.com/AnonymousDotNet/AmazonTool.git
cd AmazonTool
```

---

## 3. Build the Project

### 3.1 Build the WinForm Client

```sh
cd src/AmazonTool.WinForm
dotnet build
```

### 3.2 Build the Backend API Service

```sh
cd src/AmazonTool.Api
dotnet build
```

---

## 4. Run the Project

### 4.1 Start the Backend API Service

```sh
cd src/AmazonTool.Api
dotnet run
```
The default listening port can be configured in `appsettings.json`.

### 4.2 Start the WinForm Client

Open a new terminal:

```sh
cd src/AmazonTool.WinForm
dotnet run
```

---

## 5. Database Configuration (Optional)

The project uses SQLite by default (no manual installation required).  
To use PostgreSQL, modify the connection string in the backend's `appsettings.json` and create the database in advance.

---

## 6. Troubleshooting

- If you encounter missing dependencies, port conflicts, etc., please ensure .NET SDK is installed, or see [FAQ](docs/FAQ.md).
- For further questions, please submit via [Issues](https://github.com/AnonymousDotNet/AmazonTool/issues).

---

## 7. More

- For more details, see [README.md](README.md) and the [docs/](docs) directory.
- Contributions are welcome! See [CONTRIBUTING.en.md](CONTRIBUTING.en.md) for details.

---