// See https://aka.ms/new-console-template for more information


Console.WriteLine("Unused Packages Analyzer");

if (args.Length == 0)
{
  PrintHelp();
  return;
}

var command = args[0].ToLower();

switch (command)
{

  case "scan":
    RunScan(args);
    break;

  default:
    Console.WriteLine($"Unknown command: {command}");
    PrintHelp();
    break;
}

static void RunScan(string[] args)

{
  var path = args.Length > 1 ? args[1] : Directory.GetCurrentDirectory();
  Console.WriteLine($"Scanning path: {path}");

  try
  {
    var csProjPath = FindCSProj(path);
    Console.WriteLine($"Found project file: {csProjPath}");
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Error during scan: {ex.Message}");
  }
}

static string FindCSProj(string path)
{
  var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
  if (files.Length == 0)
  {
    throw new FileNotFoundException("No .csproj files found in the specified path.");
  }
  return files[0];
}

static void PrintHelp()
{
  Console.WriteLine("unused-packages");
  Console.WriteLine();
  Console.WriteLine("Usage:");
  Console.WriteLine("  unused-packages scan <path>");
  Console.WriteLine();
}
