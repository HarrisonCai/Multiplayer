using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Health : NetworkBehaviour
{
    [SerializeField] private float maxhp;
    private NetworkVariable<float> hp=new NetworkVariable<float>(1f,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private bool IsTurret;
    private bool dead = false;
    [SerializeField] private float respawnTimer;
    private float timer;
    [SerializeField] private ToolsItems cornGold;
    private NetworkVariable<ulong> lastHitPlayer= new NetworkVariable<ulong>(666,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    private PlayerStorage stor;

    [SerializeField] private Vector3 spawnPoint0, spawnPoint1, spawnPoint2, spawnPoint3, voidZone;
    void Start()
    {
        stor = GameObject.Find("PlayerStorageData").GetComponent<PlayerStorage>();
        ChangeHPServerRpc(1 - maxhp);
        if (OwnerClientId == 0)
        {
            transform.position = spawnPoint0;
        }
        if (OwnerClientId == 1)
        {
            transform.position = spawnPoint1;
        }
        if (OwnerClientId == 2)
        {
            transform.position = spawnPoint2;
        }
        if (OwnerClientId == 3)
        {
            transform.position = spawnPoint3;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
        
        if(hp.Value > 100)
        {
            ChangeHPServerRpc(hp.Value - 100);
        }
        
        if(IsTurret && hp.Value <= 0)
        {
            Destroy(this.gameObject);
        }
        Debug.Log(lastHitPlayer.Value);
        if (!IsOwner) { return; }
        timer -= Time.deltaTime;
        //krill your shelf button
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeHPServerRpc(10);
        }
        if (!IsTurret && hp.Value <= 0 && !dead)
        {
            dead = true;
            timer = respawnTimer;
            
            if (lastHitPlayer.Value != 666)
            {
                Debug.Log("sent id");
                Debug.Log("Corn/Gold : " + cornGold.Corn + "/" + cornGold.Gold);
                stor.CalculateDeath(lastHitPlayer.Value, cornGold.Corn, cornGold.Gold);
            }
            cornGold.Corn = 0;
            cornGold.Gold = 0;
        }
        if (dead && timer <= 0)
        {
            dead = false;
            ChangeHPServerRpc(-maxhp);
            SetFinalHitServerRpc(666);
            if (OwnerClientId == 0)
            {
                transform.position = spawnPoint0;
            }
            if (OwnerClientId == 1)
            {
                transform.position = spawnPoint1;
            }
            if (OwnerClientId == 2)
            {
                transform.position = spawnPoint2;
            }
            if (OwnerClientId == 3)
            {
                transform.position = spawnPoint3;
            }
        }
        
    }
    [ServerRpc(RequireOwnership =false)]
    public void ChangeHPServerRpc(float val)
    {
        hp.Value -= val;
    }
    
    public float HP
    {
        get { return hp.Value; }
        
    }
    public float MaxHP
    {
        get { return maxhp; }
    }
    public bool Dead
    {
        get { return dead; }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetFinalHitServerRpc(ulong playerVal)
    {
        lastHitPlayer.Value = playerVal;
    }

}
