

class Analyzer
{

  public static bool IsPackageUsedAsync(HashSet<string> usedNameSpaces, string packageName)
  {
    foreach (var ns in usedNameSpaces)
    {
      if (ns.StartsWith(packageName, StringComparison.Ordinal))
      {
        return true;
      }

    }
    return false;
  }
  public static async Task<HashSet<string>> GetUsedNameSpacesAsync(string projectRoot)
  {
    var usedNamespaces = new HashSet<string>(StringComparer.Ordinal);
    // all Files (.cs)
    var csFiles = Directory.GetFiles(projectRoot, "*.cs", SearchOption.AllDirectories);


    foreach (var file in csFiles)
    {
      var lines = await File.ReadAllLinesAsync(file);
      foreach (var line in lines)
      {

        var trimmed = line.Trim();
        // Skipping comments and empty stuff
        if (string.IsNullOrEmpty(trimmed)) continue;
        if (trimmed.StartsWith("//")) continue;


        if (trimmed.StartsWith("using ") && trimmed.EndsWith(";") && !trimmed.Contains(" static "))
        {
          var ns = trimmed.Replace("using ", "").Replace(";", "").Trim();
          if (!string.IsNullOrWhiteSpace(ns))
          {
            usedNamespaces.Add(ns);
          }
        }

      }
    }

    return usedNamespaces;
  }
}
