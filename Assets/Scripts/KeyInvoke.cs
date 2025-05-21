using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DontFall
{
    public class KeyInvoke : MonoBehaviour
    {
        [SerializeField] private InputAction invokeAction;
        [SerializeField] private UnityEvent invokeEvent;

        private void OnEnable()
        {
            invokeAction.Enable();
        }

        private void OnDisable()
        {
            invokeAction.Disable();
        }

        private void Awake()
        {
            invokeAction.performed += ctx => invokeEvent.Invoke();
        }
    }
}
