using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int index = 0;
    private CanvasGroup canvasGroup;
    private GameObject currentDraggedObject;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponentInParent<CanvasGroup>();
    }

    //�巡�� ���� �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        //�θ� ������Ʈ�� �κ��丮�� ��ĭ�� ��
        GameObject go = gameObject.transform.parent.gameObject;
        GameManager.Instance.EmptyInventory.Add(go);
        canvasGroup.blocksRaycasts = false;

        //�̹����� ���� ������ ������Ʈ�� ����
        gameObject.GetComponent<Image>().enabled = false;
        currentDraggedObject = Instantiate(GameManager.Instance.GamePrefab[index].gameObj);
        currentDraggedObject.GetComponent<DragObj>().index = index;
    }

    //�巡�� �� ��� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        // �巡���� ������Ʈ�� ���콺�� ���� �����̰� ����
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        currentDraggedObject.transform.position = mousePos;
    }

    //�巡�� ���� �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(gameObject);
        canvasGroup.blocksRaycasts = true;
    }
}
