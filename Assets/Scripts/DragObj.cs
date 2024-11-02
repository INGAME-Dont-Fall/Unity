using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class DragObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index = 0;
    private Canvas canvas;  // ������Ʈ�� ����� UI ĵ����
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvas = GameManager.Instance.Canvas;
        canvasGroup = canvas.GetComponent<CanvasGroup>();
    }

    //�巡�� ���� �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    // �巡�� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        // ���콺�� ��������� ������Ʈ ��ġ ����
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, gameObject.transform.position.z);
    }

    // �巡�� ���� �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (GameObject go in GameManager.Instance.EmptyInventory)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(go.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), canvas.worldCamera))
            {
                // UI �������� �����ϰ� �ش� UI�� ���ӽ�Ŵ
                GameObject newUIObject = Instantiate(GameManager.Instance.GamePrefab[index].gameUI, go.transform);
                newUIObject.GetComponent<DragUI>().index = index;

                //ĭ�� á���ϱ� ����
                //GameManager.Instance.EmptyInventory.Remove(go);
                Destroy(gameObject);
            }
        }

        canvasGroup.blocksRaycasts = true;
    }
}
