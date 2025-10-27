local utils = require("Assets/Lua/modules/utils")

---@class FSM Finite State Machine
---@field current Node
---@field states Node[]
local M = {}
M.__index = M

---Update method, should be called each frame
function M:tick()
  if self.current and self.current.tick then
    self.current.tick()
  end
end

---Adds a state to FSM
---@param node Node
function M:add_state(node)
  assert(node, "Cannot add a nil node")
  self.states[node.name] = node
end

---Changes current state
---@param arg string | Node of the state that should become current
function M:change_state(arg)
  local name = ""
  if type(arg) == "table" then
    ---@cast arg Node
    name = arg.name;
  elseif type(arg) == "string" then
    ---@cast arg string
    name = arg;
  end

  local new_node = self.states[name]
  assert(new_node, "State " .. name .. "was not added to the FSM")

  utils.safe_call(self.current.exit)
  self.current = new_node
  utils.safe_call(self.current.start)
end

---Create a new Finite State Machine
---@return FSM
M.new = function()
  local fsm = { states = {}, current = {} }
  local self = setmetatable(fsm, M)
  return self
end

return M;
