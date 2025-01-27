using UnityEngine;

namespace DontFall.Objects
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] private Vector2 timeRange;
        [SerializeField] private float shakeSize;

        private bool isShaking;

        private void FixedUpdate()
        {
            if (isShaking)
            {
                var rigidbody = GetComponent<Rigidbody2D>();

                var shakeDirection = Random.insideUnitCircle;

                rigidbody.MovePosition((Vector2)transform.position + shakeSize * shakeDirection);
            }
        }

        private void ItemDrop()
        {
            float time = Random.Range(timeRange.x, timeRange.y);
            Invoke(nameof(StartShaking), time);
        }

        private void StartShaking()
        {
            isShaking = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position - shakeSize * transform.right, transform.position + shakeSize * transform.right);
        }
    }
}
