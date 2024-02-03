using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolsItems : MonoBehaviour
{
    [SerializeField] private Gradient MiningGrad;
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

    
    private bool mining = false;
    [SerializeField] private float resetMiningTime;
    private float miningTimer;
    [SerializeField] private Image goldProgress;
    private float target, currentFillGold;
    //private bool pickaxe = false;
    
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
        if (/*pickaxe && */mining && Input.GetMouseButton(0))
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
}
