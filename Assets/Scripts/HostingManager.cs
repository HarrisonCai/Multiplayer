using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class HostingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (HostStore.isHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
    }

    
}
