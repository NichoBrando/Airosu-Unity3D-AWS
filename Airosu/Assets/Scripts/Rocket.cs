using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private uint ownerId = 0;

    [SerializeField]
    private Rigidbody body;

    private Vector3 forwardPosition;

    private Vector3 eulerAngles;

    private void Awake()
    {
        StartCoroutine(DestroyRocket());
    }

    public void SetOwner(uint id)
    {
        if (ownerId != 0) return; 
        ownerId = id;
    }
    
    public void SetMovementForward(Vector3 _forwardPosition)
    {
        forwardPosition = _forwardPosition * 20;
        body.velocity = forwardPosition * 20;
    }

    public void SetTransform(Vector3 cannonPosition, Vector3 tankAngles)
    {
        transform.position = cannonPosition;
        Vector3 newOrientation = new Vector3(tankAngles.x, tankAngles.y + 180, tankAngles.z);
        eulerAngles = newOrientation;
        transform.eulerAngles = newOrientation;
    }

    private void OnTriggerEnter (Collider obj)
    {
        if (ownerId == 0) return;
        if (obj.gameObject.tag == "Player") {
            Player affectedPlayer = obj.gameObject.GetComponent<Player>();
            if (affectedPlayer != null && affectedPlayer.ID != ownerId) {
                obj.gameObject.GetComponent<PlayerLife>().ReceiveDamage();
            }
            else {
                return;
            }
        }
        Destroy(gameObject);
    }

    private IEnumerator DestroyRocket()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject); 
    }

    private void FixedUpdate()
    {
        body.velocity = forwardPosition;
        transform.eulerAngles = eulerAngles;
    }
}
