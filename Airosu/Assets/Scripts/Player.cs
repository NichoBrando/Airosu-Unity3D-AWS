using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private NetworkIdentity networkIdentity;

    [SerializeField]
    private MeshRenderer teamIdentifier;

    public uint ID {
        get => networkIdentity.netId;
    }

    public override void OnStartLocalPlayer()
    {
        GameObject utils = GameObject.FindGameObjectWithTag("Utils");
        if (utils != null) {
            Materials MaterialsUtils = utils.GetComponent<Materials>();
            teamIdentifier.material = MaterialsUtils.Owner;
        }
    }
}
