using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private Collider2D fish;

    private List<Collider2D> overlapResults = new List<Collider2D>();
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fish.isTrigger = true;
    }

    public void ItemDrop()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        fish.isTrigger = false;

        int count = fish.Overlap(filter, overlapResults);

        if (count > 0)
        {
            foreach (Collider2D overlapped in overlapResults)
            {
                if (overlapped.gameObject.GetComponent<FishTank>() != null)
                {
                    if (overlapped.gameObject.GetComponent<FishTank>().hasFish) return;

                    fish.isTrigger = true;

                    GetComponent<Rigidbody2D>().simulated = false;

                    animator.SetTrigger("Stop");
                    transform.SetParent(overlapped.transform);
                    GetComponent<DragObj>().DifficultyLevel = 25;
                    transform.localPosition = new Vector3(0.0f, -0.5f, 0.0f);
                    transform.localScale = transform.localScale * 0.5f;
                    transform.localRotation = Quaternion.identity;
                    GameManager.Instance.DecreaseItemsCount();
                    overlapped.gameObject.GetComponent<FishTank>().hasFish = true;
                    Destroy(this);
                }
            }
        }
    }
}
