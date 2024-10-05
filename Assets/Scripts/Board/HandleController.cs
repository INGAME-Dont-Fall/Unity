using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace DontFall.Board
{
    [ExecuteInEditMode]
    public class HandleController : MonoBehaviour, IDragHandler
    {
        [SerializeField] private BoardController board;

        private void Update()
        {
            if (board != null)
            {
                transform.position = Vector2.Lerp(board.Edge1, board.Edge2, board.Pivot + 0.5f);
            }
        }

        void IDragHandler.OnDrag(PointerEventData pointer)
        {
            if (board != null)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(pointer.position);

                var pivotValue = Vector2.Dot(pos - board.Edge1, (board.Edge2 - board.Edge1).normalized) / (board.Edge2 - board.Edge1).magnitude;

                board.Pivot = pivotValue - 0.5f;
            }
        }
    }
}
