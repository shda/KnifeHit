using System;
using System.Collections.Generic;
using Common.Scripts;
using TriInspector;
using UnityEngine;

namespace KnifeHit.Scripts.GameUi
{
    public class AttemptsCounter : MonoBehaviour
    {
        [SerializeField] private PoolGameObjectsComponent<KnifeIcon> poolIcons;
        
        private readonly List<KnifeIcon> _listKnifes = new();
        
        public void SetCountKnifes(int allCount , int countEnable)
        {
            Debug.Log($"SetCountKnifes {allCount} - {countEnable}");
            if (countEnable > allCount)
                countEnable = allCount;
            
            if (_listKnifes.Count != allCount || _listKnifes.Count < countEnable)
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
            var icons = transform.GetComponentsInChildren<KnifeIcon>();
            foreach (var icon in icons)
            {
                poolIcons.Release(icon);
            }
            
            _listKnifes.Clear();
        }
        
        private void ReturnLastIcons()
        {
            foreach (var knifesIcon in _listKnifes)
            {
                try
                {
                    poolIcons.Release(knifesIcon);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
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