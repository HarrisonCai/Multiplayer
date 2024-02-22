using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Health : NetworkBehaviour
{
    [SerializeField] private float maxhp;
    private NetworkVariable<float> hp=new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    void Start()
    {
        hp.Value = maxhp;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
