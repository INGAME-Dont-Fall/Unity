using UnityEngine;
using UnityEngine.UI;

namespace DontFall.UI
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Image display;

        [SerializeField] private Sprite[] slides;

        private int index;

        private void Awake()
        {
            index = 0;
        }

        private void Start()
        {
            display.sprite = slides[index];
        }

        public void Previous()
        {
            if (index > 0)
                display.sprite = slides[--index];
        }

        public void Next()
        {
            if (index < slides.Length - 1)
                display.sprite = slides[++index];
        }
    }
}
