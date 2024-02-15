using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TomatoGunShoot : NetworkBehaviour
{
    [SerializeField] private GameObject tomatoPrefab;
    [SerializeField] private float cooldown;
    private float reload;
    // Start is called before the first frame update
    void Start()
    {
        reload = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) { return; }
        reload -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && reload<=0)
        {
            FireTomatoServerRpc();
            reload = cooldown;
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void FireTomatoServerRpc()
    {
        FireTomatoClientRpc();
    }
    [ClientRpc(RequireOwnership =false)]
    private void FireTomatoClientRpc()
    {
        GameObject tomato = Instantiate(tomatoPrefab, transform.position+transform.right,this.transform.rotation);
        tomato.GetComponent<Rigidbody2D>().velocity = transform.right;
        NetworkObject instanceNetworkObject = tomato.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }

}
