using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private float shootCooldown = 0f;
    private int id = 4;


    void Update()
    {
        rotationMovement = Input.GetAxisRaw("Horizontal");
        forwardMovement = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.P) && shootCooldown == 0) {
            ShootMissile();
        }
        if (shootCooldown != 0) {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
        }
    }

    void FixedUpdate()
    {
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
        shootCooldown = 4f;
    }
}
