using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteractionDetector : InputBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private PlayerProgressBar _progress;

        private IInteractable _currentTarget;
        
        protected override void SubscribeControls()
        {
            Input.actions["Interact"].performed += Interact;
            Input.actions["Interact"].started += Interact;
            Input.actions["Interact"].canceled += Interact;
        }
        protected override void UnsubscribeControls()
        {
            Input.actions["Interact"].performed += Interact;
            Input.actions["Interact"].started -= Interact;
            Input.actions["Interact"].canceled -= Interact;
        }
    
        private void Update()
        {
            CheckForInteractable();
        }
    
        private void CheckForInteractable()
        {
            var results = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(transform.position, radius, results, interactableMask);
                     
            if (size == 0)
            {
                ResetTarget();
                return;
            }

            IInteractable interactable = null;
            
            foreach (Collider col in results)
            {
                if (interactable != null) break;
                col.TryGetComponent(out interactable);
            }
            
            if (interactable != null)
            {
                if (_currentTarget == interactable) return;
                
                if (_currentTarget != null) _currentTarget.OnExitRange();
                
                _currentTarget = interactable;
                
                if (_currentTarget != null) _currentTarget.OnEnterRange();
            }
            else ResetTarget();
        }
    
        private void ResetTarget()
        {
            if (_currentTarget == null) return;

            _currentTarget.OnExitRange();
            _currentTarget = null;
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    
        private void Interact(InputAction.CallbackContext ctx)
        {
            if (_currentTarget == null) return;
            
            _currentTarget.OnInteract();
        }
    
    }
}
