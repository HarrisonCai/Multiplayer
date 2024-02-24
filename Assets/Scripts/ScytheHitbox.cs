using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ScytheHitbox : NetworkBehaviour
{
    // Start is called before the first frame update
    private Collider2D player;
    [SerializeField] ToolsItems scytheState;
    private bool done = false;
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
        
        
        if (collision.gameObject.CompareTag("PlantingUnit") && collision.gameObject.GetComponent<PlantedCorn>().ClientID==OwnerClientId && collision.gameObject.GetComponent<PlantedCorn>().Grow && scytheState.Corn<scytheState.MaxCorn)
        {
            PlantedCorn Unit= collision.gameObject.GetComponent<PlantedCorn>();
            Unit.GrowStateServerRpc(false);
            
            if (Unit.Corn && !done)
            {
                scytheState.Corn+=3;
                scytheState.Seeds++;
                done = true;
                Invoke(nameof(ResetCorn), 0.3f);
            }
            if (Unit.GoldenCorn && !done)
            {
                scytheState.Corn += 15;
                scytheState.GoldenSeeds++;
                done = true;
                Invoke(nameof(ResetCorn), 0.3f);
            }
            
            Unit.DisableGrowingServerRpc();
        }
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        {


            player = collision;

            player.gameObject.GetComponent<Health>().ChangeHPServerRpc(20);
            scytheState.Swinging = false;
        }
    }
    private void ResetCorn()
    {
        done = false;
    }
}
