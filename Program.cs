
using Spectre.Console;

class Program
{
  static async Task Main(string[] args)
  {
    if (args.Length == 0)
    {
      PrintHelp();
      return;
    }
    var cmd = args[0].ToLowerInvariant();
    switch (cmd)
    {
      case "scan":
        await RunScan(args);
        break;
      default:
        PrintHelp();
        break;
    }


  }

  static async Task RunScan(string[] args)
  {
    var path = args.Length > 1 ? args[1] : Directory.GetCurrentDirectory();
    await WithSpinnerAsync("Scanning for unused packages...", async ctx =>
    {
      var csProj = FindCsProj(path);
      ctx.Status("Reading project file...");
      var packageReferences = await GetPackageReferencesAsync(csProj);
      if (packageReferences.Count == 0)
      {
        AnsiConsole.MarkupLine("[yellow]No package references found in the project.[/]");
        return;
      }


      ctx.Status("Analyzing source files...");


    });

  }

  record PackageReference(string Name, string Version);
  static async Task<List<PackageReference>> GetPackageReferencesAsync(string csProjPath)
  {
    var packageReferences = new List<PackageReference>();
    var xml = await File.ReadAllTextAsync(csProjPath);
    var doc = System.Xml.Linq.XDocument.Parse(xml);
    var ns = doc!.Root!.Name.Namespace;
    foreach (var pkg in doc.Descendants(ns + "PackageReference"))
    {
      var name = pkg.Attribute("Include")?.Value;

      var version =
          pkg.Attribute("Version")?.Value
          ?? pkg.Element(ns + "Version")?.Value
          ?? "Unknown";

      if (!string.IsNullOrWhiteSpace(name))
      {
        packageReferences.Add(
            new PackageReference(name, version)
        );
      }
    }
    return packageReferences;
  }

  static async Task WithSpinnerAsync(string title, Func<StatusContext, Task> action)
  {
    await AnsiConsole.Status()
    .Spinner(Spinner.Known.Dots)
    .SpinnerStyle(Style.Parse("green"))
    .StartAsync(title, async ctxt =>
    {
      try
      {
        await action(ctxt);

      }
      catch (Exception ex)
      {
        AnsiConsole.MarkupLine(
    $"[red]Error:[/] {ex.Message}"
);

      }
    });
  }

  private static string FindCsProj(string path)
  {
    var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
    if (files.Length == 0)
    {
      throw new FileNotFoundException("No .csproj file found in the specified path.");
    }
    return files[0];
  }


  private static void PrintHelp()
  {
    AnsiConsole.MarkupLine("[bold cyan]unused-packages[/]");
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine("Usage:");
    AnsiConsole.MarkupLine("  [green]unused-packages scan <path>[/]");
  }
}
