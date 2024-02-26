using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TurretFiring : NetworkBehaviour
{
    [SerializeField] private GameObject turretHead, arrowPrefab;
    private List<Collider2D> players = new List<Collider2D>();
    private ToolsItems owner;
    private float timer,fireTimer;
    [SerializeField] private float resetFireTimer;
    private float minDistanceIndex, minDistance;
    private void Start()
    {
        fireTimer = 2f;
    }
    private void Update()
    {
        fireTimer -= Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            players.Clear();
        }
        if (players.Count>0 && fireTimer <= 0)
        {
            fireTimer = resetFireTimer;
            TurretFire();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player") && !players.Contains(collision) && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        {
            timer = 0.5f;
            players.Add(collision);
        }
        
    }
    private void TurretFire()
    {
        minDistance = Vector2.Distance(turretHead.transform.position, players[0].transform.position);
        minDistanceIndex = 0;
        for(int i = 1; i < players.Count; i++)
        {
            if(Vector2.Distance(turretHead.transform.position, players[i].transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(turretHead.transform.position, players[i].transform.position);
                minDistanceIndex = i;
            }
        }
        //turretHead.rotate;
    }
    public ToolsItems Owner
    {
        get { return owner; }
        set { owner = value; }
    }
}
