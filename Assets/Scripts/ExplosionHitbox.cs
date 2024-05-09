using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
public class ExplosionHitbox : NetworkBehaviour
{
    private Collider2D player;
    [SerializeField] private float damage;
    private List<Collider2D> players = new List<Collider2D>();

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!IsServer) { return; }
        Debug.Log("run");
        if (collision.gameObject.CompareTag("Player")&& !players.Contains(collision))
        {
            Debug.Log("hit");
            players.Add(collision);
            player = collision;

            player.gameObject.GetComponent<Health>().ChangeHPServerRpc(damage);
            player.gameObject.GetComponent<Health>().SetFinalHitServerRpc(OwnerClientId);
            
        }

    }
}
