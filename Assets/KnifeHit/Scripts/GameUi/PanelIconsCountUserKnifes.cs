using System.Collections.Generic;
using BlockBlast.Scripts.Common;
using TriInspector;
using UnityEngine;

namespace KnifeHit.Scripts.GameUi
{
    public class PanelIconsCountUserKnifes : MonoBehaviour
    {
        [SerializeField] private PoolGameObjectsComponent<KnifIcon> poolIcons;
        
        private readonly List<KnifIcon> _listKnifes = new();
        
        public void SetCountKnifes(int allCount , int countEnable)
        {
            Debug.Log($"SetCountKnifes {allCount} - {countEnable}");
            
            if (_listKnifes.Count != allCount)
            {
                ReturnLastIcons();
                CreateNewIcons(allCount, countEnable);
            }
            
            UpdateEnableKnifes(countEnable);
        }

        private void CreateNewIcons(int allCount, int countEnable)
        {
            for (int i = 0; i < allCount; i++)
            {
                var newIcon = poolIcons.Get();
                newIcon.transform.SetParent(transform);
                newIcon.transform.localScale = Vector3.one;
                newIcon.transform.localPosition = Vector3.zero;
                newIcon.transform.SetAsFirstSibling();
                newIcon.SetShowKnife(countEnable > i);
                _listKnifes.Add(newIcon);
            }
        }

        private void UpdateEnableKnifes(int countEnable)
        {
            for (int i = 0; i < _listKnifes.Count; i++)
            {
                _listKnifes[i].SetShowKnife(countEnable > i);
                _listKnifes[i].gameObject.SetActive(true);
            }
        }

        private void Awake()
        {
            Debug.Log($"Awake");
            
            var icons = transform.GetComponentsInChildren<KnifIcon>();
            foreach (var icon in icons)
            {
                poolIcons.Release(icon);
            }
        }
        
        private void ReturnLastIcons()
        {
            foreach (var knifesIcon in _listKnifes)
            {
                poolIcons.Release(knifesIcon);
            }
            _listKnifes.Clear();
        }
        
#if UNITY_EDITOR
        [Button]
        private void DebugSetCount(int allCount = 10 , int countEnable = 5)
        {
            SetCountKnifes(allCount , countEnable);
        }
#endif
    }
}