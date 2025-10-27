local M = {}

---Wrapper method that checks for nil before calling the argument
---@param fun function
---@return unknown
M.safe_call = function(fun)
  Log(tostring(fun))
  if fun then
    return fun()
  end

  return nil
end

return M;
