using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TomatoBulletDetect : NetworkBehaviour
{
    private Collider2D player;
    [SerializeField] private float damage, bullspeed;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right*bullspeed;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!IsServer) { return; }
        Debug.Log("run");
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        {
            Debug.Log("hit");
            
            player = collision;
            
            player.gameObject.GetComponent<Health>().ChangeHPServerRpc(damage);
            Destroy(this.gameObject);
        }
        
    }
}
