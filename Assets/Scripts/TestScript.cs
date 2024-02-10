using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TestScript : NetworkBehaviour
{
    public GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.Find("Image");
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        Debug.Log("clienet");
        if (Input.GetKeyDown(KeyCode.F))
        {
            image.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            image.SetActive(false); 
        }
    }
}
