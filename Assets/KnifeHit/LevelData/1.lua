rotations =
{
    "o/360/1,5",
	"o/0/1,5",
}

-- установка скина ножа игрока
-- setUserKnifeSkin()
setUserKnifeSkin(3)

-- установка скина цели
setTargetSkin(1)

-- первая цифра тип , вторая градус позиции по часовой стрелке
--setBonus(0 , 270)
--setBonus(1 , 300)
--setBonus(0 , 65)
--setBonus(1 , 0)

-- воткнутый нож с цель , цифра тип , вторая позиция
setObstacle(0 , 0)
--setObstacle(1 , 30)
--setObstacle(3 , 50)
--setObstacle(4 , 80)
--setObstacle(5 , 120)
--setObstacle(6 , 180)
--setObstacle(7 , 210)

skin = 0
function Work()
    for i = 1, #rotations do
        rotateAsync(rotations[i])
		
		--skin = skin + 1
		--setTargetSkin(skin)
    end
end

return Work