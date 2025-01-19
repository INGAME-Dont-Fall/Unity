using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DontFall
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private Slider zoomSlider;

        [SerializeField] private float zoomMultiplier;

        private void Update()
        {
            var camera = GetComponent<Camera>();

            if (Mouse.current.rightButton.isPressed)
            {
                var delta = Mouse.current.delta.ReadValue();
                var pos = -delta + camera.pixelRect.size * 0.5f;
                var dest = camera.ScreenToWorldPoint(pos);
                transform.position = dest;
            }
        }
    }
}
