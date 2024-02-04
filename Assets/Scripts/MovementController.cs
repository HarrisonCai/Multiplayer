using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] [Range(0.3f,100)]private float multiplier;
    private Vector2 speed;
    private float sin, cos;
    [SerializeField] private ToolsItems tools;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {

        
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
        if (/*tools.Pickaxe && */tools.Mining && Input.GetMouseButton(0))
        {
            speed *= 0.33f;
        }

        rb.velocity = speed * multiplier;
        
        
    }
}
