using UnityEngine;

namespace DontFall.UI
{
    public class OverlayController : MonoBehaviour
    {
        [SerializeField] private bool initiallyShowing;
        [SerializeField] private AnimationCurve interpolationCurve;
        [SerializeField] private float interpolationSpeed;

        private bool showing;
        private Vector2 previous;
        private Vector2 target;
        private float animationTimer;

        private void Awake()
        {
            showing = initiallyShowing;
            previous = target = (transform as RectTransform).pivot;
            animationTimer = 1;
        }

        private void Update()
        {
            var rectTransform = transform as RectTransform;

            if (animationTimer < 1)
            {
                animationTimer += Time.deltaTime * interpolationSpeed;
                rectTransform.pivot = Vector2.Lerp(previous, target, interpolationCurve.Evaluate(animationTimer));
            }
            else
            {
                rectTransform.pivot = target;
            }
        }

        public void Toggle()
        {
            previous = target;
            if (showing = !showing)
            {
                previous = target;
                target.x = 1;
            }
            else
            {
                target.x = 0;
            }
            animationTimer = 0;
        }
    }
}
