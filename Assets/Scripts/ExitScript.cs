using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class ExitScript : MonoBehaviour
{

    
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
            SceneManager.LoadScene("Level1");
        }

    }
}
