using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DontFall.Transition;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance { get; private set; } 

    [SerializeField] private TMP_Text textTotalScore;
    [SerializeField] private TMP_Text textRoundScore;
    public ScoreSO scoreData;
    public PlayData playData;

    void Start()
    {
        textTotalScore.text = string.Format("{0:D6}", scoreData.totalScore);
        textRoundScore.text = string.Format("{0:D6}", scoreData.round);
        GetComponent<SaveMaxScore>().ExcuteSave(scoreData.totalScore);

        playData.currentRound = 1;
        playData.totalScore = 0;
    }
}
