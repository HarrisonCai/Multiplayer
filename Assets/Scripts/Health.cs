using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxhp;
    private float hp;
    void Start()
    {
        hp = maxhp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float HP
    {
        get { return hp; }
        set { hp = value; }
    }
    public float MaxHP
    {
        get { return maxhp; }
    }
}
