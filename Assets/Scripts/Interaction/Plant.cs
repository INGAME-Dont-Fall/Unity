using UnityEngine;
using UnityEngine.EventSystems;

public class Plant : MonoBehaviour
{
    public int index;
    [SerializeField] private ObjectData[] plants;
    private GameObject objGroup;

    private void Awake()
    {
        index = 1;
    }

    public void GetWateringCan(Transform transform)
    {
        GameObject go = Instantiate(plants[index].go, transform);
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        go.GetComponent<Plant>().index = index + 1;

        GameManager.Instance.CanvasGroup.blocksRaycasts = true;
        Destroy(gameObject);
    }
}
