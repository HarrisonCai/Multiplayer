using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
public class ToolsItems : NetworkBehaviour
{
    private NetworkVariable<int> playerId = new NetworkVariable<int>();
    
    [SerializeField] private RectTransform hotBarIndex;
    private int corn=0;
    private int maxCorn = 20;
    private int gold=0;
    private int seeds=5;
    private int goldSeeds=2;
    private bool sharpener = false;
    private int lighter=0;
    private int turret=0;
    private int tomato = 0;
    private bool cornBag=false;
    private int healthPot=0;
    private bool cornCounter = false;
    private int hotbarLocation = 1;


    private bool planting = false;
    [SerializeField] private float resetPlanting;
    private float plantingTimer;
    private NetworkVariable<bool> plantingState= new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private bool donePlanting;

    private bool mining = false;
    [SerializeField] private float resetMiningTime;
    private float miningTimer;
    private NetworkVariable<bool> miningState = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private Image Progress;
    [SerializeField] private Gradient ProgressGrad;
    private NetworkVariable<float> target= new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    private NetworkVariable<bool> scythe = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> pickaxe = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> shovel = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> tomatoGun = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> turretItem = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> seed = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> goldSeed = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private int typeSeed = 0;

    [SerializeField] GameObject scytheObj, tomatoGunObj;

    private NetworkVariable<bool> swinging = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private bool canSwing = true;
    private float swingTimer, resetswingTimer;
    void Start()
    {
        miningTimer = resetMiningTime;
        Progress.fillAmount = 0;
    }
    

    void Update()
    {
        //Visual Updates
        if (plantingState.Value)
        {
            UpdatePlantProgress();
        }
        if (miningState.Value)
        {
            UpdateGoldProgress();
        }
        if (!(plantingState.Value || miningState.Value))
        {
            Progress.fillAmount = 0;
        }
        if (tomatoGun.Value)
        {
            tomatoGunObj.SetActive(true);
        }
        else
        {
            tomatoGunObj.SetActive(false);
        }
        if (scythe.Value)
        {
            scytheObj.SetActive(true);
        }
        else
        {
            scytheObj.SetActive(false);
        }
        //---------------
        if (!IsOwner)
        {
            
            return;
        }
        
        SwitchTool();
        //Scythe Code
        
        swingTimer -= Time.deltaTime;
        resetswingTimer -= Time.deltaTime;
        if (scythe.Value && canSwing && Input.GetMouseButtonDown(0))
        {
            swinging.Value = true;
            canSwing = false;
            swingTimer = 0.3f;
            resetswingTimer = 1.1f;
        }
        if (swingTimer <= 0)
        {
            swinging.Value = false;
        }
        if (resetswingTimer <= 0)
        {
            canSwing = true;
        }
        //PLANTING CODE

        if (seed.Value && seeds > 0 && planting && Input.GetKey(KeyCode.X))
        {
            plantingTimer -= Time.deltaTime;

            ChangePlantStateServerRpc(true);
        }

        else if (goldSeed.Value && goldSeeds > 0 && planting && Input.GetKey(KeyCode.X))
        {
            plantingTimer -= Time.deltaTime;

            ChangePlantStateServerRpc(true);

        }
        else
        {
            plantingTimer = resetPlanting;

            ChangePlantStateServerRpc(false);
        }
        
        if (plantingTimer <= 0)
        {
            donePlanting = true;
            plantingTimer = resetPlanting;
            
        }

        //PICKAXE CODE


        if (pickaxe.Value && mining && Input.GetMouseButton(0))
        {
            miningTimer -= Time.deltaTime;

            ChangeGoldStateServerRpc(true);
        }
        else
        {
            miningTimer = resetMiningTime;

            ChangeGoldStateServerRpc(false);
        }
        if (miningTimer <= 0)
        {
            gold++;
            miningTimer = resetMiningTime;
        }
        // TOMATO GUN STUFF (THIS ISN'T IN ORDER)

        
    }
    
    
    [ServerRpc(RequireOwnership =false)]
    private void ChangePlantStateServerRpc(bool state)
    {
        ChangePlantStateClientRpc(state);
    }
    [ClientRpc(RequireOwnership = false)]
    private void ChangePlantStateClientRpc(bool state)
    {
        plantingState.Value = state;
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ChangeGoldStateServerRpc(bool state)
    {
        ChangeGoldStateClientRpc(state);
    }
    [ClientRpc(RequireOwnership = false)]
    private void ChangeGoldStateClientRpc(bool state)
    {
        miningState.Value = state;
    }
    
    private void UpdatePlantProgress()
    {

        UpdatePlantProgressServerRpc();
        
        Progress.fillAmount = target.Value;
        Progress.color = ProgressGrad.Evaluate(target.Value);
    }
    [ServerRpc(RequireOwnership =false)]
    private void UpdatePlantProgressServerRpc()
    {
        UpdatePlantProgressClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void UpdatePlantProgressClientRpc()
    {
        target.Value = (resetPlanting - plantingTimer) / resetPlanting;
    }
    
    private void UpdateGoldProgress()
    {
        UpdateGoldProgressServerRpc();
        Progress.fillAmount = target.Value;
        Progress.color = ProgressGrad.Evaluate(target.Value);
    }
    [ServerRpc(RequireOwnership =false)]
    private void UpdateGoldProgressServerRpc()
    {
        UpdateGoldProgressClientRpc();
    }
    [ClientRpc(RequireOwnership =false)]
    private void UpdateGoldProgressClientRpc()
    {
        target.Value = (resetMiningTime - miningTimer) / resetMiningTime;
    }
    private void SwitchTool()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hotbarLocation = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hotbarLocation = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hotbarLocation = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hotbarLocation = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            hotbarLocation = 5;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            typeSeed++;
            typeSeed %= 2;
            plantingTimer = resetPlanting;
        }
        if (typeSeed == 0)
        {
            seed.Value = true;
            goldSeed.Value = false;
        }
        else
        {
            seed.Value = false;
            goldSeed.Value = true;
        }
        float pos = -100 + (hotbarLocation-1 )* 50;

        hotBarIndex.localPosition = new Vector3(pos, hotBarIndex.localPosition.y, hotBarIndex.localPosition.z);
        if (hotbarLocation == 1)
        {
            scythe.Value = true;
            pickaxe.Value = false;
            shovel.Value = false;
            SetTomatoGunServerRpc(false);
            turretItem.Value = false;
        }
        if (hotbarLocation == 2)
        {
            scythe.Value = false;
            pickaxe.Value = true;
            shovel.Value = false;
            SetTomatoGunServerRpc(false);
            turretItem.Value = false;
        }
        if (hotbarLocation == 3)
        {
            scythe.Value = false;
            pickaxe.Value = false;
            shovel.Value = true;
            SetTomatoGunServerRpc(false);
            turretItem.Value = false;
        }
        if (hotbarLocation == 4)
        {
            scythe.Value = false;
            pickaxe.Value = false;
            shovel.Value = false;
            SetTomatoGunServerRpc(true);
            turretItem.Value = false;
        }
        if (hotbarLocation == 5)
        {
            scythe.Value = false;
            pickaxe.Value = false;
            shovel.Value = false;
            SetTomatoGunServerRpc(false);
            turretItem.Value = true;
        }

    }
    [ServerRpc(RequireOwnership =false)]
    private void SetTomatoGunServerRpc(bool state)
    {
        SetTomatoGunClientRpc(state);
    }
    [ClientRpc(RequireOwnership =false)]
    private void SetTomatoGunClientRpc(bool state)
    {
        tomatoGun.Value = state;
    }
    //OnGUI will be temporary (will replace with canvas because those look nicer)
    private void OnGUI()
    {
        if (!IsOwner) { return; }
        GUI.Box(new Rect(200, 10, 180, 25), "Gold : " + (gold));
        if (seed.Value)
        {
            GUI.Box(new Rect(400, 10, 180, 25), "Seeds : " + (seeds));
        }
        if (goldSeed.Value)
        {
            GUI.Box(new Rect(400, 10, 180, 25), "GoldSeeds : " + (goldSeeds));
        }
    }
    public bool Mining
    {
        get { return mining; }
        set { mining = value; }
    }


    //idk this might be important later :( 
    public bool Scythe
    {
        get { return scythe.Value; }
        
    }
    public bool Pickaxe
    {
        get { return pickaxe.Value; }
        
    }
    public bool Shovel
    {
        get { return shovel.Value; }
        
    }
    public bool TomatoGun
    {
        get { return tomatoGun.Value; }
    }
    public bool Turret
    {
        get { return turretItem.Value; }
    }
    public bool Planting
    {
        get { return planting; }
        set { planting = value; }
    }
    public bool MiningState
    {
        get { return miningState.Value; }
    }
    public bool PlantingState
    {
        get { return plantingState.Value; }
    }
    public bool DonePlanting
    {
        get { return donePlanting; }
        set { donePlanting = value; }
    }
    public bool Seed
    {
        get { return seed.Value; }
    }
    public bool GoldSeed
    {
        get { return goldSeed.Value; }
    }
    public int Seeds
    {
        get { return seeds; }
        set { seeds = value; }
    }
    public int GoldenSeeds
    {
        get { return goldSeeds; }
        set { goldSeeds = value; }
    }
    public bool Swinging
    {
        get { return swinging.Value; }
        set { swinging.Value = value; }
    }
    
}
