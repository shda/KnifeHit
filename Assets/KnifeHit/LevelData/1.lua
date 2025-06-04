rotations =
{
    "a/360/3",
    "a/0/3",
}

-- установка скина ножа игрока
-- setUserKnifeSkin()
setUserKnifeSkin(0)

-- установка скина цели
setTargetSkin(0)

-- первая цифра тип бонуса, вторая градус позиции по часовой стрелке
setBonus(0 , 270)
setBonus(1 , 180)
setBonus(0 , 90)
setBonus(1 , 0)

-- воткнутый нож с цель , цифра тип бонуса, вторая позиция
setObstacle(0 , 100)
setObstacle(0 , 150)

function Work()
    for i = 1, #rotations do
        rotateAsync(rotations[i])
    end
end

return Work