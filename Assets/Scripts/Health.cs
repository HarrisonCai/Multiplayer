using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
public class Health : NetworkBehaviour
{
    [SerializeField] private GameObject deathTimer;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private float maxhp;
    private NetworkVariable<float> hp=new NetworkVariable<float>(1f,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private bool IsTurret;
    private NetworkVariable<bool> dead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    [SerializeField] private float respawnTimer;
    private float timer;
    [SerializeField] private ToolsItems cornGold;
    private NetworkVariable<ulong> lastHitPlayer= new NetworkVariable<ulong>(666,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    private PlayerStorage stor;

    [SerializeField] private Vector3 spawnPoint0, spawnPoint1, spawnPoint2, spawnPoint3, voidZone;
    
    void Start()
    {
        
        SetHPFullServerRpc();
        if (!IsTurret)
        {
            stor = GameObject.Find("PlayerStorageData").GetComponent<PlayerStorage>();
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
    
    // Update is called once per frame
    void Update()
    {
        
        
        
        
        if(IsTurret && hp.Value <= 0)
        {
            Destroy(this.gameObject);
        }
        
        if (!IsOwner||IsTurret) { return; }
        timer -= Time.deltaTime;
        
        if (dead.Value)
        {
            deathTimer.SetActive(true);
            deathText.text = timer.ToString("#.0");
        }
        else
        {
            deathTimer.SetActive(false);
        }
        
        if (hp.Value > maxhp)
        {
            SetHPFullServerRpc();
        }
        if (!IsTurret && hp.Value <= 0 && !dead.Value)
        {
            SetDeadServerRpc(true);
            timer = respawnTimer;
            cornGold.CornPot = false;
            if (lastHitPlayer.Value != 666)
            {
                Debug.Log("sent id");
                Debug.Log("Corn/Gold : " + cornGold.Corn + "/" + cornGold.Gold);
                stor.CalculateDeath(OwnerClientId, lastHitPlayer.Value, cornGold.Corn, cornGold.Gold);
            }
            cornGold.Corn = 0;
            cornGold.Gold = 0;
        }
        if(dead.Value && hp.Value>0)
        {
            timer = 0.1f;
        }
        if ((dead.Value && timer <= 0))
        {

            SetHPFullServerRpc();
            SetDeadServerRpc(false);
            Debug.Log("Failed");
            Debug.Log(dead.Value);
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
        get { return dead.Value; }
    }
    [ServerRpc(RequireOwnership =false)]
    public void SetDeadServerRpc(bool ded)
    {
        dead.Value = ded;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetFinalHitServerRpc(ulong playerVal)
    {
        lastHitPlayer.Value = playerVal;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetHPFullServerRpc()
    {
        hp.Value = maxhp;
    }
}
