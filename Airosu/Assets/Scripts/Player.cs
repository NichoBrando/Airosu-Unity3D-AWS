using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody body;

    [SerializeField]
    private Transform cannonTransform;

    [SerializeField]
    private Transform tankTransform;

    [SerializeField]
    private GameObject rocketPrefab;

    private float forwardMovement;
    private float rotationMovement;

    private float shootCooldown = 0f;

    [SerializeField]
    private Image shootTimerImage;
    private int id = 4;

    private float life = 4f;

    [SerializeField]
    private Image lifeBarImage;

    private bool isDead = false;

    private bool isInvincible;

    private PostProcess shaderController;

    private void Awake()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        shaderController = camera.gameObject.GetComponent<PostProcess>();
    }

    private void UpdateShootCD()
    {
        if (shootCooldown != 0 && !isDead) {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
            shootTimerImage.fillAmount = 1 - shootCooldown / 1.5f;
        }
        else if(isDead) {
            shootTimerImage.fillAmount = 0;
        }
    }

    private void UpdateLife()
    {
            if (!isDead && life < 4) {
                life += 1 * Time.deltaTime / 10;
            }
            lifeBarImage.fillAmount = life / 4;
            if (life > 4) life = 4f;
    }

    void Update()
    {
        rotationMovement = Input.GetAxisRaw("Horizontal");
        forwardMovement = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.P) && shootCooldown == 0 && !isDead) {
            ShootMissile();
        }
        UpdateLife();
        UpdateShootCD();
    }

    void ReceiveDamage()
    {
        if (isInvincible) return;
        life -= 1;
        if (life <= 0)
        {
            isDead = true;
            StartCoroutine(ResurrectPlayer());
        }
    }

    void FixedUpdate()
    {
        if (isDead) {
            body.velocity = Vector3.zero;
            return;
        }

        Vector3 updatedAngles = new Vector3(
            transform.eulerAngles.x, 
            transform.eulerAngles.y, 
            transform.eulerAngles.z
        );

        updatedAngles.y += rotationMovement * 60 * Time.deltaTime;

        transform.eulerAngles = updatedAngles;

        Vector3 movement = transform.forward * forwardMovement * 2;
        movement.y -= 9.81f * Time.deltaTime;

        body.velocity = movement;
    }

    private void ShootMissile()
    {
        GameObject createdRocket = Instantiate(rocketPrefab);
        Rocket rocketConfigs = createdRocket.GetComponent<Rocket>();
        rocketConfigs.SetTransform(cannonTransform.position, tankTransform.eulerAngles);
        rocketConfigs.SetOwner(id);
        rocketConfigs.SetMovementForward(transform.forward);
        shootCooldown = 1.5f;
        ReceiveDamage();
    }

    private IEnumerator ResurrectPlayer()
    {
        shaderController.grayScale = 1f;
        yield return new WaitForSeconds(3f);

        // TODO add halo on Tank to show invincibility
        isDead = false;
        isInvincible = true;
        shootCooldown = 1.5f;
        life = 4f;
        shaderController.grayScale = 0;
        
        yield return new WaitForSeconds(1.5f);
        isInvincible = false;
    }
}
