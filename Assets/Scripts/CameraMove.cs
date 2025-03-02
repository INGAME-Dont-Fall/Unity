using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DontFall
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private Slider zoomSlider;

        [SerializeField] private float zoomMultiplier;

        [SerializeField] private Vector2 limitTopLeft;
        [SerializeField] private Vector2 limitBottomRight;

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

            Vector2 topLeft = camera.ViewportToWorldPoint(new(0, 1, 0));
            Vector2 bottomRight = camera.ViewportToWorldPoint(new(1, 0, 0));

            if (topLeft.x < limitTopLeft.x)
                transform.position += new Vector3(limitTopLeft.x - topLeft.x, 0, 0);
            if (topLeft.y > limitTopLeft.y)
                transform.position += new Vector3(0, limitTopLeft.y - topLeft.y, 0);
            if (bottomRight.x > limitBottomRight.x)
                transform.position += new Vector3(limitBottomRight.x - bottomRight.x, 0, 0);
            if (bottomRight.y < limitBottomRight.y)
                transform.position += new Vector3(0, limitBottomRight.y - bottomRight.y, 0);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector2.Lerp(limitTopLeft, limitBottomRight, 0.5f), limitTopLeft - limitBottomRight);
        }
    }
}
