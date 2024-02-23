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
    private NetworkVariable<float> timer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private GameObject cornIMG, goldenCornIMG;
    private NetworkVariable<ulong> clientId = new NetworkVariable<ulong>(6000, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    [ServerRpc(RequireOwnership = false)]
    public void SetClientServerRpc(ulong num)
    {
        clientId.Value = num;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (corn.Value)
        {
            cornIMG.SetActive(true);
        }
        if (goldenCorn.Value)
        {
            goldenCornIMG.SetActive(true);
        }
    }
    [ServerRpc(RequireOwnership =false)]
    public void CornServerRpc()
    {
        CornClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    public void CornClientRpc()
    {
        cornIMG.SetActive(true);
        isPlanted.Value = true;
        corn.Value = true;
    }
    [ServerRpc(RequireOwnership = false)]
    public void GoldenCornServerRpc()
    {
        GoldenCornClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    public void GoldenCornClientRpc()
    {
        cornIMG.SetActive(true);
        isPlanted.Value = true;
        goldenCorn.Value = true;
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
}
