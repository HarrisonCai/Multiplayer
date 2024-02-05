using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolsItems : MonoBehaviour
{
    [SerializeField] private Gradient MiningGrad;
    [SerializeField] private RectTransform hotBarIndex;
    private int corn=0;
    private int maxCorn = 20;
    private int gold=0;
    private int seeds=5;
    private int goldSeeds=0;
    private bool sharpener = false;
    private int lighter=0;
    private int turret=0;
    private bool cornBag=false;
    private int healthPot=0;
    private bool cornCounter = false;
    private int hotbarLocation = 1;

    
    private bool mining = false;
    [SerializeField] private float resetMiningTime;
    private float miningTimer;
    [SerializeField] private Image goldProgress;
    private float target, currentFillGold;

    private bool scythe = false;
    private bool pickaxe = false;
    private bool shovel = false;
    private bool lighterItem = false;
    private bool turretItem = false;
    
    void Start()
    {
        miningTimer = resetMiningTime;
        goldProgress.fillAmount = 0;
    }

    
    void Update()
    {
        
        SwitchTool();
        //PICKAXE CODE
        goldProgress.fillAmount = (resetMiningTime - miningTimer) / resetMiningTime;
        UpdateProgress();
        if (pickaxe && mining && Input.GetMouseButton(0))
        {
            miningTimer -= Time.deltaTime;
        }
        else
        {
            miningTimer = resetMiningTime;
        }
        if (miningTimer <= 0)
        {
            gold++;
            miningTimer = resetMiningTime;
        }
        //----------------------------------------
    }
    private void UpdateProgress()
    {
        target = (resetMiningTime - miningTimer) / resetMiningTime;
        currentFillGold = goldProgress.fillAmount;
        goldProgress.fillAmount = Mathf.Lerp(currentFillGold, target, Time.deltaTime * 10);
        goldProgress.color = MiningGrad.Evaluate(goldProgress.fillAmount);
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

}
