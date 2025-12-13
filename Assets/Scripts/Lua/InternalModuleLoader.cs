using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using Logger = Utils.Logger;

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

    private string ResolveModulePath(string moduleName)
    {
      string path = moduleName.Contains('.') ? moduleName.Replace('.', Path.DirectorySeparatorChar) : moduleName;

      if (!Path.HasExtension(path))
      {
        path += ".lua";
      }

      Logger.Log($"Path: {path}", Tags.LUA);

      if (!path.Contains(luaScriptLocations))
      {
        path = Path.Combine("Assets", "Lua", path);
      }

      Logger.Log($"Result: {path}", Tags.LUA);

      return path;
    }
  }
}
