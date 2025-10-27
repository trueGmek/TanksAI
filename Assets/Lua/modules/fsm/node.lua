---@class Node Node class for FSM
---@field name string
---@field transitions table
---@field tick function
---@field exit function
---@field start function
local M = {}
M.__index = M


---@param name string
---@return Node
M.new = function(name)
  assert(name:len() > 0, "Name cannot be empyt")

  local node = {
    name = name,
  }

  local self = setmetatable(node, M)
  return self
end

return M;
