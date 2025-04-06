using UnityEngine;

namespace DontFall.UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private bool startClosed;

        public bool Openned { get; private set; }

        private void Awake()
        {
            Openned = !startClosed;
            if (!Openned)
                CloseOption();
        }

        public void OpenOption()
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
            Openned = true;
        }

        public void CloseOption()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            Openned = false;
        }
    }
}
