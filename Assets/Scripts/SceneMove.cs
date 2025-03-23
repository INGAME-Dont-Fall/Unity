using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DontFall
{
    public class SceneMove : MonoBehaviour
    {
        [SerializeField] private Transition.TransitionManager transition;

        [SerializeField] private float delay;

        public void MoveScene(string sceneName)
        {
            StartCoroutine(PrivMoveScene(sceneName));
        }

        private IEnumerator PrivMoveScene(string sceneName)
        {
            yield return new WaitForSeconds(delay);

            if (transition)
                transition.StartTransition(true, true, () => SceneManager.LoadScene(sceneName));
            else
                SceneManager.LoadScene(sceneName);
        }
    }
}
