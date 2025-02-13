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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WateringCan"))
        {
            objGroup = collision.transform.parent.gameObject;

            GameObject go = Instantiate(plants[index].go,objGroup.transform);
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
            go.GetComponent<Plant>().index = index + 1;

            GameManager.Instance.CanvasGroup.blocksRaycasts = true;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
