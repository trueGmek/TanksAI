local vector = require("modules/vector3")
local M = {};

M.Start = function()
end

M.Update = function()
  local position = Opponent:GetPosition()
  Agent:shoot(position)
  Wait(1000)
end

return M
