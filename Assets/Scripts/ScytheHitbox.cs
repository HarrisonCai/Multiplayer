using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ScytheHitbox : NetworkBehaviour
{
    // Start is called before the first frame update
    private Collider2D player;
    private List<Collider2D> Objs = new List<Collider2D>();
    [SerializeField] private ToolsItems scytheState;
    [SerializeField] private Health self;

    // Update is called once per frame
    void Update()
    {
        if (Objs.Count!=0 && !scytheState.Swinging)
        {
            Objs.Clear();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!IsOwner || !scytheState.Scythe || !scytheState.Swinging) { return; }
        
        
        if (!Objs.Contains(collision) && collision.gameObject.CompareTag("PlantingUnit") && collision.gameObject.GetComponent<PlantedCorn>().ClientID==OwnerClientId && collision.gameObject.GetComponent<PlantedCorn>().Grow && scytheState.Corn<scytheState.MaxCorn)
        {
            Objs.Add(collision);
            PlantedCorn Unit= collision.gameObject.GetComponent<PlantedCorn>();
            Unit.GrowStateServerRpc(false);
            
            if (Unit.Corn )
            {
                scytheState.Corn+=3;
                scytheState.Seeds++;
                
                
            }
            if (Unit.GoldenCorn)
            {
                scytheState.Corn += 15;
                scytheState.GoldenSeeds++;
                float randomNum = Random.Range(0, 1);
                if (randomNum < 0.05)
                {
                    scytheState.Gold++;
                }
                
            }
            
            Unit.DisableGrowingServerRpc();
        }
        if (!Objs.Contains(collision) && collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        {
            Objs.Add(collision);

            player = collision;

            player.gameObject.GetComponent<Health>().ChangeHPServerRpc(scytheState.Damage);
            
            player.gameObject.GetComponent<Health>().SetFinalHitServerRpc(OwnerClientId);
            Debug.Log(OwnerClientId);
        }
        if(!Objs.Contains(collision) && collision.gameObject.CompareTag("Storage") && collision.gameObject.GetComponent<StorageHouse>().ClientID != OwnerClientId)
        {
            Objs.Add(collision);
            collision.gameObject.GetComponent<StorageHouse>().ChangeCornValServerRpc(-3);
        }
    }
    
}
