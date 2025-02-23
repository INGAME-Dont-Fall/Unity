using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fish : MonoBehaviour
{
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
                if (overlapped.CompareTag("FishTank"))
                {
                    Destroy(GetComponent<Rigidbody2D>());
                    Destroy(GetComponent<Collider2D>());
                    transform.SetParent(overlapped.transform);
                    overlapped.transform.localPosition = Vector3.zero;
                    overlapped.transform.localRotation = Quaternion.identity;
                    GameManager.Instance.DecreaseItemsCount();
                    Destroy(this);
                }
            }
        }
    }
}
