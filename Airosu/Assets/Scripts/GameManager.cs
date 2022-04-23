using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public SyncList<int> teamPoints = new SyncList<int>();

    [SerializeField]
    private UpdateScoreBoard scoreBoardHandler;

    [SyncVar]
    public bool IsGameFinished = false;

    [SerializeField]
    private GameObject winNotifier;
    [SerializeField]
    private Text winNotifierMessage;
    public int playerTeamIndex;

    private void Awake()
    {
        teamPoints.Add(0);
        teamPoints.Add(0);
    }

    public void addPoint(int index)
    {
        teamPoints[index]++;
        RpcSyncScore(index, teamPoints[index]);
        if (teamPoints[index] == 4) {
            RpcWinGame(index);
            WinGame(index);
        }
    }

    [ClientRpc]
    private void RpcSyncScore(int index, int value)
    {
        scoreBoardHandler.UpdateScoreForTeam(index, value);
    }

    [ClientRpc]
    private void RpcWinGame(int teamIndex) 
    {
        WinGame(teamIndex);
    }

    private void WinGame(int teamIndex)
    {
        IsGameFinished = true;
        winNotifier.SetActive(true);
        string result = playerTeamIndex == teamIndex ? "won" : "lost";
        string message = $"You {result} this game!";
        winNotifierMessage.text = message;
    }
}
