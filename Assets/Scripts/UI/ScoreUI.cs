using UnityEngine;
using TMPro;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textTotalScore;
    [SerializeField] private TMP_Text textRoundScore;
    [SerializeField] private ScoreSO scoreData;

    void Start()
    {
        textTotalScore.text = "" + scoreData.totalScore;
        textRoundScore.text = "" + scoreData.roundScore;
    }
}
