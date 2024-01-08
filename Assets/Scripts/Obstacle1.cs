using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle1 : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "CanPickup")
        {
            Debug.Log("Contact");
            target.SetActive(false);
        }

    }
}
