using UnityEngine;

public class OpenRotate : MonoBehaviour
{
    [SerializeField] private RectTransform imageTransform;

    public void RotateImage()
    {
        imageTransform.Rotate(0, 0, 180);
    }

}
