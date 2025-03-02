using UnityEngine;

namespace DontFall.Objects
{
    public class Clock : Special
    {
        [SerializeField] private Vector2 timeRange;
        [SerializeField] private float shakeInterval;
        [SerializeField] private float shakeSize;

        [SerializeField] private bool debugStart;

        private bool isFirstShake;

        private void Awake()
        {
            isFirstShake = true;
        }

        private void Start()
        {
            GameManager.Instance.GameEnd += StopShaking;

            if (debugStart)
            {
                ItemDrop();
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.GameEnd -= StopShaking;
        }

        private void ItemDrop()
        {
            float time = Random.Range(timeRange.x, timeRange.y);
            InvokeRepeating(nameof(Shake), time, shakeInterval);
        }

        private void Shake()
        {
            var rigidbody = GetComponent<Rigidbody2D>();

            var shakeDirection = Random.insideUnitCircle;

            rigidbody.MovePosition((Vector2)transform.position + shakeSize * shakeDirection);

            if (isFirstShake)
            {
                GetComponent<AudioSource>().Play();

                isFirstShake = false;
            }
        }

        private void StopShaking()
        {
            CancelInvoke();
            GetComponent<AudioSource>().Stop();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position - shakeSize * transform.right, transform.position + shakeSize * transform.right);
        }
    }
}
