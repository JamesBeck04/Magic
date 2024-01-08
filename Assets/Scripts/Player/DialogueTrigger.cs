using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
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
        if (collisionInfo.tag == "Player")
        {
            Debug.Log("Contact");
            target.SetActive(true);
        }
       
    }

   
}
