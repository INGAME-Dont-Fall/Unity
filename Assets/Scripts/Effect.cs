using UnityEngine;

public class Effect : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(transform.parent.gameObject);
    }
}
