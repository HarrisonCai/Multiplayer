using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] [Range(0.3f,20)]private float multiplier;
    Vector2 speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        speed = new Vector2(h * multiplier, v * multiplier).normalized;
        rb.velocity = speed;
    }
}
