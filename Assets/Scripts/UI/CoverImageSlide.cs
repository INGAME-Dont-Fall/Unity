using System.Collections;
using UnityEngine;

namespace DontFall.UI
{
    public class CoverImageSlide : MonoBehaviour
    {
        [SerializeField] private float speed;

        public void StartSlide()
        {
            StartCoroutine(Slide());
        }

        private IEnumerator Slide()
        {
            var rectTransform = GetComponent<RectTransform>();

            var width = rectTransform.rect.width;
            var initPos = rectTransform.anchoredPosition;

            while ((initPos - rectTransform.anchoredPosition).sqrMagnitude < width * width)
            {
                rectTransform.anchoredPosition += Vector2.left * (Time.deltaTime * width * speed);
                yield return null;
            }
        }
    }
}
