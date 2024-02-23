using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class DestroyAtk : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(TimedDestroy), 2);
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
