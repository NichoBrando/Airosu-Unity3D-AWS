using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private int ownerId = 0;

    [SerializeField]
    private Rigidbody body;

    private void Awake()
    {
        StartCoroutine(DestroyRocket());
    }

    public void SetOwner(int id)
    {
        if (ownerId != 0) return; 
        ownerId = id;
    }
    
    public void SetMovementForward(Vector3 forwardPosition)
    {
        body.velocity = forwardPosition * 20;
    }

    public void SetTransform(Vector3 cannonPosition, Vector3 tankAngles)
    {
        transform.position = cannonPosition;
        Vector3 newOrientation = new Vector3(tankAngles.x, tankAngles.y + 180, tankAngles.z);
        transform.eulerAngles = newOrientation;
    }

    // -8.775, -22 3.537
    private void OnTriggerEnter (Collider obj)
    {
        Destroy(gameObject);
    }

    private IEnumerator DestroyRocket()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);   
    }
}
