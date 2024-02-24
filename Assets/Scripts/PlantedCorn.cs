using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlantedCorn : NetworkBehaviour
{
    private NetworkVariable<bool> isPlanted = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> corn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> goldenCorn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private float cornTimeToGrow;
    [SerializeField] private float goldenCornTimeToGrow;
    private NetworkVariable<float> timer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private GameObject cornStage1, cornStage2, cornStage3, goldenCornStage1, goldenCornStage2, goldenCornStage3;
    private NetworkVariable<ulong> clientId = new NetworkVariable<ulong>(6000, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<bool> doneGrowing = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
   
    [ServerRpc(RequireOwnership = false)]
    public void SetClientServerRpc(ulong num)
    {
        clientId.Value = num;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlanted)
        {
            cornStage1.SetActive(false);
            cornStage2.SetActive(false);
            cornStage3.SetActive(false);
            goldenCornStage1.SetActive(false);
            goldenCornStage2.SetActive(false);
            goldenCornStage3.SetActive(false);
        }
        TimerServerRpc();
        if (corn.Value)
        {
            if (timer.Value<=5 && timer.Value > 0)
            {
                cornStage1.SetActive(false);
                cornStage2.SetActive(true);
            }
            if (timer.Value <= 0)
            {
                cornStage2.SetActive(false);
                cornStage3.SetActive(true);
                GrowStateServerRpc(true);
            }
        }
        if (goldenCorn.Value)
        {
            if (timer.Value <= 5 && timer.Value > 0)
            {
                goldenCornStage1.SetActive(false);
                goldenCornStage2.SetActive(true);
            }
            if (timer.Value <= 0 )
            {
                goldenCornStage2.SetActive(false);
                goldenCornStage3.SetActive(true);
                GrowStateServerRpc(true);
            }
        }
    }
    [ServerRpc(RequireOwnership =false)]
    public void GrowStateServerRpc(bool state)
    {
        doneGrowing.Value = state;
        

    }
    [ServerRpc(RequireOwnership =false)]
    public void TimerServerRpc()
    {
        timer.Value -= Time.deltaTime;
    }
    [ServerRpc(RequireOwnership =false)]
    public void CornServerRpc()
    {
        CornClientRpc();
        timer.Value = cornTimeToGrow;
    }
    [ClientRpc(RequireOwnership = false)]
    public void CornClientRpc()
    {
        cornStage1.SetActive(true);
        isPlanted.Value = true;
        corn.Value = true;
    }
    [ServerRpc(RequireOwnership = false)]
    public void GoldenCornServerRpc()
    {
        GoldenCornClientRpc();
        timer.Value = goldenCornTimeToGrow;
    }
    [ClientRpc(RequireOwnership = false)]
    public void GoldenCornClientRpc()
    {
        goldenCornStage1.SetActive(true);
        isPlanted.Value = true;
        goldenCorn.Value = true;
    }
    [ServerRpc(RequireOwnership =false)]
    public void DisableGrowingServerRpc()
    {
        DisableGrowingClientRpc();
    }
    [ClientRpc(RequireOwnership =false)]
    public void DisableGrowingClientRpc()
    {
        isPlanted.Value = false;
        goldenCorn.Value = false;
        corn.Value = false;
        cornStage3.SetActive(false);
        goldenCornStage3.SetActive(false);
    }
    public bool IsPlanted
    {
        get { return isPlanted.Value; }
        
    }
    public bool Corn
    {
        get { return corn.Value; }
        
    }
    public bool GoldenCorn
    {
        get { return goldenCorn.Value; }
        
    }
    public ulong ClientID
    {
        get { return clientId.Value; }
    }
    public bool Grow
    {
        get { return doneGrowing.Value; }
    }
}
