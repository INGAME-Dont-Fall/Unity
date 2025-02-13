using UnityEngine;
using UnityEngine.SceneManagement;

namespace DontFall
{
    public class SceneMove : MonoBehaviour
    {
        [SerializeField] private Transition.TransitionManager transition;

        public void MoveScene(string sceneName)
        {
            if (transition)
                transition.StartTransition(true, true, () => SceneManager.LoadScene(sceneName));
            else
                SceneManager.LoadScene(sceneName);
        }
    }
}
