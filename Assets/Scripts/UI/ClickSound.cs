using UnityEngine;
using UnityEngine.InputSystem;

namespace DontFall.UI
{
    public class ClickSound : MonoBehaviour
    {
        [SerializeField] private PlaySound playSound;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private InputActionReference clickAction;

        private void Awake()
        {
            clickAction.action.performed += OnClick;
        }

        private void OnDestroy()
        {
            clickAction.action.performed -= OnClick;
        }

        private void OnClick(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValueAsButton())
                playSound.Play(audioClip);
        }
    }
}
