using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerStorage : NetworkBehaviour
{
    public ToolsItems player0, player1, player2, player3;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player0==null && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId == 0)
            {
                player0 = collision.gameObject.GetComponent<ToolsItems>();
            }
            if (player1 == null && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId == 1)
            {
                player1 = collision.gameObject.GetComponent<ToolsItems>();
            }
            if (player2 == null && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId == 2)
            {
                player2 = collision.gameObject.GetComponent<ToolsItems>();
            }
            if (player3 == null && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId == 3)
            {
                player3 = collision.gameObject.GetComponent<ToolsItems>();
            }
        }
    }
    public void CalculateDeath(ulong playerVal, int corn, int gold)
    {
        Debug.Log("kill received");
        if (playerVal == 0)
        {
            Debug.Log("player 0 sent corn and gold");
            player0.CornGoldAddServerRpc(corn, gold);
        }
        if (playerVal == 1)
        {
            player1.CornGoldAddServerRpc(corn, gold);
        }
        if (playerVal == 2)
        {
            player2.CornGoldAddServerRpc(corn, gold);
        }
        if (playerVal == 3)
        {
            player3.CornGoldAddServerRpc(corn, gold);
        }
    }
}
