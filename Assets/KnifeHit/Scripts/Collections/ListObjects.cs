using UnityEngine;

namespace KnifeHit.Scripts.Collections
{
    public class ListObjects<T> : ScriptableObject where T : Object
    {
        [SerializeField] private T[] list;
        public T GetWithOverflow(int index)
        {
            if (index < 0)
                index = 0;
            
            return list[index % list.Length];
        }
    }
}