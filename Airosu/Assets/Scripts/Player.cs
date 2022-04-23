using UnityEngine;
using Mirror;
using Singletons;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private NetworkIdentity networkIdentity;

    [SerializeField]
    private MeshRenderer teamIdentifier;

    [SyncVar]
    private int teamIndex;

    public uint ID {
        get => networkIdentity.netId;
    }

    public override void OnStartServer()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        teamIndex = playerList.Length % 2 == 0 ? 0 : 1;
        GameManagerSingleton.instance.playerTeamIndex = teamIndex;
        GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (spawnPositions[teamIndex] != null) {
            transform.position = spawnPositions[teamIndex].gameObject.transform.position;
        }
    }

    public override void OnStartLocalPlayer()
    {
        teamIdentifier.material = MaterialsSingleton.instance.Owner;
    }

    public void Score()
    {
        if (isServer) {
            GameManagerSingleton.instance.addPoint(teamIndex);
        }
    }
}
