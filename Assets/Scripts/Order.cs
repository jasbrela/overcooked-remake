using UnityEngine;

[CreateAssetMenu(menuName = "Order", fileName = "New Order")]
public class Order : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public Food finalItem;
    [SerializeField] public int price;
}
