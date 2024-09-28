using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DontFall
{
    public class HandleController : MonoBehaviour
    {
        [SerializeField] private BoardController board;

        private bool selected;

        private void Start()
        {
            transform.position = Vector2.Lerp(board.Edge1, board.Edge2, board.Pivot + 0.5f);
        }

        private void Update()
        {
            MouseInput();
        }

        private void MouseInput()
        {
            var mouse = Mouse.current;
            if(!selected)
            {
                if(mouse.press.wasPressedThisFrame)
                {
                    var pos = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
                    var pos2d = new Vector2(pos.x, pos.y);

                    // TODO: make universal click-detection system so that one can't select multiple objects.
                    var hits = Physics2D.RaycastAll(pos2d, Vector2.zero);
                    foreach(var hit in hits)
                    {
                        if(hit.collider.gameObject == gameObject)
                        {
                            selected = true;
                        }
                    }
                }
            }
            else
            {
                if(mouse.press.wasReleasedThisFrame)
                {
                    selected = false;
                }
                else
                {
                    var pos = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
                    var pos2d = new Vector2(pos.x, pos.y);

                    var pivotValue = Vector2.Dot(pos2d - board.Edge1, (board.Edge2 - board.Edge1).normalized) / (board.Edge2 - board.Edge1).magnitude;

                    transform.position = Vector2.Lerp(board.Edge1, board.Edge2, pivotValue);
                    board.Pivot = pivotValue - 0.5f;
                }
            }
        }
    }
}
