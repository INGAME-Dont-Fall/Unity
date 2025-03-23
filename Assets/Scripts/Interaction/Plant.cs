using UnityEngine;
using UnityEngine.EventSystems;

public class Plant : Special
{
    public int index;
    [SerializeField] private ObjectData[] plants;
    private GameObject objGroup;

    public void GetWateringCan(Transform transform)
    {
        GameManager.Instance.AddObject(plants[GetComponent<Plant>().index + 1], false);
        GameManager.Instance.CanvasGroup.blocksRaycasts = true;
        GameManager.Instance.DecreaseItemsCount();
        Destroy(gameObject);

    }
}
