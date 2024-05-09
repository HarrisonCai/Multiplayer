using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CornadeActive : NetworkBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private GameObject explosion, shrapnel;
    private NetworkObject networkObjPlaceholder;
    private NetworkVariable<bool> launched = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    // Start is called before the first frame update
    void Start()
    {
        if (!IsServer) return;
        Invoke(nameof(TimedDestroy), timer);
        //something
    }
    private void TimedDestroy()
    {
        ExplodeServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ExplodeServerRpc()
    {
        networkObjPlaceholder = Instantiate(explosion, transform.position, this.transform.rotation).GetComponent<NetworkObject>();
        networkObjPlaceholder.SpawnWithOwnership(OwnerClientId);
        for (int i = 0; i < 30; i++)
        {
            networkObjPlaceholder = Instantiate(shrapnel, transform.position, Quaternion.Euler(0,0,Random.Range(0,360))).GetComponent<NetworkObject>();
            networkObjPlaceholder.SpawnWithOwnership(OwnerClientId);
            networkObjPlaceholder.gameObject.GetComponent<Rigidbody2D>().velocity += this.gameObject.GetComponent<Rigidbody2D>().velocity*1;
        }
        this.NetworkObject.Despawn();
    }
}
