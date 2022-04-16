using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody body;

    [SyncVar]
    private float forwardMovement;

    [SyncVar]
    private float rotationMovement;

    [Command]
    private void CmdChangeTransformProps(float _forwardMovement, float _rotationMovement) 
    {
        forwardMovement = _forwardMovement;
        rotationMovement = _rotationMovement;
    }

    private void Update()
    {
        if (isLocalPlayer) {
            rotationMovement = Input.GetAxisRaw("Horizontal");
            forwardMovement = Input.GetAxisRaw("Vertical");

            CmdChangeTransformProps(forwardMovement, rotationMovement);
        }
    }

    private void FixedUpdate()
    {
        if (!isServer) return;   
        Vector3 updatedAngles = new Vector3(
            transform.eulerAngles.x, 
            transform.eulerAngles.y, 
            transform.eulerAngles.z
        );

        updatedAngles.y += rotationMovement * 60 * Time.deltaTime;

        transform.eulerAngles = updatedAngles;

        Vector3 newVelocity = transform.forward * forwardMovement * 2;
        newVelocity.y -= 9.81f * Time.deltaTime;
                
        body.velocity = newVelocity;
    }
}
