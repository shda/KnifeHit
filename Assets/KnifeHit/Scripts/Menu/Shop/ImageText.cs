using System.Globalization;
using TMPro;
using UnityEngine;

namespace KnifeHit.Scripts.Menu.Shop
{
    public class ImageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI priceText;

        public void SetValue(float value)
        {
            priceText.text = value.ToString(CultureInfo.InvariantCulture);
            gameObject.SetActive(true);
        }
    }
}