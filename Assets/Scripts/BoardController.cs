using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontFall
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField, Range(-0.5f, 0.5f)] private float pivot;
        [SerializeField] private float width;

        [SerializeField] private Vector2 edge1, edge2;

        public float Pivot
        {
            get => pivot;
            set
            {
                pivot = Mathf.Clamp(value, -0.5f, 0.5f);
                
                var joint = GetComponent<HingeJoint2D>();
                joint.anchor = pivot * Vector2.right;
                joint.connectedAnchor = Vector2.Lerp(edge1, edge2, pivot + 0.5f);
            }
        }

        public Vector2 Edge1 => edge1;
        public Vector2 Edge2 => edge2;

        public void SetBoard()
        {
            transform.position = Vector2.Lerp(edge1, edge2, 0.5f);
            transform.localScale = new Vector3(Vector2.Distance(edge1, edge2), width, 1);
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(edge2.y - edge1.y, edge2.x - edge1.x) * Mathf.Rad2Deg, Vector3.forward);

            var rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = Vector2.zero;
            rigid.angularVelocity = 0;

            Pivot = Pivot;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(edge1, edge2);
            Gizmos.DrawWireSphere(Vector2.Lerp(edge1, edge2, pivot + 0.5f), 0.1f);
        }
    }
}
