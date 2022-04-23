using System.Collections;
using UnityEngine;
using Mirror;

public class PlayerCoroutines : NetworkBehaviour
{
    private PostProcess shaderController;

    [SerializeField]
    private PlayerCondition playerCondition;

    [SerializeField]
    private PlayerShoot playerShoot;

    [SerializeField]
    private PlayerLife playerLife;

    [SerializeField]
    private GameObject playerHalo;

    [SerializeField]
    private Material greyMaterial;

    [SerializeField]
    private MeshRenderer teamIdentifier;

    [ClientRpc]
    public void RpcStartClientResurrect()
    {
        StartCoroutine(ResurrectPlayer());
    }

    public void ShareResurrect()
    {
        StartCoroutine(ResurrectPlayer());
        RpcStartClientResurrect();
    }

    public override void OnStartLocalPlayer()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        shaderController = camera.gameObject.GetComponent<PostProcess>();
    }



    private IEnumerator ResurrectPlayer()
    {
        Material defaultIdentifier = teamIdentifier.material;
        teamIdentifier.material = greyMaterial;
        if (isLocalPlayer) {
            StartCoroutine(shaderController.SetGrayscale(1f, 0.5f));
        }
        yield return new WaitForSeconds(3f);
        teamIdentifier.material = defaultIdentifier;
        playerHalo.SetActive(true);
        if (isLocalPlayer) {
            StartCoroutine(shaderController.SetGrayscale(0f, 0.5f));
        }
        if (isServer) {
            playerLife.ResetLife();
            playerCondition.isDead = false;
            playerCondition.isInvincible = true;
            playerShoot.AddShootCooldown();
        }
        yield return new WaitForSeconds(1.5f);
        if (isServer) {
            playerCondition.isInvincible = false;
        }
        playerHalo.SetActive(false);
    }
}
