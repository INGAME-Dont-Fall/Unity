using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "Scriptable Objects/Score")]
public class ScoreSO : ScriptableObject
{
    public int score;
    public int maxScore;
}