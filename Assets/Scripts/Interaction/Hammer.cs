using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
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
                if (overlapped.gameObject.GetComponent<Daruma>() != null)
                {
                    if(overlapped.gameObject.GetComponent<Daruma>().RemoveBlock())
                    {
                        if (Random.Range(0, 10) < 8)
                        {
                            GameManager.Instance.AddObject(data, false);
                        }

                        GameManager.Instance.DecreaseItemsCount();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
