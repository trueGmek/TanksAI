Agent:move({ X = 0, Y = 0, Z = 0 })
Wait(1000)

for _ = 1, 10, 1 do
  Agent:shoot({ X = 0, Y = 0, Z = 1 });
end

return 0
