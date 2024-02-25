using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TurretFiring : NetworkBehaviour
{
    [SerializeField] private GameObject turretHead;
    private List<Collider2D> players = new List<Collider2D>();
    private ToolsItems owner;
    private float timer;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            players.Clear();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player") && !players.Contains(collision) && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        {
            timer = 0.5f;
            players.Add(collision);
        }
        Debug.Log(players.Count);
    }
    private void TurretFire()
    {

    }
    public ToolsItems Owner
    {
        get { return owner; }
        set { owner = value; }
    }
}
