local FSM = require("Assets/Lua/modules/fsm/fsm")
local Nodes = require("Assets/Lua/modules/fsm/node")
local M = {};

local patrol = Nodes.new("partol")

patrol.start = function()
  Log("PATROL: on_start")
  Agent:move({ X = -6, Y = 0, Z = 0 })
end

patrol.exit = function()
  Log("PATROL: on_exit")
end

patrol.tick = function()
  Log("PATROL: upadte")
end

local fsm = FSM.new()
fsm:add_state(patrol)

M.Start = function()
  fsm:change_state(patrol)
  Log 'Start'
end

M.Update = function()
  Log 'Update'
  fsm:tick()
end

return M;
