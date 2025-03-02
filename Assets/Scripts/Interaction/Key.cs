using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private ObjectData data;
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
                if(overlapped.CompareTag("Lock"))
                {
                    overlapped.gameObject.GetComponent<Lock>().GetKey();
                    Destroy(gameObject);
                }
            }
        }
    }
}
