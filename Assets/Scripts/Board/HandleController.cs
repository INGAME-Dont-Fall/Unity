using UnityEngine;
using UnityEngine.EventSystems;

namespace DontFall.Board
{
    public class HandleController : MonoBehaviour, IDragHandler
    {
        [SerializeField] private BoardController board;
        [SerializeField] private Vector2 offset;

        private void Start()
        {
            if (board != null)
            {
                transform.position = board.Position + offset;
            }
        }

        void IDragHandler.OnDrag(PointerEventData pointer)
        {
            if (pointer.button == PointerEventData.InputButton.Left && board != null && !board.Moving)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(pointer.position);

                float pivot = Mathf.Clamp(Vector2.Dot(pos - (board.Position + offset), Vector2.right) / board.Width, -0.4f, 0.4f);

                transform.position = pivot * board.Width * Vector2.right + board.Position + offset;
            }
        }
    }
}
