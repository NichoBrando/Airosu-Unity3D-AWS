using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private GameObject rocketPrefab;

    [SerializeField]
    private Transform cannonTransform;

    [SerializeField]
    private Transform tankTransform;
    private float shootCooldown;

    private Image shootTimerImage;

    [SerializeField]
    Player playerManager;

    [SerializeField]
    PlayerCondition playerCondition;

    public void AddShootCooldown()
    {
        shootCooldown = 1.5f;
    }

    public override void OnStartLocalPlayer()
    {
        GameObject shootTimerObject = GameObject.FindGameObjectWithTag("ShootTimer");
        shootTimerImage = shootTimerObject.GetComponent<Image>();
    }

    private void ShootMissile() {
        GameObject createdRocket = Instantiate(rocketPrefab);
        Rocket rocketConfigs = createdRocket.GetComponent<Rocket>();
        rocketConfigs.SetOwner(playerManager.ID);
        rocketConfigs.SetTransform(cannonTransform.position, tankTransform.eulerAngles);
        rocketConfigs.SetMovementForward(transform.forward);
        rocketConfigs.playerOwner = playerManager;
        AddShootCooldown();
    }

    [ClientRpc]
    private void RpcShootMissile()
    {
        if (isLocalPlayer) return;
        ShootMissile();
    }

    [Command]
    private void CmdShootMissile()
    {
        if (shootCooldown != 0) return;
        ShootMissile();
        RpcShootMissile();
    }

    private void UpdateShootCD()
    {
        if (!isLocalPlayer) return;
        shootTimerImage.fillAmount = 1 - shootCooldown / 1.5f;
    }

    private void FixedUpdate()
    {
        if (isServer || isLocalPlayer) {
            if (shootCooldown != 0 && !playerCondition.isDead) {
                shootCooldown -= Time.fixedDeltaTime;
                if (shootCooldown < 0) shootCooldown = 0;
            }
        }
        if (isLocalPlayer) UpdateShootCD();
    }

    private void Update()
    {
        if (isLocalPlayer) {
            if (Input.GetKeyDown(KeyCode.P) && shootCooldown == 0 && !playerCondition.isDead) {
                CmdShootMissile();
                if (!isServer) ShootMissile();
            }
        }
    }
}
