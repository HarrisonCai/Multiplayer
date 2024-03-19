using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
public class CameraController : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;
    [SerializeField] private Health hp;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            if (hp.Dead)
            {
                vc.Priority = 0;
            }
            else
            {
                vc.Priority = 1;
            }
        }
        else
        {
            vc.Priority = 0;
            listener.enabled = false;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
