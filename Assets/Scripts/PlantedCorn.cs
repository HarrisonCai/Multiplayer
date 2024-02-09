using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlantedCorn : NetworkBehaviour
{
    private NetworkVariable<bool> isPlanted = new NetworkVariable<bool>();
    private NetworkVariable<bool> corn = new NetworkVariable<bool>();
    private NetworkVariable<bool> goldenCorn = new NetworkVariable<bool>();
    [SerializeField] private float cornTimeToGrow;
    [SerializeField] private float goldenCornTimeToGrow;
    private float timer;
    [SerializeField] private GameObject cornIMG, goldenCornIMG;
    // Start is called before the first frame update
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
    public bool IsPlanted
    {
        get { return isPlanted.Value; }
        set { isPlanted.Value = value; }
    }
    public bool Corn
    {
        get { return corn.Value; }
        set { corn.Value = value; }
    }
    public bool GoldenCorn
    {
        get { return goldenCorn.Value; }
        set { goldenCorn.Value = value; }
    }
}
