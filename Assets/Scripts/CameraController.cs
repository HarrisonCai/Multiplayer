using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
public class CameraController : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private GameObject listener;
    [SerializeField] private Health hp;
    [SerializeField] private Vector3 deathPosition;
    [SerializeField] private GameObject cameraHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.SetActive(true);
            
                vc.Priority = 1;
            
            
        }
        else
        {
            vc.Priority = 0;
            listener.SetActive(false);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
