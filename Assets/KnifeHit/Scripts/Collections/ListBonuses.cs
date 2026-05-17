using KnifeHit.Scripts.Bonuses;
using UnityEngine;

namespace KnifeHit.Scripts.Collections
{
    [CreateAssetMenu(menuName = "Create ListBonuses", fileName = "ListBonuses", order = 0)]
    public class ListBonuses : ListObjects<Bonus> { }
}