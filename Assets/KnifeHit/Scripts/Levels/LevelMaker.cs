using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Levels;
using TMPro;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class LevelMaker : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI stepText;
        [SerializeField] private GameObject editor;
        [SerializeField] private LevelPlayer levelPlayer;
        
        public void OnPressButtonOpenEditor()
        {
            if (editor.activeInHierarchy)
            {
                editor.SetActive(false);
                CloseEditor();
            }
            else
            {
                editor.SetActive(true);
                OpenEditor();
            }
        }


        private void OpenEditor()
        {
            var ed = PlayerPrefs.GetString("Editor");
            if (!string.IsNullOrEmpty(ed))
            {
                inputField.text = ed;
            }
        }

        private void CloseEditor()
        {
            PlayerPrefs.SetString("Editor" , inputField.text);
            
            try
            {
                var parse = LevelParser.ParseInput(inputField.text);
                levelPlayer.PlayLevel(inputField.text , moving =>
                {
                    stepText.text = moving.StringStep;
                });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
    }
}