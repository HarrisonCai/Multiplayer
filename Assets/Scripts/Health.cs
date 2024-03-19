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
    void Start()
    {
        hp.Value = maxhp;
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
}
