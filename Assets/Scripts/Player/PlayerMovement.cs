using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : InputBehaviour
    {
        [SerializeField] private float force;
        [SerializeField] private float maxVel;

        private Vector3 _direction;
        private Rigidbody _rb;
        
        protected override void ProtectedStart()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected override void SubscribeControls()
        {
            Input.actions[Actions.Movement.ToString()].performed += SetDirection;
            Input.actions[Actions.Movement.ToString()].canceled += ResetMovement;
        }

        protected override void UnsubscribeControls()
        {
            Input.actions[Actions.Movement.ToString()].performed -= SetDirection;
            Input.actions[Actions.Movement.ToString()].canceled -= ResetMovement;
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
}
