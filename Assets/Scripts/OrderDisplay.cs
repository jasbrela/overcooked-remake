using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI priceText;
    
    public void Show(Order data)
    {
        priceText.text = data.price.ToString();
        image.sprite = data.sprite;
    }
}
