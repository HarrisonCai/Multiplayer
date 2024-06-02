using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class BearTrap : NetworkBehaviour
{
    private NetworkVariable<bool> triggered = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private Collider2D player;
    [SerializeField] private float damage;
    [SerializeField] private GameObject active, nonactive;
    [SerializeField] private float timer;
    private float duration = 10;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
        if (!IsServer) { return; }
        timer-=Time.deltaTime;
        if (timer <= 0)
        {
            hideBearClientRpc();
        }
        if (triggered.Value)
        {
            activateAnimationClientRpc();
            duration -= Time.deltaTime;
        }
        if (duration <= 0)
        {
            DestroyServerRpc();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!IsServer) { return; }
        Debug.Log("run");
        if (!triggered.Value && collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        {
            Debug.Log("hit");
            player = collision;

            player.gameObject.GetComponent<Health>().ChangeHPServerRpc(damage);
            player.gameObject.GetComponent<Health>().SetFinalHitServerRpc(OwnerClientId);
            player.gameObject.GetComponent<MovementController>().trapStateServerRpc(true);
            triggeredServerRpc();
        }

    }
    [ServerRpc(RequireOwnership =false)]
    private void triggeredServerRpc()
    {
        triggered.Value = true;
    }
    [ServerRpc(RequireOwnership = false)]
    private void DestroyServerRpc()
    {
        this.NetworkObject.Despawn();
    }
    [ClientRpc(RequireOwnership = false)]
    private void hideBearClientRpc()
    {
        if (!IsOwner)
        {
            active.SetActive(false);
        }
    }
    [ClientRpc(RequireOwnership =false)]
    private void activateAnimationClientRpc()
    {
        active.SetActive(false);
        nonactive.SetActive(true);
    }
}
