using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Size size;
    public int index = 0;

    public DontFall.PlaySound playSound;
    public AudioClip dropSound;
    
    private GameObject inventory;
    private CanvasGroup canvasGroup;
    private GameObject currentDraggedObject;
    private Canvas canvas;  // 오브젝트를 드랍할 UI 캔버스
    private Vector3 defaultPosition;

    private void Awake()
    {
        inventory = GameManager.Instance.InventoryArea;
        defaultPosition = gameObject.transform.localPosition;
        canvas = GameManager.Instance.Canvas;
        canvasGroup = canvas.GetComponent<CanvasGroup>();
    }

    //드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            canvasGroup.blocksRaycasts = false;

            //이미지는 끄고 정해진 오브젝트를 생성
            gameObject.GetComponent<Image>().enabled = false;
            currentDraggedObject = Instantiate(GameManager.Instance.Objects[(int)size].objectList[index].go, GameManager.Instance.objectGroup.transform);

            if(currentDraggedObject.GetComponent<Collider2D>() != null)
            {
                currentDraggedObject.GetComponent<Collider2D>().isTrigger = true;
            }
            currentDraggedObject.GetComponent<DragObj>().index = index;
            currentDraggedObject.GetComponent<DragObj>().isClicked = true;
        }
    }


    //드래그 중 계속 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentDraggedObject == null) return;

            // 드래그한 오브젝트가 마우스를 따라 움직이게 설정
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            currentDraggedObject.transform.position = mousePos;
        }
    }

    //드래그 끝날 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentDraggedObject != null)
            {
                currentDraggedObject.GetComponent<DragObj>().InputDisable();
                if (currentDraggedObject.GetComponent<Collider2D>() != null)
                {
                    currentDraggedObject.GetComponent<Collider2D>().isTrigger = false;
                }

                //다시 집어 넣기
                if (RectTransformUtility.RectangleContainsScreenPoint(inventory.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), canvas.worldCamera))
                {
                    gameObject.transform.localPosition = defaultPosition;
                    gameObject.GetComponent<Image>().enabled = true;
                    Destroy(currentDraggedObject);

                    playSound.Play(dropSound);

                    canvasGroup.blocksRaycasts = true;
                }
                else
                {
                    currentDraggedObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Object";
                    currentDraggedObject.GetComponent<DragObj>().isClicked = false;
                    currentDraggedObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                    currentDraggedObject.transform.SendMessage("ItemDrop", SendMessageOptions.DontRequireReceiver);

                    canvasGroup.blocksRaycasts = true;

                    GameManager.Instance.IncreaseItemsCount();

                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
}
