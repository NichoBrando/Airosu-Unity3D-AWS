using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : NetworkBehaviour
{
    [SyncVar]
    private float life = 4f;

    Image lifeBarImage;

    [SerializeField]
    PlayerCondition playerCondition;

    [SerializeField]
    PlayerCoroutines playerCoroutines;

    public override void OnStartLocalPlayer()
    {
        GameObject lifeBarObject = GameObject.FindGameObjectWithTag("LifeBar");
        lifeBarImage = lifeBarObject.GetComponent<Image>();
    }

    private void UpdateLife()
    {
        if (!playerCondition.isDead && life < 4) {
            life += 1 * Time.fixedDeltaTime / 10;
        }
        if (life > 4) life = 4f;
        if (isLocalPlayer) lifeBarImage.fillAmount = life / 4;
    }

    private void FixedUpdate()
    {
        UpdateLife();
    }

    public bool ReceiveDamage()
    {
        if (isServer) {
            if (playerCondition.isInvincible) return false;
            life -= 1;
            if (life <= 0)
            {
                playerCondition.isDead = true;
                playerCoroutines.ShareResurrect();
                return true;
            }
        }
        return false;
    }

    public void ResetLife()
    {
        if (isServer) life = 4f;
    }
}
