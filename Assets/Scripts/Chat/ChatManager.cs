using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Singleton;

    [SerializeField] ChatMessage chatMessagePrefab;
    [SerializeField] CanvasGroup chatContent;
    [SerializeField] TMP_InputField chatInput;
    [SerializeField] Scrollbar scrollBar;

    public string playerName;

    [Header("Debug")]
    public bool debugScrollBar;

    void Awake()
    { ChatManager.Singleton = this; }

    private void Start()
    {
        if (debugScrollBar)
        {
            AddMessage("a");
            AddMessage("a");
            AddMessage("a");
            AddMessage("a");
            AddMessage("a");
            AddMessage("a");
            AddMessage("a");
            AddMessage("a");
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInput.text, playerName);
            chatInput.text = "";
        }
    }

    public void SendChatMessage(string _message, string _fromWho = null)
    {
        if (string.IsNullOrWhiteSpace(_message)) return;

        string S = _fromWho + " > " + _message;
        SendChatMessageServerRpc(S);
    }

    void AddMessage(string msg)
    {
        ChatMessage CM = Instantiate(chatMessagePrefab, chatContent.transform);

        CM.SetText(msg);
        StartCoroutine(ResetScroll());
    }

    [ServerRpc(RequireOwnership = false)]
    void SendChatMessageServerRpc(string message)
    {
        ReceiveChatMessageClientRpc(message);
    }

    [ClientRpc]
    void ReceiveChatMessageClientRpc(string message)
    {
        ChatManager.Singleton.AddMessage(message);
    }

    /*
     * https://discussions.unity.com/t/how-to-adjust-rect-height-according-to-amount-of-text-in-a-textmeshprougui/206406/2
     */
    IEnumerator ResetScroll()
    {
        yield return new WaitForSeconds(0.1f);
        scrollBar.value = 0f;
    }
}
