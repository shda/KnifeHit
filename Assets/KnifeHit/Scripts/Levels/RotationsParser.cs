using System;
using System.Collections.Generic;
using System.Linq;

namespace KnifeHit.Scripts.Levels
{
    public static class RotationsParser
    {
        //o/160/4,3 такая строка
        public static RotationData ParseLine(string line)
        {
            string trimmedLine = line.Trim();
            
            // Пропускаем пустые строки и строки с #
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                return null;

            // Обрабатываем строки с /
            if (trimmedLine.Contains("/"))
            {
                string[] parts = trimmedLine.Split('/');
                    
                //offset - o
                //absolute - a
                string pos = "o";
                string angle = "";
                string duration = "";
                    
                if (parts.Length == 2)
                {
                    angle = parts[1];
                    duration = parts[1];
                }
                else if (parts.Length == 3)
                {
                    pos = parts[0];
                    angle = parts[1];
                    duration = parts[2];
                }
                    
                if (float.TryParse(angle.Trim(), out float firstNumber) && 
                    float.TryParse(duration.Trim().Replace(',', '.'), 
                        System.Globalization.NumberStyles.Any, 
                        System.Globalization.CultureInfo.InvariantCulture, out float secondNumber))
                {
                    return new RotationData()
                    {
                        Angle = firstNumber,
                        Duration = secondNumber,
                        IsOffset = pos.ToLower() == "o",
                        StringStep = trimmedLine,
                    };
                }
            }

            return null;
        }
        
        public static List<RotationData> ParseInput(string input)
        {
            var lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.Select(ParseLine).Where(result => result != null).ToList();
        }
    }
}