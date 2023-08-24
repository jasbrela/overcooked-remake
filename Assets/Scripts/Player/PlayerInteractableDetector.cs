using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteractionDetector : InputBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private Transform snapPoint;

        private IInteractable _currentTarget;
        private ISurface _currentSurface;
        
        private Item _currentItem;
        
        protected override void SubscribeControls()
        {
            Input.actions[Actions.Place.ToString()].performed += CheckSurfaces;
            Input.actions[Actions.Interact.ToString()].performed += CheckInteractions;
            Input.actions[Actions.Interact.ToString()].canceled += CancelInteraction;
        }
        protected override void UnsubscribeControls()
        {
            Input.actions[Actions.Place.ToString()].performed -= CheckSurfaces;
            Input.actions[Actions.Interact.ToString()].performed += CheckInteractions;
            Input.actions[Actions.Interact.ToString()].canceled -= CancelInteraction;
        }

        private T ScanForElement<T>()
        {
            var results = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(transform.position, radius, results, interactableMask);
            
            if (size == 0) return default;
            T elem = default;

            foreach (Collider col in results)
            {
                if (elem != null) break;
                col.TryGetComponent(out elem);
            }
            return elem;
        }
        
        private T[] ScanForList<T>()
        {
            var results = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(transform.position, radius, results, interactableMask);
            
            var list = new T[5];
            
            var i = 0;
            
            if (size == 0) return list;

            foreach (Collider col in results)
            {
                if (col == null) continue;
                
                col.TryGetComponent(out T elem);
                
                if (elem == null) continue;
                
                list[i] = elem;
                i++;
            }
            return list;
        }

        private void CheckForInteractable()
        {
            IInteractable interactable = ScanForElement<IInteractable>();
            
            if (interactable != null)
            {
                if (_currentTarget == interactable) return;
                
                _currentTarget = interactable;
            }
            else ResetTarget();
        }
        
        private void CheckForSurface()
        {
            ISurface[] surfaces = ScanForList<ISurface>();
            foreach (ISurface surface in surfaces)
            {
                if (surface == null) continue;

                bool canPick = _currentItem == null && surface.HasItem;
                bool canPlace = _currentItem != null && !surface.HasItem;
                
                if (canPick || canPlace)
                {
                    _currentSurface = surface;
                }
            }
            
            if (_currentSurface == null) ResetSurface();
        }
    
        private void ResetTarget()
        {
            if (_currentTarget == null) return;

            _currentTarget.CancelInteraction();
            _currentTarget = null;
        }
        
        private void ResetSurface()
        {
            _currentSurface = null;
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    
        private void CheckInteractions(InputAction.CallbackContext ctx)
        {
            CheckForInteractable();
            
            if (_currentTarget == null) return;
            
            _currentTarget.OnInteract();
        }
        
        private void CheckSurfaces(InputAction.CallbackContext ctx)
        {
            CheckForSurface();
            
            if (_currentSurface == null) return;

            if (_currentItem == null)
            {
                _currentItem = _currentSurface.Pick();
                                    
                Transform t = _currentItem.transform;
                    
                t.position = snapPoint.position;
                t.rotation = snapPoint.rotation;

                t.parent = snapPoint;
            }
            else
            {
                _currentItem.transform.parent = null;

                _currentSurface.Place(_currentItem);
                _currentItem = null;
            }
        }
    
        private void CancelInteraction(InputAction.CallbackContext ctx)
        {
            if (_currentTarget == null) return;
            
            _currentTarget.CancelInteraction();
        }
    }
}
