using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float maxVel;

    private Vector3 _direction;
    private PlayerInput _input;
    private Rigidbody _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
        
        SetUpControls();
    }

    private void SetUpControls()
    {
        _input.actions[Actions.Movement.ToString()].performed += SetDirection;
        _input.actions[Actions.Movement.ToString()].canceled += ResetMovement;
    }

    private void SetDirection(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<Vector3>();
    }
    private void ResetMovement(InputAction.CallbackContext ctx)
    {
        _direction = Vector3.zero;
        Vector3 vel = _rb.velocity;

        vel = new Vector2(vel.x * 0.15f, vel.y * 0.15f);

        _rb.velocity = vel;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_rb == null) return;
        if (_rb.velocity.magnitude >= maxVel) return;
        
        _rb.AddRelativeForce(_direction * force, ForceMode.Force);
    }
}
