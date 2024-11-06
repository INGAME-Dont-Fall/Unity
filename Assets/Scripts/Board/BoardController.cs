using UnityEngine;

namespace DontFall.Board
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private float width;

        private Vector2 position;

        private bool moving;

        public Vector2 Position => position;

        public float Width => width;

        public bool Moving
        {
            get => moving;
            set
            {
                var rigid = GetComponent<Rigidbody2D>();

                if (moving = value)
                {
                    rigid.simulated = true;
                }
                else
                {
                    rigid.simulated = false;
                    ResetBoard();
                }
            }
        }

        private void ResetBoard()
        {
            var rigid = GetComponent<Rigidbody2D>();

            rigid.velocity = Vector2.zero;
            rigid.angularVelocity = 0f;

            transform.position = position;
            transform.rotation = Quaternion.identity;
        }

        private void Awake()
        {
            position = transform.position;
        }

        private void Start()
        {
            Moving = false;
        }
    }
}
