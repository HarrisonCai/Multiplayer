using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlantingUnit : NetworkBehaviour
{

    [SerializeField] private ToolsItems plantState;
    private Collider2D Unit;
    // Update is called once per frame
    void Update()
    {

        if (!IsOwner)
        {
            return;
        }
        
        if (Unit != null)
        {
            
            plantState.UnitPlanted = Unit.gameObject.GetComponent<PlantedCorn>().IsPlanted;
            if (plantState.Removed && Unit.gameObject.GetComponent<PlantedCorn>().IsPlanted)
            {
                plantState.Removed = false;
                plantState.UnitPlanted = false;
                Unit.gameObject.GetComponent<PlantedCorn>().GrowStateServerRpc(false);
                Unit.gameObject.GetComponent<PlantedCorn>().DisableGrowingServerRpc();
            }
            if (!Unit.gameObject.GetComponent<PlantedCorn>().IsPlanted && plantState.DonePlanting)
            {
                
                plantState.DonePlanting = false;
                if (plantState.Seed)
                {
                    Unit.gameObject.GetComponent<PlantedCorn>().CornServerRpc();
                    plantState.Seeds--;
                }
                if (plantState.GoldSeed)
                {

                    Unit.gameObject.GetComponent<PlantedCorn>().GoldenCornServerRpc();
                    plantState.GoldenSeeds--;
                }
                Unit.gameObject.GetComponent<PlantedCorn>().SetClientServerRpc(OwnerClientId);
            }
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner)
        {
            return;
        }
        if (collision.gameObject.CompareTag("PlantingUnit"))
        {

            
            plantState.Planting = true;
                
            
            Unit = collision;
        }
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlantingUnit"))
        {
            
            if (!collision.gameObject.GetComponent<PlantedCorn>().IsPlanted && plantState.DonePlanting )
            {
                plantState.DonePlanting = false;
                collision.gameObject.GetComponent<PlantedCorn>().IsPlanted = true;
                if (plantState.Seed)
                {
                    collision.gameObject.GetComponent<PlantedCorn>().Corn = true;
                    plantState.Seeds--;
                }
                if (plantState.GoldSeed)
                {

                    collision.gameObject.GetComponent<PlantedCorn>().GoldenCorn = true;
                    plantState.GoldenSeeds--;
                }
            }
        }
    }*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsOwner) { return; }
        if (collision.gameObject.CompareTag("PlantingUnit"))
        {
       
            Unit = null;
            plantState.Planting = false;
            plantState.UnitPlanted = false;
        }
    }
}
