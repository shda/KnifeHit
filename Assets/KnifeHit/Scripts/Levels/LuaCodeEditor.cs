using UnityEngine;

namespace KnifeHit.Scripts
{
    public class LuaCodeEditor : MonoBehaviour
    {
        [SerializeField] private Font font;
        private Vector2 scrollPosition;
        
        public string text;
        private void OnGUI()
        {
            int width = Screen.width - 10;
            int height = Screen.height - 100;
            
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, 
                GUILayout.Width(width), GUILayout.Height(height));
           
            GUIStyle style = new GUIStyle(GUI.skin.textField);
            style.fontSize = 20; 
            style.font = font;
            
            text = GUILayout.TextArea(
                text, 
                style,
                GUILayout.Width(width - 20), 
                GUILayout.ExpandHeight(true) 
            );
            
            // Заканчиваем область скролла
            GUILayout.EndScrollView();
        }
    }
}