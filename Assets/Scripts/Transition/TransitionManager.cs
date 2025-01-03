using System;
using UnityEngine;

namespace DontFall.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        public enum TransitionType
        {
            Linear,
            Circular,
        }

        [SerializeField] private Material transitionMaterial;
        [SerializeField] private TransitionType transitionType;
        [SerializeField] private float transitionAngle;
        [SerializeField] private Vector2 transitionCenter;
        [SerializeField] private float transitionDuration;
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
                StartTransition(true, false, () => { });
            }
            else
            {
                transitionMaterial.SetFloat("_Invert", 0);
                transitionMaterial.SetFloat("_Reverse", 0);
                transitionMaterial.SetFloat("_Progress", 0);
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

        public void StartTransition(bool inverted, bool reverse, Action callback)
        {
            switch (transitionType)
            {
                case TransitionType.Linear:
                    transitionMaterial.EnableKeyword("_TYPE_LINEAR");
                    break;
                case TransitionType.Circular:
                    transitionMaterial.EnableKeyword("_TYPE_CIRCULAR");
                    break;
            }

            transitionMaterial.SetFloat("_LinearAngle", transitionAngle);
            transitionMaterial.SetVector("_CircularCenter", transitionCenter);

            transitionMaterial.SetFloat("_Invert", inverted ? 1 : 0);
            transitionMaterial.SetFloat("_Reverse", reverse ? 1 : 0);
            transitionMaterial.SetFloat("_Progress", transitionCurve.Evaluate(0));

            progress = 0;
            this.callback = callback;
        }
    }
}
