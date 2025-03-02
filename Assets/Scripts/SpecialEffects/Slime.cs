using UnityEngine;

namespace DontFall.Objects
{
    public class Slime : Special
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Object"))
            {
                var contacts = new ContactPoint2D[collision.contactCount];
                collision.GetContacts(contacts);

                foreach (var contact in contacts)
                {
                    var impulse = contact.normalImpulse * contact.normal + contact.tangentImpulse * Vector2.Perpendicular(contact.normal);

                    collision.otherRigidbody.AddForceAtPosition(impulse, contact.point, ForceMode2D.Impulse);
                    collision.rigidbody.AddForceAtPosition(-impulse, contact.point, ForceMode2D.Impulse);
                }
            }
        }
    }
}
