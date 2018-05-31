using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHarvestClass : MonoBehaviour, ObjectInteractionInterface {

    public HarvestStruct harvestData;

    //implement interface with function
    public void Interact()
    {
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().AddItemToInventory(harvestData.Name);
        GameObject.FindWithTag("TankManager").GetComponent<TankManager>().RemoveObjectFromTank(gameObject);
        Destroy(gameObject);
    }

    public HarvestStruct CollectHarvest()
    {
        return harvestData;
    }

    public void DestroyHarvest()
    {
        Destroy(gameObject);
    }
}
