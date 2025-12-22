using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Lua
{
  public class InternalModuleLoader : ILuaModuleLoader
  {
    private static readonly string luaScriptLocations = Path.Combine("Assets", "Lua");

    public bool Exists(string moduleName)
    {
      string path = ResolveModulePath(moduleName);
      return File.Exists(path);
    }

    public async ValueTask<LuaModule> LoadAsync(string moduleName, CancellationToken cancellationToken = default)
    {
      string path = ResolveModulePath(moduleName);
      return new LuaModule(moduleName, await File.ReadAllTextAsync(path, cancellationToken));
    }

    private string ResolveModulePath(string modulePath)
    {
      string validatedPath = modulePath.Trim();

      string path = validatedPath.Contains('.')
        ? validatedPath.Replace('.', Path.DirectorySeparatorChar)
        : validatedPath;

      if (!Path.HasExtension(path))
      {
        path += ".lua";
      }

      if (!path.Contains(luaScriptLocations))
      {
        path = Path.Combine(luaScriptLocations, path);
      }

      return path;
    }
  }
}
