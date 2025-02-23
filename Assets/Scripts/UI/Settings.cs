using UnityEngine;

namespace DontFall.UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private bool startClosed;

        private void Awake()
        {
            if (startClosed)
                CloseOption();
        }

        public void OpenOption()
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }

        public void CloseOption()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}
