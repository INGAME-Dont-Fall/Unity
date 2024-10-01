using UnityEngine;

namespace DontFall.Board
{
    [RequireComponent(typeof(HingeJoint2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private float pivot = 0;
        [SerializeField] private Vector2 clamp = new Vector2(0.1f, 0.9f);

        [SerializeField] private Vector2 edge1 = new Vector2(-1, 0), edge2 = new Vector2(1, 0);
        
        [SerializeField] private GameObject clampObject;

        public float Pivot
        {
            get => pivot;
            set
            {
                pivot = Mathf.Clamp(value, clamp.x - 0.5f, clamp.y - 0.5f);
                
                var joint = GetComponent<HingeJoint2D>();
                joint.anchor = pivot * Vector2.right;
                joint.connectedAnchor = Vector2.Lerp(edge1, edge2, pivot + 0.5f);
            }
        }

        public Vector2 Edge1 => edge1;
        public Vector2 Edge2 => edge2;
        public Vector2 Clamp => clamp;

        public void SetBoard()
        {
            var renderer = GetComponent<SpriteRenderer>();
            var collider = GetComponent<BoxCollider2D>();

            transform.position = Vector2.Lerp(edge1, edge2, 0.5f);
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(edge2.y - edge1.y, edge2.x - edge1.x) * Mathf.Rad2Deg, Vector3.forward);

            renderer.size = collider.size = new Vector2((edge2 - edge1).magnitude, renderer.size.y);

            Pivot = Pivot;

            if (clampObject != null)
            {
                var clampRenderer = clampObject.GetComponent<SpriteRenderer>();

                Vector2 clampedEdge1 = Vector2.Lerp(edge1, edge2, clamp.x), clampedEdge2 = Vector2.Lerp(edge1, edge2, clamp.y);

                clampObject.transform.position = Vector2.Lerp(clampedEdge1, clampedEdge2, 0.5f);
                clampObject.transform.localRotation = Quaternion.identity;

                clampRenderer.size = new Vector2((clampedEdge2 - clampedEdge1).magnitude, clampRenderer.size.y);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(edge1, edge2);
            Gizmos.DrawWireSphere(Vector2.Lerp(edge1, edge2, pivot + 0.5f), 0.1f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Vector2.Lerp(edge1, edge2, clamp.x), 0.1f);
            Gizmos.DrawWireSphere(Vector2.Lerp(edge1, edge2, clamp.y), 0.1f);
        }
    }
}
