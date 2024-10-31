using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace DontFall
{
    public class PlayUIManager : MonoBehaviour
    {
        private VisualElement root;

        [SerializeField] private UnityEvent<float> onZoomChange;
        [SerializeField] private UnityEvent onPressedPlay;

        public int Score
        {
            set
            {
                root.Q<Label>("Score").text = value.ToString("D6");
            }
        }

        private void Start()
        {
            var document = GetComponent<UIDocument>();
            root = document.rootVisualElement;

            Score = 0;

            var zoom = root.Q<Slider>("Zoom");
            zoom.value = zoom.highValue;

            var play = root.Q<Button>("Play");
            play.clicked += () => onPressedPlay.Invoke();

            var overlayOpen = root.Q<Button>("OverlayOpen");
            var overlayClose = root.Q<Button>("OverlayClose");
            var overlay = root.Q("Overlay");
            overlayOpen.clicked += () =>
            {
                overlayOpen.style.display = DisplayStyle.None;
                overlayClose.style.display = DisplayStyle.Flex;
                overlay.style.translate = new StyleTranslate(new Translate(0, 0));
            };
            overlayClose.clicked += () =>
            {
                overlayClose.style.display = DisplayStyle.None;
                overlayOpen.style.display = DisplayStyle.Flex;
                overlay.style.translate = new StyleTranslate(new Translate(222, 0));
            };
        }

        private void Update()
        {
            // Slider input is kinda discrete for some reason...
            onZoomChange.Invoke(root.Q<Slider>("Zoom").value * 1e-1f);
        }
    }
}
