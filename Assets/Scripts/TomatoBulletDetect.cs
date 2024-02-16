using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TomatoBulletDetect : NetworkBehaviour
{
    private ulong clientID;
    private Collider2D player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!IsServer) { return; }
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != clientID)
        {
            Debug.Log("hit");
            
            player = collision;
            //DamageServerRpc();
            player.gameObject.GetComponent<Health>().HP -= 20;
            Destroy(this.gameObject);
        }
        
    }
    [ServerRpc(RequireOwnership =false)]
    private void DamageServerRpc()
    {
        DamageClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void DamageClientRpc()
    {
        player.gameObject.GetComponent<Health>().HP -= 20;
        Destroy(player.gameObject);
    }
    public ulong ClientID
    {
        set { clientID = value; }
    }
}
