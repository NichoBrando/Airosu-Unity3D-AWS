using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private NetworkIdentity networkIdentity;

    [SerializeField]
    private MeshRenderer teamIdentifier;

    [SyncVar]
    private int teamIndex;

    private GameManager gameManager;

    public uint ID {
        get => networkIdentity.netId;
    }

    public override void OnStartServer()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        teamIndex = playerList.Length % 2 == 0 ? 0 : 1;
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameController");
        Debug.Log(gameManagerObject);
        if (gameManagerObject != null) {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
    }

    public override void OnStartLocalPlayer()
    {
        GameObject utils = GameObject.FindGameObjectWithTag("Utils");
        if (utils != null) {
            Materials MaterialsUtils = utils.GetComponent<Materials>();
            teamIdentifier.material = MaterialsUtils.Owner;
        }
    }

    public void Score()
    {
        if (isServer) {
            gameManager.addPoint(teamIndex);
        }
    }
}
