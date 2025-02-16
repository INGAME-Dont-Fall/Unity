using UnityEngine;

namespace DontFall.UI
{
    public class Settings : MonoBehaviour
    {
        public void OpenOption()
        {
            Time.timeScale = 0;
        }

        public void CloseOption()
        {
            Time.timeScale = 1;
        }
    }
}
