rotations =
{
	"a/-180/1,5",
	"a/0/1,5",
}


setUserKnifeSkin("1")
setTargetSkin("1")

-- первая цифра тип бонуса, вторая позиция 
setBonus("1" , 240)

-- воткнутый нож с цель , цифра тип бонуса, вторая позиция 
setObstacle("1" , 180)


function Work()
	for i = 1, #rotations do
		rotateAsync(rotations[i])
	end
end

return Work