local M = {};

M.Start = function()
  Agent:move({ X = -6, Y = 0, Z = 0 })
  Wait(2000)
end

M.Update = function()
  Agent:move({ X = 0, Y = 0, Z = -12 })
  Wait(2000)
end

return M
