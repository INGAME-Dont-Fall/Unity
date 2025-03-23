using UnityEngine;

namespace DontFall.UI
{
    public class DropObjects : MonoBehaviour
    {
        public void OnStart()
        {
            foreach (var child in GetComponentsInChildren<Rigidbody2D>())
            {
                child.simulated = true;
            }
        }
    }
}
