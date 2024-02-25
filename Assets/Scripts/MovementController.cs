using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MovementController : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] [Range(0.3f,100)]private float multiplier;
    private Vector2 speed;
    private float sin, cos;
    [SerializeField] private ToolsItems tools;
    [SerializeField] private GameObject playerUI;
    private bool move;
    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            playerUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //DO NOT TOUCH THIS
        if (!IsOwner || !move)
        {
            return;
        }
        //----------------------------


        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (v != 0 || h != 0)
        {

            sin = Mathf.Abs(v / (Mathf.Sqrt(Mathf.Pow(v, 2) + Mathf.Pow(h, 2))));
            cos = Mathf.Abs(h / (Mathf.Sqrt(Mathf.Pow(v, 2) + Mathf.Pow(h, 2))));

            speed = new Vector2(h * cos, v * sin);
        }
        else
        {
            speed = Vector2.zero;
        }
        if (tools.MiningState)
        {
            speed *= 0.33f;
        }
        if (tools.PlantingState || tools.ShovelingState)
        {
            speed = Vector2.zero;
        }

        rb.velocity = speed * multiplier;
        
        
    }
    public void Chat()
    {
        move = false;
    }
    public void unChat()
    {
        move = true;
    }
}
