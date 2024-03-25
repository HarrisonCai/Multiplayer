using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
public class StorageHouse : NetworkBehaviour
{
    [SerializeField] PlayerStorage stor;
    [SerializeField] private ulong clientID;
    private Collider2D player;
    private ToolsItems tools;
    private NetworkVariable<int> corn = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private TextMeshProUGUI cornText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(corn.Value >= 750&& !stor.GameOver && OwnerClientId==clientID)
        {
            
            stor.Win(PlayerPrefs.GetString("Name"));
        }
        cornText.text = "" + corn.Value;
        if (!IsServer) { return; }
        
        if (player != null)
        {
            tools = player.gameObject.GetComponent<ToolsItems>();
            
            if (tools.Stored && tools.StoragHouse)
            {
                tools.ChangeStoredStateServerRpc(false);
                ChangeCornValServerRpc(tools.Corn);
                tools.ResetCornClientRpc();
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!IsServer) { return; }
        if (collision.gameObject.CompareTag("Player")&& collision.gameObject.GetComponent<NetworkObject>().OwnerClientId==clientID)
        {
            
            player = collision;
            collision.gameObject.GetComponent<ToolsItems>().StoragHouse=true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsServer) { return; }
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId == clientID)
        {
            player = collision;
            collision.gameObject.GetComponent<ToolsItems>().StoragHouse = false;
        }
    }
    [ServerRpc(RequireOwnership =false)]
    public void ChangeCornValServerRpc(int val)
    {
        corn.Value += val;
    }
    public ulong ClientID
    {
        get { return clientID; }
    }
    public int Corn
    {
        get { return corn.Value; }
    }
}
