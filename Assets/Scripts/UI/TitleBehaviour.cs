using UnityEngine;
using DontFall.Transition;

namespace DontFall.UI
{
    public class TitleBehaviour : MonoBehaviour
    {
        [SerializeField] private TransitionManager transitionManager;

        [SerializeField] private GameObject cover;
        [SerializeField] private DropObjects dropObjects;

        [SerializeField] private SceneMove sceneMove;

        public void PlayPressed()
        {
            transitionManager.StartTransition(false, false, () =>
            {
                cover.SetActive(false);
                dropObjects.OnStart();
                transitionManager.StartTransition(true, false, () =>
                {
                    Invoke(nameof(Delayed), 7);
                });
            });
        }

        private void Delayed()
        {
            sceneMove.MoveScene("PlayScene");
        }
    }
}
