using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DontFall.Transition;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textTotalScore;
    [SerializeField] private TMP_Text textRoundScore;
    [SerializeField] private ScoreSO scoreData;
    [SerializeField] TransitionManager transitionManager;

    void Start()
    {
        textTotalScore.text = "" + scoreData.totalScore;
        textRoundScore.text = "" + scoreData.roundScore;
    }

    public void LoadPlayScene()
    {
        transitionManager.StartTransition(true, true, ()=>
        {
            SceneManager.LoadScene("PlayScene");
        });
    }
}
