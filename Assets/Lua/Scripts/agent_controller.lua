local Vector = require("Assets/Lua/Modules/vector3")
local M = {};

M.Start = function()
end

M.Update = function()
  local position = Opponent:GetPosition()
  Agent:shoot(position)
  Wait(1000)
end

local v1 = Vector.new(4, 2, 0)
print(tostring(v1))


return M
