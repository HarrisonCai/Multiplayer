using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerStorage : NetworkBehaviour
{
    [SerializeField] private GameObject textObj;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField]private ToolsItems player0, player1, player2, player3;
    private NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
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
    public void CalculateDeath(ulong origin, ulong playerVal, int corn, int gold)
    {
        if (origin == playerVal)
        {
            return;
        }
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
    public void Win(string name)
    {
        setGameOverServerRpc(true);
        textObj.SetActive(true);
        text.text = name + " won!";
        Invoke(nameof(DisconnectClientRpc), 8);
    }
    [ClientRpc(RequireOwnership =false)]
    private void DisconnectClientRpc()
    {
        Disconnect();
        Cleanup();
        SceneManager.LoadScene(0);
    }
    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }
    public void Cleanup()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void setGameOverServerRpc(bool val)
    {
        gameOver.Value = val;
    }
    public bool GameOver
    {
        get { return gameOver.Value; }
    }
}
