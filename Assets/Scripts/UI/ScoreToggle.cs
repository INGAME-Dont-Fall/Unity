using UnityEngine;
using UnityEngine.EventSystems;

namespace DontFall.UI
{
    public class ScoreToggle : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RectTransform[] items;
        [SerializeField] private int initialIndex;

        private int Index
        {
            get
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].gameObject.activeSelf)
                        return i;
                }
                return -1;
            }
            set
            {
                for (int i = 0; i < items.Length; i++)
                {
                    items[i].gameObject.SetActive(i == value);
                }
            }
        }

        private void Start()
        {
            Index = initialIndex;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Index = (Index + 1) % items.Length;
        }
    }
}
