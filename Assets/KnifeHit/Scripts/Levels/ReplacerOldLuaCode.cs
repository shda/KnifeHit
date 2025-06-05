namespace KnifeHit.Scripts.Levels
{
    public class ReplacerOldLuaCode
    {
        private static (string, string)[] replace1 = new[] 
        {
            ("setUserKnifeSkin", "level:SetUserKnifeSkin"),
            ("setTargetSkin", "level:SetTargetSkin"),
            ("setBonus", "level:SetBonus"),
            ("setObstacle", "level:SetObstacle"),
            ("rotateAsync", "level:RotateAsync"),
        };
        
        public static string Convert(string code)
        {
            foreach (var tuple in replace1)
            {
                code = code.Replace(tuple.Item1, tuple.Item2);
            }

            return code;
        }
    }
}