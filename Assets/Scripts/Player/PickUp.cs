using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform player;
    public Transform PlayerCam;
    public float throwForce = 10;
    bool hasPlayer = false;
    bool beingCarried = false;
    private bool touched = false;

    [Header("Keybinds")]
    public KeyCode Use = KeyCode.E;


    private void Update()
    {
        float dist = Vector3.Distance(gameObject.transform.position, player.position);
        if (dist <= 2.5f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }

        if (hasPlayer && Input.GetKey(Use))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = PlayerCam;
            beingCarried = true;
        }
        if (beingCarried)
        {
            if (touched)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                touched = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                GetComponent<Rigidbody>().AddForce(PlayerCam.forward * throwForce);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
            }
        }
    }
    private void OnTriggerEnter()
    {
        if (beingCarried)
        {
            touched = true;
        }
    }

}
