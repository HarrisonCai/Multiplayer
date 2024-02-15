using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RotateHitbox : NetworkBehaviour
{
    [SerializeField]
    Camera cam;

    private Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotate = transform.position - mousePos;
            float rot = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot+180);
        }
    }
}
