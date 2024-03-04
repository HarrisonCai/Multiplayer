using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class StorageHouse : NetworkBehaviour
{
    [SerializeField] private ulong clientID;
    private Collider2D player;
    private NetworkVariable<float> corn = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
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
        if (collision.gameObject.CompareTag("Player")&& collision.gameObject.GetComponent<NetworkObject>().OwnerClientId==clientID)
        {
            player = collision;
            collision.gameObject.GetComponent<ToolsItems>().StoragHouse=true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId == clientID)
        {
            player = collision;
            collision.gameObject.GetComponent<ToolsItems>().StoragHouse = false;
        }
    }
}
