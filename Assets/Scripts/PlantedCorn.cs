using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantedCorn : MonoBehaviour
{
    private bool isPlanted = false;
    private bool corn;
    private bool goldenCorn;
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
        if (corn)
        {
            cornIMG.SetActive(true);
        }
        if (goldenCorn)
        {
            goldenCornIMG.SetActive(true);
        }
    }
    public bool IsPlanted
    {
        get { return isPlanted; }
        set { isPlanted = value; }
    }
    public bool Corn
    {
        get { return corn; }
        set { corn = value; }
    }
    public bool GoldenCorn
    {
        get { return goldenCorn; }
        set { goldenCorn = value; }
    }
}
