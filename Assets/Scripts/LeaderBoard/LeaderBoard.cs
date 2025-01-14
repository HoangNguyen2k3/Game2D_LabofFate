using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Dan.Main;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> name_players;
    [SerializeField] private List<TextMeshProUGUI> scores_time;

    private string publicLeaderboardKey = "2e0b755aa58cfa9126300e8d8492678aa4306bd08526ae173f6f18082682a4a1";

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            for(int i=0;i<name_players.Count; i++)
            {
                name_players[i].text = msg[i].Username;
                scores_time[i].text = msg[i].Score.ToString();
            }
        }));

    }
    public void SetLeaderboardEntry(string username,int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey,username,score, ((msg) =>
        {

            GetLeaderboard();
        }
        ));
    }
}
