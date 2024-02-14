using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RotateHitbox : NetworkBehaviour
{
    [SerializeField]
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            transform.right = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }
    }
}
