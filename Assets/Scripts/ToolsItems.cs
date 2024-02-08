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
    private bool cornBag=false;
    private int healthPot=0;
    private bool cornCounter = false;
    private int hotbarLocation = 1;


    private bool planting = false;
    [SerializeField] private float resetPlanting;
    private float plantingTimer;
    private bool plantingState;
    private bool donePlanting;

    private bool mining = false;
    [SerializeField] private float resetMiningTime;
    private float miningTimer;
    private bool miningState;

    [SerializeField] private Image Progress;
    [SerializeField] private Gradient ProgressGrad;
    private float target, currentFill;

    private bool scythe = false;
    private bool pickaxe = false;
    private bool shovel = false;
    private bool lighterItem = false;
    private bool turretItem = false;
    private bool seed, goldSeed;
    private int typeSeed = 0;
    
    void Start()
    {
        miningTimer = resetMiningTime;
        Progress.fillAmount = 0;
    }
    

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        
        SwitchTool();
        //PLANTING CODE

        if (seed && seeds > 0 && planting && Input.GetKey(KeyCode.X))
        {
            plantingTimer -= Time.deltaTime;
            UpdatePlantProgress();
            plantingState = true;
        }

        else if (goldSeed && goldSeeds > 0 && planting && Input.GetKey(KeyCode.X))
        {
            plantingTimer -= Time.deltaTime;
            UpdatePlantProgress();
            plantingState = true;

        }
        else
        {
            plantingTimer = resetPlanting;
            if (plantingState)
            {
                Progress.fillAmount = 0;
            }
            plantingState = false;
        }
        
        if (plantingTimer <= 0)
        {
            donePlanting = true;
            plantingTimer = resetPlanting;
            
        }

        //PICKAXE CODE


        if (pickaxe && mining && Input.GetMouseButton(0))
        {
            miningTimer -= Time.deltaTime;
            UpdateGoldProgress();
            miningState = true;
        }
        else
        {
            miningTimer = resetMiningTime;
            if (miningState)
            {
                Progress.fillAmount = 0;
            }
            miningState = false;
        }
        if (miningTimer <= 0)
        {
            gold++;
            miningTimer = resetMiningTime;
        }
        //----------------------------------------
    }
    private void UpdatePlantProgress()
    {
        target = (resetPlanting - plantingTimer) / resetPlanting;
        currentFill = Progress.fillAmount;
        Progress.fillAmount = Mathf.Lerp(currentFill, target, Time.deltaTime * 10);
        Progress.color = ProgressGrad.Evaluate(Progress.fillAmount);
    }
    private void UpdateGoldProgress()
    {
        target = (resetMiningTime - miningTimer) / resetMiningTime;
        currentFill = Progress.fillAmount;
        Progress.fillAmount = Mathf.Lerp(currentFill, target, Time.deltaTime * 10);
        Progress.color = ProgressGrad.Evaluate(Progress.fillAmount);
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
            seed = true;
            goldSeed = false;
        }
        else
        {
            seed = false;
            goldSeed = true;
        }
        float pos = -100 + (hotbarLocation-1 )* 50;

        hotBarIndex.localPosition = new Vector3(pos, hotBarIndex.localPosition.y, hotBarIndex.localPosition.z);
        if (hotbarLocation == 1)
        {
            scythe = true;
            pickaxe = false;
            shovel = false;
            lighterItem = false;
            turretItem = false;
        }
        if (hotbarLocation == 2)
        {
            scythe = false;
            pickaxe = true;
            shovel = false;
            lighterItem = false;
            turretItem = false;
        }
        if (hotbarLocation == 3)
        {
            scythe = false;
            pickaxe = false;
            shovel = true;
            lighterItem = false;
            turretItem = false;
        }
        if (hotbarLocation == 4)
        {
            scythe = false;
            pickaxe = false;
            shovel = false;
            lighterItem = true;
            turretItem = false;
        }
        if (hotbarLocation == 5)
        {
            scythe = false;
            pickaxe = false;
            shovel = false;
            lighterItem = false;
            turretItem = true;
        }

    }
    //OnGUI will be temporary (will replace with canvas because those look nicer)
    private void OnGUI()
    {
        GUI.Box(new Rect(200, 10, 180, 25), "Gold : " + (gold));
        if (seed)
        {
            GUI.Box(new Rect(400, 10, 180, 25), "Seeds : " + (seeds));
        }
        if (goldSeed)
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
        get { return scythe; }
        
    }
    public bool Pickaxe
    {
        get { return pickaxe; }
        
    }
    public bool Shovel
    {
        get { return shovel; }
        
    }
    public bool Lighter
    {
        get { return lighterItem; }
    }
    public bool Turret
    {
        get { return turretItem; }
    }
    public bool Planting
    {
        get { return planting; }
        set { planting = value; }
    }
    public bool MiningState
    {
        get { return miningState; }
    }
    public bool PlantingState
    {
        get { return plantingState; }
    }
    public bool DonePlanting
    {
        get { return donePlanting; }
        set { donePlanting = value; }
    }
    public bool Seed
    {
        get { return seed; }
    }
    public bool GoldSeed
    {
        get { return goldSeed; }
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
}
