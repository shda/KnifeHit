using System;
using UnityEngine;

namespace KnifeHit.Scripts.Menu
{
    [Serializable]
    public class ItemInfo
    {
        [SerializeField] private Sprite knifeSprite;
        [SerializeField] private Sprite shadowKnifeSprite;
        
        public Sprite KnifeSprite => knifeSprite;
        public Sprite ShadowKnifeSprite => shadowKnifeSprite;
    }
}