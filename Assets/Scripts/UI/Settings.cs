using UnityEngine;

namespace DontFall.UI
{
    public class Settings : MonoBehaviour
    {
        private void Awake()
        {
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
