using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class NewMonoBehaviourScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Size size;
    public int index = 0;
    private Canvas canvas;  // ������Ʈ�� ����� UI ĵ����
    private PlayerInput inputActions;
    private CanvasGroup canvasGroup;
    private Rigidbody2D rb2D;

    public bool isClicked;
    private void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.PlayerActions.Enable();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void InputDisable()
    {
        inputActions.PlayerActions.Disable();
    }

    private void Update()
    {
        if (isClicked)
        {
            float rotateDirection = -inputActions.PlayerActions.Rotate.ReadValue<float>();
            gameObject.transform.Rotate(Vector3.forward * rotateDirection * Time.deltaTime * 100.0f);
        }
    }

    private void Start()
    {
        //canvas = GameManager.Instance.Canvas;
        //canvasGroup = canvas.GetComponent<CanvasGroup>();
    }
    //�巡�� ���� �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            canvasGroup.blocksRaycasts = false;
        }
        rb2D.gravityScale = 0;
        rb2D.linearVelocity = Vector2.zero;
    }

    // �巡�� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // ���콺�� ��������� ������Ʈ ��ġ ����
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
    }

    // �巡�� ���� �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            for (int i = GameManager.Instance.emptyInventory.Count - 1; i >= 0; i--)
            {
                GameObject go = GameManager.Instance.emptyInventory[i];
                if (RectTransformUtility.RectangleContainsScreenPoint(go.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), canvas.worldCamera))
                {
                    GameObject newUIObject = null;

                    // UI �������� �����ϰ� �ش� UI�� ���ӽ�Ŵ
                    newUIObject = Instantiate(GameManager.Instance.Objects[(int)size].objectList[index].ui, go.transform);

                    if (newUIObject is not null)
                    {
                        newUIObject.GetComponent<DragUI>().index = index;
                    }

                    //ĭ�� á���ϱ� ����
                    GameManager.Instance.emptyInventory.Remove(go);
                    Destroy(gameObject);
                }
            }

            rb2D.gravityScale = 1;

            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }
}
