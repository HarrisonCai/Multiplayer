using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class SharpnelHitbox : NetworkBehaviour
{
 
    [SerializeField] private float rotspeed;
    private float rot = 0;
    private Collider2D player;
    [SerializeField] private float damage, bullspeed;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity += (Vector2)transform.right * bullspeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        rot += rotspeed;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!IsServer) { return; }
        Debug.Log("run");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit");

            player = collision;

            player.gameObject.GetComponent<Health>().ChangeHPServerRpc(damage);
            player.gameObject.GetComponent<Health>().SetFinalHitServerRpc(OwnerClientId);
            Destroy(this.gameObject);
        }

    }
}
