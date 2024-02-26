using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class DestroyAtk : NetworkBehaviour
{
    [SerializeField] private float timer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(TimedDestroy), timer);
        //something
    }
    private void TimedDestroy()
    {
        DestroyServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void DestroyServerRpc()
    {
        this.NetworkObject.Despawn();
    }
}
