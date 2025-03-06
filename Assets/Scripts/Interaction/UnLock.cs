using System.Collections.Generic;
using UnityEngine;

public class UnLock : MonoBehaviour
{
    private List<Collider2D> overlapResults = new List<Collider2D>();
    public void ItemDrop()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        int count = GetComponent<Collider2D>().Overlap(filter, overlapResults);

        if (count > 0)
        {
            Collider2D overlapped = overlapResults[0];
            if(overlapped.gameObject.GetComponent<Special>() != null)
            {
                if (overlapped.gameObject.GetComponent<AudioSource>() != null)
                {
                    Destroy(overlapped.gameObject.GetComponent<AudioSource>());
                }
                Destroy(overlapped.gameObject.GetComponent<Special>());
                GetComponent<DragObj>().InputDisable();
                GameManager.Instance.DecreaseItemsCount();
                Destroy(gameObject);
            }
            else if (!overlapped.gameObject.CompareTag("Object"))
            {
                overlapped.gameObject.tag = "Object";
                if (overlapped.gameObject.GetComponent<AudioSource>() != null)
                {
                    Destroy(overlapped.gameObject.GetComponent<AudioSource>());
                }
                GetComponent<DragObj>().InputDisable();
                GameManager.Instance.DecreaseItemsCount();
                Destroy(gameObject);
            }
        }
    }
}
