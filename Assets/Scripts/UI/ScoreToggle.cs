using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace DontFall.UI
{
    public class ScoreToggle : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RectTransform score;
        [SerializeField] private RectTransform round;

        [SerializeField] private float animationDuration;
        [SerializeField] private Vector2 flipDistance;
        [SerializeField] private Vector2 stackOffset;

        private float animationPosition;
        private int currentFront;

        private int inputBuffer;

        public string ScoreText
        {
            get => score.GetComponentInChildren<TMP_Text>().text;
            set => score.GetComponentInChildren<TMP_Text>().text = value;
        }

        public string RoundText
        {
            get => round.GetComponentInChildren<TMP_Text>().text;
            set => round.GetComponentInChildren<TMP_Text>().text = value;
        }

        public int Score
        {
            get
            {
                return int.Parse(ScoreText);
            }
            set
            {
                ScoreText = string.Format("{0:D6}", value);
            }
        }

        public int Round
        {
            get
            {
                return int.Parse(RoundText);
            }
            set
            {
                RoundText = string.Format("Round {0}", value);
            }
        }

        private void Awake()
        {
            animationPosition = -1;
            currentFront = 0;
            inputBuffer = 0;
        }

        private void Start()
        {
            var rectTransform = transform as RectTransform;

            score.localPosition = rectTransform.rect.center;
            round.localPosition = rectTransform.rect.center + stackOffset;
        }

        private void Update()
        {
            var rectTransform = transform as RectTransform;

            if (animationPosition >= 0)
            {
                if (animationPosition < 0.5f)
                {
                    score.localPosition = rectTransform.rect.center + Vector2.Lerp(currentFront * stackOffset, (currentFront * 2 - 1) * flipDistance, animationPosition * 2);
                    round.localPosition = rectTransform.rect.center + Vector2.Lerp((1 - currentFront) * stackOffset, (1 - currentFront * 2) * flipDistance, animationPosition * 2);
                }
                else if (animationPosition < 1)
                {
                    score.localPosition = rectTransform.rect.center + Vector2.Lerp((1 - currentFront) * stackOffset, (currentFront * 2 - 1) * flipDistance, (1 - animationPosition) * 2);
                    round.localPosition = rectTransform.rect.center + Vector2.Lerp(currentFront * stackOffset, (1 - currentFront * 2) * flipDistance, (1 - animationPosition) * 2);
                }
                else
                {
                    animationPosition = -1;
                    currentFront = 1 - currentFront;
                    score.localPosition = rectTransform.rect.center + currentFront * stackOffset;
                    round.localPosition = rectTransform.rect.center + (1 - currentFront) * stackOffset;

                    if (inputBuffer > 0)
                    {
                        animationPosition = 0;
                        inputBuffer = 0;
                    }
                }

                if (animationPosition < 0.5f && animationPosition + Time.deltaTime / animationDuration >= 0.5f)
                {
                    (score, round) = (round, score);
                    (ScoreText, RoundText) = (RoundText, ScoreText);
                }
                if (animationPosition >= 0)
                {
                    animationPosition += Time.deltaTime / animationDuration;
                }
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (animationPosition < 0)
            {
                animationPosition = 0;
            }
            else
            {
                inputBuffer++;
            }
        }
    }
}
