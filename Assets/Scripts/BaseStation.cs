using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStation : MonoBehaviour, IInteractable
{
    [SerializeField] private StationType type;

    public void OnInteract()
    {
        Debug.Log("Interact");
    }

    public void OnEnterRange()
    {
        Debug.Log("OnEnterRange");

    }

    public void OnExitRange()
    {
        Debug.Log("OnExitRange");

    }
}
