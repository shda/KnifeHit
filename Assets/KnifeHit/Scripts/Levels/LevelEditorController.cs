using System;
using UniRx;
using UnityEngine;

namespace KnifeHit.Scripts.Levels
{
    public class LevelEditorController : MonoBehaviour
    {
        [SerializeField] private LuaCodeEditor inputField;
        [SerializeField] private GameObject editor;
        [SerializeField] private Game game;

        public static string NameSave = "Editor6";
        public void OnPressButtonOpenEditor()
        {
            if (editor.activeInHierarchy)
            {
                editor.SetActive(false);
                Restart();
            }
            else
            {
                editor.SetActive(true);
                OpenEditor();
            }
        }
        private void OpenEditor()
        {
            var ed = PlayerPrefs.GetString(NameSave);
            if (!string.IsNullOrEmpty(ed))
            {
                inputField.text = ed;
            }
        }
        
        public void Restart()
        {
            PlayerPrefs.SetString(NameSave, inputField.text);
            game.Restart();
        }

        public void Convert()
        {
            inputField.text = ReplacerOldLuaCode.Convert(inputField.text);
        }
    }
}