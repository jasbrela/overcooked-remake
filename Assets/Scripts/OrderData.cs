using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Order", fileName = "New Order")]
public class OrderData : ScriptableObject
{
    [SerializeField] public Food finalItem;
    [SerializeField] public int price;
}
