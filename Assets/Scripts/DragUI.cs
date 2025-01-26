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

    private CanvasGroup canvasGroup;
    private GameObject currentDraggedObject;
    private Canvas canvas;  // 오브젝트를 드랍할 UI 캔버스

    private void Awake()
    {
        canvas = GameManager.Instance.Canvas;
        canvasGroup = canvas.GetComponent<CanvasGroup>();
    }

    //드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //부모 오브젝트는 인벤토리의 빈칸이 됨
            GameObject go = transform.parent.gameObject;
            GameManager.Instance.emptyInventory.Add(go);
            canvasGroup.blocksRaycasts = false;
            //이미지는 끄고 정해진 오브젝트를 생성
            gameObject.GetComponent<Image>().enabled = false;
            currentDraggedObject = Instantiate(GameManager.Instance.Objects[(int)size].objectList[index].go, GameManager.Instance.objectGroup.transform);

            currentDraggedObject.GetComponent<Collider2D>().isTrigger = true;
            currentDraggedObject.GetComponent<DragObj>().index = index;
            currentDraggedObject.GetComponent<DragObj>().isClicked = true;
        }
    }


    //드래그 중 계속 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 드래그한 오브젝트가 마우스를 따라 움직이게 설정
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            currentDraggedObject.transform.position = mousePos;
        }
    }

    //드래그 끝날 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        bool returning = false;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            currentDraggedObject.GetComponent<DragObj>().InputDisable();
            currentDraggedObject.GetComponent<Collider2D>().isTrigger = false;
            for (int i = GameManager.Instance.emptyInventory.Count - 1; i >= 0; i--)
            {
                GameObject go = GameManager.Instance.emptyInventory[i];
                if (RectTransformUtility.RectangleContainsScreenPoint(go.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), canvas.worldCamera))
                {
                    GameObject newUIObject = null;

                    // UI 프리팹을 생성하고 해당 UI에 종속시킴
                    newUIObject = Instantiate(GameManager.Instance.Objects[(int)size].objectList[index].ui, go.transform);

                    if (newUIObject is not null)
                    {
                        var ui = newUIObject.GetComponent<DragUI>();
                        ui.index = index;
                        ui.playSound = playSound;
                        ui.dropSound = dropSound;
                    }

                    //칸이 찼으니까 삭제
                    GameManager.Instance.emptyInventory.Remove(go);
                    Destroy(currentDraggedObject);

                    returning = true;
                }
            }

            currentDraggedObject.GetComponent<SpriteRenderer>().sortingLayerName = "Object";
            currentDraggedObject.GetComponent<DragObj>().isClicked = false;
            currentDraggedObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

            if (!returning)
                playSound.Play(dropSound);

            canvasGroup.blocksRaycasts = true;
            Destroy(gameObject);
        }
    }
}
