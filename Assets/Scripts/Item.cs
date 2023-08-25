using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private bool isPlate;
    
    public int Id => id;
}
