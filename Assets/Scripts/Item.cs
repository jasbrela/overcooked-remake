using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int id;
    
    public int Id => id;

    public void Snap(Transform parent)
    {
        var t = transform;
        
        t.position = parent.position;
        t.rotation = parent.rotation;
        t.parent = parent;
    }
}
