using UnityEngine;
using TMPro;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text maxScore;
    [SerializeField] private ScoreSO scoreData;

    void Start()
    {
        score.text = "" + scoreData.score;
        maxScore.text = "" + scoreData.maxScore;
    }
}
