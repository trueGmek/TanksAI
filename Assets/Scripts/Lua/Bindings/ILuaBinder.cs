namespace Lua.Bindings
{
  public interface ILuaBinder
  {
    public void Bind(in LuaState state);
  }
}