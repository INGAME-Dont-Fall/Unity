using UnityEngine;

[CreateAssetMenu(fileName = "PlayData", menuName = "Scriptable Objects/PlayData")]
public class PlayData : ScriptableObject
{
    public int currentRound;
    public int totalScore;

}
