using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
                if (overlapped.CompareTag("FishTank"))
                {
                    fish.isTrigger = true;

                    GetComponent<Rigidbody2D>().simulated = false;

                    animator.SetTrigger("Stop");
                    transform.SetParent(overlapped.transform);
                    transform.localPosition = new Vector3(0.0f, -0.5f, 0.0f);
                    transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    transform.localRotation = Quaternion.identity;

                    Destroy(this);
                }
            }
        }
    }
}
