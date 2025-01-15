using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager instance;
    public UnityEvent<string, int> SubmitScoreEvent;
    private void Start()
    {
        instance = this;
    }
    
    public void SubmitScore(string playername,int score)
    {
            SubmitScoreEvent.Invoke(playername,score);
    }
}
