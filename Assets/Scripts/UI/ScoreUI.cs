using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DontFall.Transition;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textTotalScore;
    [SerializeField] private TMP_Text textRoundScore;
    [SerializeField] private ScoreSO scoreData;

    void Start()
    {
        textTotalScore.text = string.Format("{0:D6}", scoreData.totalScore);
        textRoundScore.text = string.Format("{0:D6}", scoreData.roundScore);
    }
}
