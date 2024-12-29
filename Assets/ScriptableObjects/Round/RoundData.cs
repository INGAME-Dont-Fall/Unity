using System.Linq;
using UnityEngine;

public enum Level
{ 
    Low,
    Medium, 
    High
}


[CreateAssetMenu(fileName = "RoundData", menuName = "Scriptable Objects/RoundData")]
public class RoundData : ScriptableObject
{
    public Level level;
    public int lowProbability;
    public int mediumProbability;
    public int highProbability;
    public int specialProbability;
}
