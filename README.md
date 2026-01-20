# unused-packages

A fast and helpful CLI tool to identify unused NuGet packages in your C# projects. It helps keep your dependencies clean and your build times fast.

## Installation

### As a .NET Global Tool
The easiest way to install and use `unused-packages` is as a .NET global tool.

1. **Install globally**:
   ```bash
   dotnet tool install -g unused-packages
   ```

2. **Run from anywhere**:
   ```bash
   unused-packages
   ```

### From Source
If you want to build and install it yourself:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/mnsdojo/unused-packages.git
   cd unused-packages
   ```

2. **Pack and Install locally**:
   ```bash
   dotnet pack -o ./nupkg
   dotnet tool install -g --add-source ./nupkg unused-packages
   ```

## Usage

Simply navigate to your C# project or solution directory and run:

```bash
unused-packages
```

The tool will scan your project files, analyze package usage, and provide a detailed report of packages that appear to be unused.

### Options
- `--path <dir>`: Specify a directory to scan (default is current directory).
- `--help`: Show all available commands and options.

## License
MIT
