using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    [SerializeField] private GameObject foldUmbrella;

    private List<Collider2D> overlapResults = new List<Collider2D>();


    public void ItemDrop()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        int count = GetComponent<Collider2D>().Overlap(filter, overlapResults);

        if (count > 0)
        {
            foreach (Collider2D overlapped in overlapResults)
            {
                if (overlapped.gameObject.GetComponent<UmbrellaStand>() != null)
                {
                    if (overlapped.gameObject.GetComponent<UmbrellaStand>().IncreaseCount())
                    {
                        GameObject go = Instantiate(foldUmbrella, overlapped.gameObject.transform);
                        go.transform.localPosition = overlapped.gameObject.GetComponent<UmbrellaStand>().GetPosition();
                        go.transform.localRotation = Quaternion.Euler(overlapped.gameObject.GetComponent<UmbrellaStand>().GetRotation());

                        overlapped.gameObject.GetComponent<Rigidbody2D>().simulated = false;
                        overlapped.gameObject.GetComponent<Rigidbody2D>().simulated = true;
                        GameManager.Instance.DecreaseItemsCount();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
