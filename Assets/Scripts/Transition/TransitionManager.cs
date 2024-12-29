using System;
using UnityEngine;

namespace DontFall.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private Material transitionMaterial;
        [SerializeField] private float transitionDuration;
        [SerializeField] private Color transitionColor;
        [SerializeField] private float transitionAngle;
        [SerializeField] private AnimationCurve transitionCurve;
        [SerializeField] private bool runOnStart;

        private float progress;
        private Action callback;

        private void Awake()
        {
            progress = -1;
        }

        private void Start()
        {
            if (runOnStart)
            {
                StartTransition(true, () => { });
            }
        }

        private void Update()
        {
            if (progress >= 0 && progress < 1)
            {
                progress += Time.deltaTime / transitionDuration;

                transitionMaterial.SetFloat("_Progress", transitionCurve.Evaluate(progress));
            }
            else if (progress >= 1)
            {
                transitionMaterial.SetFloat("_Progress", transitionCurve.Evaluate(1));
                progress = -1;
                callback.Invoke();
            }
        }

        public void StartTransition(bool inverted, Action callback)
        {
            transitionMaterial.SetColor("_BlockColor", transitionColor);
            transitionMaterial.SetFloat("_Angle", transitionAngle);
            transitionMaterial.SetFloat("_Inverse", inverted ? 1 : 0);
            transitionMaterial.SetFloat("_Progress", transitionCurve.Evaluate(0));
            progress = 0;
            this.callback = callback;
        }
    }
}
