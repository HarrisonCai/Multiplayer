using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine.UIElements;
public class CornadeActive : NetworkBehaviour
{
    [SerializeField] private int fragCount;
    [SerializeField] private float timer;
    [SerializeField] private GameObject explosion, shrapnel, player;
    [SerializeField] private float accel;
    
    private float velocity;
    private NetworkObject networkObjPlaceholder;
    private bool no = false;
    private float angle,rot;
    private Vector2 distance;
    private NetworkVariable<bool> launched = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerStorageData").GetComponent<PlayerStorage>().playerIdentify(OwnerClientId).gameObject;
        if (!IsOwner) return;
        Invoke(nameof(TimedDestroy), timer);
        //something
    }
    private void Update()
    {
        
        if (!IsOwner) return;
        
        if (!launched.Value)
        {
            transform.position = player.transform.position;
        }
        else if (launched.Value && !no)
        {
            no = true;
            velocity = Mathf.Sqrt(accel * distance.magnitude * 2);
            angle= Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg + 180;
            
            
        }
        if (velocity < 0)
        {
            velocity = 0;
        }
        if (launched.Value && velocity >= 0) {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity * Mathf.Cos(angle*Mathf.Deg2Rad), velocity * Mathf.Sin(angle*Mathf.Deg2Rad));
            velocity -= accel*Time.deltaTime;
            rot += velocity * 1.5f;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }
    private void TimedDestroy()
    {
        ExplodeServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ExplodeServerRpc()
    {
        
        networkObjPlaceholder = Instantiate(explosion, transform.position, this.transform.rotation).GetComponent<NetworkObject>();
        networkObjPlaceholder.SpawnWithOwnership(OwnerClientId);
        for (int i = 0; i < fragCount; i++)
        {
            networkObjPlaceholder = Instantiate(shrapnel, transform.position, Quaternion.Euler(0,0,Random.Range(0,360))).GetComponent<NetworkObject>();
            networkObjPlaceholder.SpawnWithOwnership(OwnerClientId);
            networkObjPlaceholder.gameObject.GetComponent<Rigidbody2D>().velocity += Random.Range(0,1)*this.gameObject.GetComponent<Rigidbody2D>().velocity*100;
        }
        this.NetworkObject.Despawn();
    }
    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }
    [ServerRpc(RequireOwnership =false)]
    public void LaunchedServerRpc(bool val)
    {
        launched.Value = val;
    }
    public bool Launched
    {
        get { return launched.Value; }
    }
    public Vector2 Dist
    {
        get { return distance; }
        set { distance = value;}
    }
    [ClientRpc(RequireOwnership =false)]
    public void setDistanceClientRpc(float x, float y)
    {
        distance = new Vector2(x, y);
    }
}
