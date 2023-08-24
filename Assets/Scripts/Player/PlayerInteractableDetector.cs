using System.Linq;
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

        private IInteractable _currentInteractable;
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
            var center = transform.position;
            
            var results = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(center, radius, results, interactableMask);
            
            if (size == 0) return default;
            T elem = default;
            
            var ordered = results.OrderBy(c =>
            {
                if (c == null) return 999;
                return (center - c.transform.position).sqrMagnitude;
            }).ToArray();
            
            foreach (Collider col in ordered)
            {
                if (elem != null) break;
                col.TryGetComponent(out elem);
            }
            return elem;
        }
        
        private T[] ScanForList<T>()
        {
            var center = transform.position;

            var results = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(center, radius, results, interactableMask);
            
            var ordered = results.OrderBy(c =>
            {
                if (c == null) return 999;
                return (center - c.transform.position).sqrMagnitude;
            }).ToArray();

            var list = new T[5];
            
            var i = 0;
            
            if (size == 0) return list;

            foreach (Collider col in ordered)
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
                if (_currentInteractable == interactable) return;
                
                _currentInteractable = interactable;
            }
            else ResetInteractable();
        }
        
        private void CheckForSurface()
        {
            ISurface[] surfaces = ScanForList<ISurface>();
            
            foreach (ISurface surface in surfaces)
            {
                if (surface == null) continue;

                bool canPick = _currentItem == null && surface.HasItem;
                bool canPlace = _currentItem != null && !surface.HasItem;
                bool canCombine = _currentItem != null && surface.HasItem;
                
                if (!canPick && !canPlace && !canCombine) continue;
                
                _currentSurface = surface;
                return;
            }
            
            ResetSurface();
        }
    
        private void ResetInteractable()
        {
            if (_currentInteractable == null) return;

            _currentInteractable.CancelInteraction();
            _currentInteractable = null;
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
            
            if (_currentInteractable == null) return;
            
            _currentInteractable.OnInteract();
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
                if (_currentSurface.Combine(_currentItem)) return;
                if (_currentSurface.Place(_currentItem)) _currentItem = null;
            }
        }
    
        private void CancelInteraction(InputAction.CallbackContext ctx)
        {
            if (_currentInteractable == null) return;
            
            _currentInteractable.CancelInteraction();
        }
    }
}
