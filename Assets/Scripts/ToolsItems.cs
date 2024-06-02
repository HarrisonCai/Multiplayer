using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using Unity.Collections;
public class ToolsItems : NetworkBehaviour
{
    [SerializeField] private PlayerStorage stor;
    [SerializeField] private GameObject ItemCounts;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject plantGuide, goldGuide, shopGuide, storeGuide;
    [SerializeField] private GameObject TomatoGunButton, SharpeningButton, GoldenCornBagButton,CornBagImage,GoldenCornbagImage, TomatoGunImage;
    [SerializeField] private GameObject shopCan;
    [SerializeField] private TextMeshProUGUI cornTextVal, goldTextVal, seedText, goldSeedText, tomatoText, healthPotText, goldCornBagText, turretText, cornadeText, bearText;
    [SerializeField] private Health hp;
    [SerializeField] private RectTransform hotBarIndex, seedArrowIndex, swapButton;
    private NetworkVariable<int> corn= new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private int maxCorn = 60;
    private NetworkVariable<int> gold= new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private int seeds=5;
    private int goldSeeds=0;
    
    //private int lighter=0;
    private int turret = 0;
    private int cornade = 0;
    private float cornadeTimer = 0;
    public NetworkObject cornadeInstance = null;
    private bool hasTomatoGun = false;
    private NetworkVariable<float> damage = new NetworkVariable<float>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private int tomato = 0;
    private bool cornBag=false;
    private int healthPot=0;
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
    private NetworkVariable<bool> CornadeItem = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> seed = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> goldSeed = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private int typeSeed = 0;

    [SerializeField] GameObject scytheObj, tomatoGunObj;

    private NetworkVariable<bool> swinging = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private bool canSwing = true;
    private float swingTimer, resetswingTimer;

    private bool unitPlanted=false;
    private bool removed=false;
    [SerializeField] private float resetShovelTimer;
    private float shovelTimer;
    private NetworkVariable<bool> shovelingState = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<bool> storageHouse = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> storageState = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private float resetStorage;
    private float storageTimer;
    private NetworkVariable<bool> stored = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private bool shoppingState;
    private bool shop;
    private bool cornPot;
    private bool move;

    [SerializeField] private GameObject turretPrefab, cornadePrefab, bearTrapPrefab;
    private float bearTrap = 0;
    private NetworkObject instanceNetworkObject;

    private Vector2 distance;
    private bool AntiCCP;

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) { return; }
        if (collision.gameObject.CompareTag("Anti-CCP"))
        {
            AntiCCP = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsOwner) { return; }
        if (collision.gameObject.CompareTag("Anti-CCP"))
        {
            AntiCCP = false;
        }
    }
    void Start()
    {
        
        miningTimer = resetMiningTime;
        shovelTimer = resetShovelTimer;
        Progress.fillAmount = 0;
        stor = GameObject.Find("PlayerStorageData").GetComponent<PlayerStorage>();
        
    }
    

    void Update()
    {
        
        cornTextVal.text = ""+corn.Value;
        goldTextVal.text = "" + gold.Value;
        //Visual Updates
        if (plantingState.Value)
        {
            UpdatePlantProgress();
        }
        if (miningState.Value)
        {
            UpdateGoldProgress();
        }
        if (shovelingState.Value)
        {
            UpdateShovelProgress();
        }
        if (storageState.Value)
        {
            UpdateStorageProgress();
        }
        if (!(plantingState.Value || miningState.Value || shovelingState.Value || storageState.Value))
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
        if (hp.Dead) { return; }
        //-----------------------------------
        if (!IsOwner)
        {
            
            return;
        }
        TomatoGunImage.SetActive(hasTomatoGun);
        if (corn.Value > maxCorn)
        {
            corn.Value = maxCorn;
        }
        if (maxCorn == 60 && cornBag)
        {
            GoldenCornbagImage.SetActive(true);
            CornBagImage.SetActive(false);
            maxCorn = 120;
        }
        seedText.text = ""+seeds;
        goldSeedText.text = "" + goldSeeds;
        tomatoText.text = "" + tomato;
        healthPotText.text = "" + healthPot;
        goldCornBagText.text = "" + corn.Value + "/" + maxCorn;
        turretText.text = "" + turret;
        cornadeText.text = "" + cornade;
        bearText.text = "" + bearTrap;
        //SHOPPING
        if (shop && !shoppingState && Input.GetKeyDown(KeyCode.E))
        {
            shoppingState = true;
        }
        if (shoppingState && Input.GetKeyDown(KeyCode.Escape))
        {
            shoppingState = false;
        }
        shopCan.SetActive(shoppingState);
        if (!shoppingState)
        {
            shopGuide.SetActive(shop);
        }
        else
        {
            shopGuide.SetActive(false);
        }
        if (shoppingState) { return; }
        SwitchTool();
        ItemCounts.SetActive(Input.GetKey(KeyCode.Tab));
        plantGuide.SetActive(planting);
        goldGuide.SetActive(mining);
        storeGuide.SetActive(storageHouse.Value);
        
        //Scythe Code
        
        swingTimer -= Time.deltaTime;
        resetswingTimer -= Time.deltaTime;
        if (scythe.Value && canSwing && Input.GetMouseButtonDown(0))
        {
            swinging.Value = true;
            canSwing = false;
            swingTimer = 0.1f;
            resetswingTimer = 0.5f;
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
        
        if (!unitPlanted && seed.Value && seeds > 0 && planting && Input.GetKeyDown(KeyCode.X))
        {
            donePlanting = true;
        }

        else if (!unitPlanted && goldSeed.Value && goldSeeds > 0 && planting && Input.GetKeyDown(KeyCode.X))
        {
            donePlanting = true;

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
            gold.Value++;
            miningTimer = resetMiningTime;
        }
        //SHOVEL CODE
       
        if (shovel.Value && unitPlanted && Input.GetMouseButton(0))
        {
            shovelTimer -= Time.deltaTime;

            shovelingState.Value = true;
        }
        else
        {
            shovelingState.Value = false;
            shovelTimer = resetShovelTimer;
        }

        if (shovelTimer <= 0)
        {
            removed = true;
            shovelTimer = resetShovelTimer;

        }
        //TURRET PLACING CODE
        if (!AntiCCP && turret>0 && Input.GetKeyDown(KeyCode.Z))
        {
            turret--;
            
            PlaceTurretServerRpc();
            
        }
        //CORN GRENADE CODE
        cornadeTimer -= Time.deltaTime;
        
        if (cornadeTimer <= 0 && CornadeItem.Value && cornade>0 && Input.GetMouseButtonDown(0))
        {

            distance = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
            cornade--;
                ThrowCornadeServerRpc(distance.x,distance.y);
                cornadeTimer = 0.5f;
            
        }
        
        
        //STORAGE
        if (storageHouse.Value && corn.Value > 0 && Input.GetMouseButton(0))
        {
            storageTimer -= Time.deltaTime;
            
            ChangeStorageValStateServerRpc(true);
        }
        else
        {
            ChangeStorageValStateServerRpc(false);
            storageTimer = resetStorage;
        }

        if (storageTimer <= 0)
        {
            ChangeStoredStateServerRpc(true);
            storageTimer = resetStorage;

        }
        //HEALTH
        if (healthPot>0 && Input.GetKeyDown(KeyCode.C))
        {
            healthPot--;
            hp.ChangeHPServerRpc(-20);
        }
        //BEAR TRAP CODE
        if (  !AntiCCP && bearTrap>0 && Input.GetKeyDown(KeyCode.R)){
            PlaceBearTrapServerRpc();
            bearTrap--;
        }
        //DEV CHEATS DELETE LATER U BOZO
        /*if (Input.GetKeyDown(KeyCode.F))
        {
            gold.Value++;
            corn.Value+=750;
        }*/
  
    }

    //=======================================================================================================================]

    
    [ServerRpc(RequireOwnership=false)]
    public void ChangeStoredStateServerRpc(bool state)
    {
        stored.Value = state;
    }
    [ServerRpc(RequireOwnership =false)]
    public void ChangeStorageValStateServerRpc(bool state)
    {
        storageState.Value = state;
    }
    [ServerRpc(RequireOwnership = false)]
    private void PlaceTurretServerRpc()
    {
        instanceNetworkObject = Instantiate(turretPrefab, transform.position, this.transform.rotation).GetComponent<NetworkObject>();
        instanceNetworkObject.SpawnWithOwnership(OwnerClientId);
        
    }
    
    [ServerRpc(RequireOwnership =false)]
    private void ThrowCornadeServerRpc(float x, float y)
    {
        cornadeInstance = Instantiate(cornadePrefab, transform.position, this.transform.rotation).GetComponent<NetworkObject>();
        cornadeInstance.SpawnWithOwnership(OwnerClientId);
        cornadeInstance.gameObject.GetComponent<CornadeActive>().setDistanceClientRpc(x, y);
        cornadeInstance.gameObject.GetComponent<CornadeActive>().LaunchedServerRpc(true);
        cornadeInstance = null;
        //cornadeInstance.gameObject.GetComponent<CornadeActive>().LaunchedServerRpc(true);
        //cornadeInstance.gameObject.GetComponent<CornadeActive>().Player = this.gameObject;
        //cornadeInstance.gameObject.GetComponent<Rigidbody2D>().velocity += (Vector2)transform.right * 2;
    }
    [ServerRpc(RequireOwnership = false)]
    private void PlaceBearTrapServerRpc()
    {
        instanceNetworkObject = Instantiate(bearTrapPrefab, transform.position, this.transform.rotation).GetComponent<NetworkObject>();
        instanceNetworkObject.SpawnWithOwnership(OwnerClientId);

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
    private void UpdateShovelProgress()
    {

        UpdateShovelProgressServerRpc();

        Progress.fillAmount = target.Value;
        Progress.color = ProgressGrad.Evaluate(target.Value);
    }
    [ServerRpc(RequireOwnership = false)]
    private void UpdateShovelProgressServerRpc()
    {
        UpdateShovelProgressClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void UpdateShovelProgressClientRpc()
    {
        target.Value = (resetShovelTimer - shovelTimer) / resetShovelTimer;
    }
    private void UpdateStorageProgress()
    {

        UpdateStorageProgressServerRpc();

        Progress.fillAmount = target.Value;
        Progress.color = ProgressGrad.Evaluate(target.Value);
    }
    [ServerRpc(RequireOwnership = false)]
    private void UpdateStorageProgressServerRpc()
    {
        UpdateStorageProgressClientRpc();
    }
    [ClientRpc(RequireOwnership = false)]
    private void UpdateStorageProgressClientRpc()
    {
        target.Value = (resetStorage - storageTimer) / resetStorage;
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
            seedArrowIndex.localPosition = new Vector3(seedArrowIndex.localPosition.x, 160, hotBarIndex.localPosition.z);
            swapButton.localPosition = new Vector3(swapButton.localPosition.x, 125, swapButton.localPosition.z);
            seed.Value = true;
            goldSeed.Value = false;
        }
        else
        {
            seedArrowIndex.localPosition = new Vector3(seedArrowIndex.localPosition.x, 125, hotBarIndex.localPosition.z);
            swapButton.localPosition = new Vector3(swapButton.localPosition.x, 160, swapButton.localPosition.z);
            seed.Value = false;
            goldSeed.Value = true;
        }
        float pos = -98 + (hotbarLocation-1 )* 49;

        hotBarIndex.localPosition = new Vector3(pos, hotBarIndex.localPosition.y, hotBarIndex.localPosition.z);
        if (hotbarLocation == 1)
        {
            scythe.Value = true;
            pickaxe.Value = false;
            shovel.Value = false;
            SetTomatoGunServerRpc(false);
            CornadeItem.Value = false;
        }
        if (hotbarLocation == 2)
        {
            scythe.Value = false;
            pickaxe.Value = true;
            shovel.Value = false;
            SetTomatoGunServerRpc(false);
            CornadeItem.Value = false;
        }
        if (hotbarLocation == 3)
        {
            scythe.Value = false;
            pickaxe.Value = false;
            shovel.Value = true;
            SetTomatoGunServerRpc(false);
            CornadeItem.Value = false;
        }
        if (hotbarLocation == 4)
        {
            scythe.Value = false;
            pickaxe.Value = false;
            shovel.Value = false;
            if (hasTomatoGun)
            {
                SetTomatoGunServerRpc(true);
            }
            CornadeItem.Value = false;
        }
        if (hotbarLocation == 5)
        {
            scythe.Value = false;
            pickaxe.Value = false;
            shovel.Value = false;
            SetTomatoGunServerRpc(false);
            CornadeItem.Value = true;
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
    [ClientRpc(RequireOwnership =false)]
    public void ResetCornClientRpc()
    {
        corn.Value = 0;
    }
    //OnGUI will be temporary (will replace with canvas because those look nicer)
    /*private void OnGUI()
    {
        if (!IsOwner) { return; }
        
        GUI.Box(new Rect(200, 10, 180, 25), "Gold : " + (gold.Value));
        if (seed.Value)
        {
            GUI.Box(new Rect(400, 10, 180, 25), "Seeds : " + (seeds));
        }
        if (goldSeed.Value)
        {
            GUI.Box(new Rect(400, 10, 180, 25), "GoldSeeds : " + (goldSeeds));
        }
        GUI.Box(new Rect(600, 10, 180, 25), "Corn : " + (corn.Value));
        GUI.Box(new Rect(200, 40, 180, 25), "Turret : " + (turret));
        
    }*/
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
    public bool Cornade
    {
        get { return CornadeItem.Value; }
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
    public int Corn
    {
        get { return corn.Value; }
        set { corn.Value = value; }
    }
    public int MaxCorn
    {
        get { return maxCorn; }
    }
    public bool Removed
    {
        get { return removed; }
        set { removed = value; }
    }
    public bool UnitPlanted
    {
        get { return unitPlanted; }
        set { unitPlanted = value; }
    }
    public bool ShovelingState
    {
        get { return shovelingState.Value; }
    }
    
    [ServerRpc(RequireOwnership =false)]
    public void SetStorageHouseServerRpc(bool val)
    {
        storageHouse.Value = val;
    }
    public bool StoragHouse
    {
        get { return storageHouse.Value; }
    }
    public bool Stored
    {
        get { return stored.Value; }
    }
    public bool StoragState
    {
        get { return storageState.Value; }
    }
    public bool ShoppingState
    {
        get { return shoppingState; }
    }
    public bool Shop
    {
        get { return shop; }
        set { shop = value; }
    }
    public int Tomato
    {
        get { return tomato; }
        set { tomato = value; }
    }
    public float Damage
    {
        get { return damage.Value; }
    }
    public int Gold
    {
        get { return gold.Value; }
        set { gold.Value = value; }
    }
    
    public void AddCornSeed()
    {
        if (gold.Value >= 1)
        {
            gold.Value--;
            seeds++;
        }
    }
    public void AddGoldCornSeed()
    {
        if (gold.Value >= 5)
        {
            gold.Value -= 5;
            goldSeeds++;
        }
    }
    public void GetTomatoGun()
    {
        if (gold.Value >= 15)
        {
            gold.Value -= 15;
            hasTomatoGun=true;
            TomatoGunButton.SetActive(false);
        }
    }
    public void Sharpening()
    {
        if (gold.Value >= 20)
        {
            gold.Value -= 20;
            damage.Value=20;
            SharpeningButton.SetActive(false);
        }
    }
    public void AddTomatoes()
    {
        if (gold.Value >= 3)
        {
            gold.Value -= 3;
            tomato += 10;
        }
    }
    public void AddPotion()
    {
        if (gold.Value >= 2)
        {
            gold.Value -= 2;
            healthPot++;
        }
    }
    public void GoldenCornBag()
    {
        if (gold.Value >= 5)
        {
            gold.Value -= 5;
            cornBag = true;
            GoldenCornBagButton.SetActive(false);
        }
    }
    public void AddTurret()
    {
        if(gold.Value >= 8)
        {
            gold.Value -= 8;
            turret++;
        }
    }
    public void SellCorn()
    {
        if (corn.Value >= 15)
        {
            corn.Value -= 15;
            gold.Value += 1;
        }
    }
    public void AddCornade()
    {
        if(corn.Value >= 3 && gold.Value>=3)
        {
            corn.Value -= 3;
            gold.Value -= 3;
            cornade++;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void CornGoldAddServerRpc(int val, int val2)
    {
        CornGoldAddClientRpc(val, val2);
    }
    [ClientRpc(RequireOwnership = false)]
    public void CornGoldAddClientRpc(int val, int val2)
    {
        Corn += val;
        Gold += val2;
    }
    public void OneWithTheCorn()
    {
        if (gold.Value >= 4)
        {
           gold.Value -= 4;
            cornPot = true;
        }
    }
    public bool CornPot
    {
        get { return cornPot; }
        set { cornPot = value; }
    }
    public void AddBearTrap()
    {
        if (gold.Value >= 3)
        {
            gold.Value -= 3;
            bearTrap++;
        }
    }
    

}
