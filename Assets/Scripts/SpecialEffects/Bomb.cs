using UnityEngine;

namespace DontFall.Objects
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float effectDelay;
        [SerializeField] private float effectRadius;
        [SerializeField] private float effectPower;

        private void Start()
        {
            GameManager.Instance.GameStart += OnGameStart;
            GameManager.Instance.GameEnd += OnGameEnd;
        }

        private void OnDestroy()
        {
            GameManager.Instance.GameStart -= OnGameStart;
            GameManager.Instance.GameEnd -= OnGameEnd;
        }

        private void OnGameStart()
        {
            Invoke(nameof(Explode), effectDelay);
        }

        private void OnGameEnd()
        {
            CancelInvoke(nameof(Explode));
        }

        private void Explode()
        {
            Debug.Log("Explode");

            foreach (Collider2D obj in Physics2D.OverlapCircleAll(transform.position, effectRadius))
            {
                if (obj.transform != transform && obj.CompareTag("Object"))
                {
                    obj.attachedRigidbody?.AddForce(effectPower * (obj.transform.position - transform.position), ForceMode2D.Impulse);
                }
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, effectRadius);
        }
    }
}
