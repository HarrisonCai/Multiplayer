using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TomatoGunShoot : NetworkBehaviour
{
    [SerializeField] private GameObject tomatoPrefab;
    [SerializeField] private float cooldown;
    private float reload;
    [SerializeField] private Health hp;
    [SerializeField] private ToolsItems gunState;
    // Start is called before the first frame update
    void Start()
    {
        reload = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner || !gunState.TomatoGun || gunState.ShoppingState || hp.Dead) { return; }
        reload -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && reload<=0 &&gunState.Tomato>0)
        {
            //FireTomatoServerRpc();


            gunState.Tomato--;
            FireTomatoServerRpc();
            reload = cooldown;
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void FireTomatoServerRpc()
    {
        NetworkObject instanceNetworkObject = Instantiate(tomatoPrefab, transform.position + transform.right*0.2f, this.transform.rotation).GetComponent<NetworkObject>();
        instanceNetworkObject.SpawnWithOwnership(OwnerClientId);
        
    }
    


}
