using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncScore))]
    public SyncList<int> teamPoints = new SyncList<int>();

    [SerializeField]
    private UpdateScoreBoard scoreBoardHandler;

    private void Awake()
    {
        teamPoints.Add(0);
        teamPoints.Add(0);
    }

    public void addPoint(int index)
    {
        teamPoints[index]++;
        scoreBoardHandler.UpdateScoreForTeam(index, teamPoints[index]);
        if (teamPoints[index] == 4) {
            WinGame(index);
        }
    }

    private void SyncScore(SyncList<int> oldValue, SyncList<int> newValue)
    {
        scoreBoardHandler.UpdateScoreForTeam(0, newValue[0]);
        scoreBoardHandler.UpdateScoreForTeam(1, newValue[1]);
    }

    private void WinGame(int teamIndex)
    {

    }
}
