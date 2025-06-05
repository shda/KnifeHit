-- Для работы старого кода если используются старые методы
function setUserKnifeSkin(skin) level:SetUserKnifeSkin(skin) end
function setTargetSkin(skin) level:SetTargetSkin(skin) end
function setBonus(index , rotation) level:SetBonus(index , rotation) end
function setObstacle(index , rotation) level:SetObstacle(index , rotation) end
function rotateAsync(str) level:RotateAsync(index , rotation) end

rotations =
{
    "o/360/1,5",
	"o/0/1,5",
}

level:SetCountUserKnives(6)
-- установка скина ножа игрока
-- level:setUserKnifeSkin()
level:SetUserKnifeSkin(3)

-- установка скина цели
level:SetTargetSkin(1)


-- первая цифра тип , вторая градус позиции по часовой стрелке
level:SetBonus(0 , 270)
level:SetBonus(1 , 300)
level:SetBonus(0 , 65)
level:SetBonus(1 , 100)

-- воткнутый нож с цель , цифра тип , вторая позиция
level:SetObstacle(0 , 0)
--level:SetObstacle(1 , 30)
--level:SetObstacle(3 , 50)
--level:SetObstacle(4 , 80)
--level:SetObstacle(5 , 120)
--level:SetObstacle(6 , 180)
--level:SetObstacle(7 , 210)

while true do
  for i = 1, #rotations do
        level:RotateAsync(rotations[i])
    end
end