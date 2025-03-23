using Unity.VisualScripting;
using UnityEngine;

public class InitPlayData : MonoBehaviour
{
    [SerializeField] PlayData playdata;

    private void Awake()
    {
        playdata.totalScore = 0;
        playdata.currentRound = 1;
    }
}
