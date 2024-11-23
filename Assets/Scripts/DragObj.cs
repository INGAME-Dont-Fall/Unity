using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class DragObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private PlayerInput inputActions;
    public int index = 0;
    private Canvas canvas;  // 오브젝트를 드랍할 UI 캔버스
    private CanvasGroup canvasGroup;

    public bool isClicked;
    private void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.PlayerActions.Enable();
    }

    public void InputDisable()
    {
        inputActions.PlayerActions.Disable();
    }

    private void Update()
    {
        if(isClicked)
        {
            float rotateDirection = -inputActions.PlayerActions.Rotate.ReadValue<float>();
            gameObject.transform.Rotate(Vector3.forward * rotateDirection * Time.deltaTime * 100.0f);
        }
    }

    private void Start()
    {
        canvas = GameManager.Instance.Canvas;
        canvasGroup = canvas.GetComponent<CanvasGroup>();
    }
    //드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    // 드래그 중 호출
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스를 따라오도록 오브젝트 위치 설정
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);

    }

    // 드래그 종료 시 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        for(int i = GameManager.Instance.emptyInventory.Count - 1; i >=0; i--)
        {
            GameObject go = GameManager.Instance.emptyInventory[i];
            if (RectTransformUtility.RectangleContainsScreenPoint(go.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), canvas.worldCamera))
            {
                GameObject newUIObject = null;
                // UI 프리팹을 생성하고 해당 UI에 종속시킴
                if (gameObject.tag == "Object")
                {
                    newUIObject = Instantiate(GameManager.Instance.GamePrefab[index].gameUI, go.transform);
                }
                else if(gameObject.tag == "Assist")
                {
                    newUIObject = Instantiate(GameManager.Instance.AssistPrefab[index].gameUI, go.transform);
                }

                if(newUIObject is not null)
                {
                    newUIObject.GetComponent<DragUI>().index = index;
                }

                //칸이 찼으니까 삭제
                GameManager.Instance.emptyInventory.Remove(go);
                Destroy(gameObject);
            }
        }

        canvasGroup.blocksRaycasts = true;
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
