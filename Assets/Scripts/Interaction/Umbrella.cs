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
                if (overlapped.CompareTag("UmbrellaStand"))
                {
                    Instantiate(foldUmbrella, overlapped.gameObject.transform);
                    Destroy(gameObject);
                }
            }
        }
    }
}
