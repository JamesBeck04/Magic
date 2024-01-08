using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalExit : MonoBehaviour
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
            SceneManager.LoadScene("TitleScnee");
        }

    }
}
