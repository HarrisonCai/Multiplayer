using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Health : NetworkBehaviour
{
    [SerializeField] private float maxhp;
    private NetworkVariable<float> hp=new NetworkVariable<float>(0.1f,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private bool IsTurret;
    private bool dead = false;
    [SerializeField] private float respawnTimer;
    private float timer;
    [SerializeField] private ToolsItems cornGold;
    private Health lastHitPlayer;
    private int killCorn, killGold;

    [SerializeField] private Vector3 spawnPoint0, spawnPoint1, spawnPoint2, spawnPoint3, voidZone;
    void Start()
    {
        hp.Value = maxhp;
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
        
        timer -= Time.deltaTime;
        if(hp.Value > 100)
        {
            ChangeHPServerRpc(hp.Value - 100);
        }
        
        if(IsTurret && hp.Value <= 0)
        {
            Destroy(this.gameObject);
        }
        if (!IsOwner) { return; }
        if (killGold != 0)
        {
            cornGold.Gold += killGold;
        }
        if (killCorn != 0)
        {
            cornGold.Corn += killCorn;
        }
        //krill your shelf button
        if (Input.GetKeyDown(KeyCode.H))
        {
            hp.Value -= 10;
        }
        if (!IsTurret && hp.Value <= 0 && !dead)
        {
            dead = true;
            timer = respawnTimer;
            transform.position = voidZone;
            if (lastHitPlayer != null)
            {
                lastHitPlayer.KillGold = cornGold.Gold;
                lastHitPlayer.KillCorn = cornGold.Corn;
            }
            cornGold.Corn = 0;
            cornGold.Gold = 0;
        }
        if (dead && timer <= 0)
        {
            dead = false;
            hp.Value = maxhp;
            
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
        ChangeHPClientRpc(val);
    }
    [ClientRpc(RequireOwnership =false)]
    public void ChangeHPClientRpc(float val)
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
    public Health Player
    {
        get { return lastHitPlayer; }
        set { lastHitPlayer = value; }
    }
    public int KillGold
    {
        get { return killGold; }
        set { killGold = value; }
    }
    public int KillCorn
    {
        get { return killCorn; }
        set { killCorn=value; }
    }
    
}
