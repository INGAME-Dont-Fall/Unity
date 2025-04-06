using DontFall.Transition;
using UnityEngine;

namespace DontFall.UI
{
    public class TitleBehaviour : MonoBehaviour
    {
        [SerializeField] private TransitionManager transitionManager;

        [SerializeField] private GameObject cover;
        [SerializeField] private DropObjects dropObjects;

        [SerializeField] private SceneMove sceneMove;

        [SerializeField] private float delay;

        public void PlayPressed()
        {
            transitionManager.StartTransition(false, false, () =>
            {
                cover.SetActive(false);
                dropObjects.OnStart();
                transitionManager.StartTransition(true, false, () =>
                {
                    Invoke(nameof(Delayed), delay);
                });
            });
        }

        private void Delayed()
        {
            sceneMove.MoveScene("PlayScene");
        }
    }
}
