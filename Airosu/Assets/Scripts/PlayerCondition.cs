using Mirror;

public class PlayerCondition : NetworkBehaviour
{
    [SyncVar]
    public bool isDead = false;
    [SyncVar]
    public bool isInvincible = false;
}
