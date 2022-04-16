using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody body;

    [SerializeField]
    private Transform cannonTransform;

    [SerializeField]
    private Transform tankTransform;

    [SerializeField]
    private GameObject rocketPrefab;
    private float shootCooldown = 0f;

    private Image shootTimerImage;
    private int id = 4;

    [SyncVar]
    private float life = 4f;

    private Image lifeBarImage;

    private bool isDead = false;

    private bool isInvincible;

    private PostProcess shaderController;

    public int ID {
        get => id;
    }

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer) {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            shaderController = camera.gameObject.GetComponent<PostProcess>();
            GameObject shootTimerObject = GameObject.FindGameObjectWithTag("ShootTimer");
            shootTimerImage = shootTimerObject.GetComponent<Image>();
            GameObject lifeBarObject = GameObject.FindGameObjectWithTag("LifeBar");
            lifeBarImage = lifeBarObject.GetComponent<Image>();
        }
    }

    private void UpdateShootCD()
    {
        if (isServer) {
            if (shootCooldown != 0 && !isDead) {
                shootCooldown -= Time.deltaTime;
                if (shootCooldown < 0) shootCooldown = 0;
                shootTimerImage.fillAmount = 1 - shootCooldown / 1.5f;
            }
            else if(isDead) {
                shootTimerImage.fillAmount = 0;
            }
        }
    }

    private void UpdateLife()
    {
        if (!isDead && life < 4) {
            life += 1 * Time.deltaTime / 10;
        }
        if (life > 4) life = 4f;
        lifeBarImage.fillAmount = life / 4;
    }

    void Update()
    {
        if (isLocalPlayer) {
            if (Input.GetKeyDown(KeyCode.P) && shootCooldown == 0 && !isDead) {
                ShootMissile();
            }
            UpdateLife();
            UpdateShootCD();
        }
    }

    public void ReceiveDamage()
    {
        if (isServer) {
            if (isInvincible) return;
            life -= 1;
            if (life <= 0)
            {
                isDead = true;
                StartCoroutine(ResurrectPlayer());
            }
        }
    }

    private void ShootMissile()
    {
        GameObject createdRocket = Instantiate(rocketPrefab);
        Rocket rocketConfigs = createdRocket.GetComponent<Rocket>();
        rocketConfigs.SetTransform(cannonTransform.position, tankTransform.eulerAngles);
        rocketConfigs.SetOwner(id);
        rocketConfigs.SetMovementForward(transform.forward);
        shootCooldown = 1.5f;
    }

    private IEnumerator ResurrectPlayer()
    {
        if (isLocalPlayer) {
            StartCoroutine(shaderController.SetGrayscale(1f, 0.5f));
        }
        yield return new WaitForSeconds(3f);
        if (isLocalPlayer) {
            StartCoroutine(shaderController.SetGrayscale(0f, 0.5f));
        }
        // TODO add halo on Tank to show invincibility
        if (isServer) {
            isDead = false;
            isInvincible = true;
            shootCooldown = 1.5f;
            life = 4f;
            yield return new WaitForSeconds(1.5f);
            isInvincible = false;
        }
    }
}
