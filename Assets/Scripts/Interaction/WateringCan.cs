using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
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
                if (overlapped.gameObject.GetComponent<Plant>() != null)
                {
                    GameManager.Instance.DecreaseItemsCount();
                    overlapped.gameObject.GetComponent<Plant>().GetWateringCan(transform.parent);
                    Destroy(gameObject);
                }
            }
        }
    }
}
