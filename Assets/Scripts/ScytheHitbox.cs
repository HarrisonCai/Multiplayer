using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ScytheHitbox : NetworkBehaviour
{
    // Start is called before the first frame update
    private Collider2D player;
    [SerializeField] ToolsItems scytheState;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!IsOwner || !scytheState.Scythe || !scytheState.Swinging) { return; }
        
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        {
            

            player = collision;

            player.gameObject.GetComponent<Health>().ChangeHPServerRpc(20);
            scytheState.Swinging = false;
        }

    }
}
