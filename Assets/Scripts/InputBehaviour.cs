using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public abstract class InputBehaviour : MonoBehaviour
{
    protected PlayerInput Input;

    private void Start()
    {
        Input = GetComponent<PlayerInput>();

        SubscribeControls();
        ProtectedStart();
    }

    private void OnDisable()
    {
        UnsubscribeControls();
        ProtectedOnDisable();
    }

    protected virtual void ProtectedStart() { }
    protected virtual void ProtectedOnDisable() { }
    
    protected virtual void SubscribeControls() { }
    protected virtual void UnsubscribeControls() { }
}
