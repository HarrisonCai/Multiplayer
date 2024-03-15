using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShopDetect : NetworkBehaviour
{
    [SerializeField] private ToolsItems chickenbutt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) { return; }
        if (collision.gameObject.CompareTag("Villager"))
        {
            chickenbutt.Shop = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsOwner)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Villager"))
        {
            chickenbutt.Shop = false;
        }
    }
}
