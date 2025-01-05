using UnityEngine;

public class MergeObject : MonoBehaviour
{
    private bool isClick;

    private void OnMouseDown()
    {
        isClick = false;
    }

    private void OnMouseDrag()
    {
        Vector3 vpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vpos.z = 0f;
        transform.position = vpos;
    }

    private void OnMouseUp()
    {
        isClick = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        string clickObject = transform.name.Substring(transform.name.LastIndexOf("_") + 1);
        string collisionObject = collision.transform.name.Substring(collision.transform.name.LastIndexOf("_") + 1);


        if (transform.name == collision.transform.name)
        {
            // GameObject newObject = (GameObject)Instantiate(Resources.Load("ItemCode_" + 0), transform.position, Quaternion.identity);
            // newObject.name = "ItemCode_" + 0;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}