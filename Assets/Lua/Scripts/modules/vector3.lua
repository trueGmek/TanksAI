---@class Vector3
---@field X number
---@field Y number
---@field Z number
Vector3 = {}
Vector3.__index = Vector3

function Vector3:__tostring()
  return string.format("{x=%s, y=%s, z=%s}", tostring(self.X), tostring(self.Y), tostring(self.Z))
end

---@param a Vector3
---@param b Vector3
---@return Vector3
function Vector3.__add(a, b)
  return Vector3.new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
end

---@param a Vector3
---@param b Vector3
---@return Vector3
function Vector3.__sub(a, b)
  return Vector3.new(a.X - b.X, a.Y - b.Y, a.Z - b.Z)
end

---@param vector Vector3
---@param number number
---@return Vector3
function Vector3.__mul(vector, number)
  return Vector3.new(vector.X * number, vector.Y * number, vector.Z * number)
end

---@param vector Vector3
---@param number number
---@return Vector3
function Vector3.__mul(number, vector)
  return Vector3.new(vector.X * number, vector.Y * number, vector.Z * number)
end

---@param vector Vector3
---@param number number
---@return Vector3
function Vector3.__div(vector, number)
  return Vector3.new(vector.X / number, vector.Y / number, vector.Z / number)
end

function Vector3.new(x, y, z)
  local self = setmetatable({
    X = x or 0,
    Y = y or 0,
    Z = z or 0
  }, Vector3)
  return self
end

return Vector3
