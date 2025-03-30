using UnityEngine;

namespace DontFall.Objects
{
    public class Safe : Special
    {
        [SerializeField] private Sprite closed;

        public void ItemDropped(object obj)
        {
            if (obj is GameObject gobj)
            {
                var renderer = GetComponent<SpriteRenderer>();
                renderer.sprite = closed;

                var rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.mass += gobj.GetComponent<Rigidbody2D>().mass;

                foreach (var collider in GetComponents<Collider2D>())
                {
                    collider.enabled = !collider.enabled;
                }

                gobj.gameObject.SetActive(false);
            }
        }
    }
}
