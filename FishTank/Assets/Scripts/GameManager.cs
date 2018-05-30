using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private int collectedMoney;
    public List<HarvestStruct> collectedHarvest; //change to private {get;} after debugging
    public Text moneyText;
    public Text harvestText;

	// Use this for initialization
	void Start ()
    {
        collectedMoney = 10;
        UpdateUI();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
            RaycastHit hit;
            int mask = ~(1<<8);//add mask later
            if(Physics.Raycast(ray, out hit, 200, mask))
            {
                Debug.Log("Hit: " + hit.transform.gameObject);
                ObjectInteractionInterface objectInterfact = (ObjectInteractionInterface)hit.transform.gameObject.GetComponent(typeof(ObjectInteractionInterface));
                if(objectInterfact != null)
                {
                    objectInterfact.Interact();
                }
                /*if(hit.transform.tag == "Harvest")
                {
                    hit.transform.gameObject.GetComponent<BaseHarvestClass>().Interact();
                }*/
            }
        }
	}

    public void AddMoney(int amount)
    {
        collectedMoney += amount;
        UpdateUI();
    }

    public bool RemoveMoney(int amount)
    {
        if(collectedMoney >= amount)
        {
            collectedMoney -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddHarvest(HarvestStruct harvest)
    {
        collectedHarvest.Add(harvest);
        UpdateUI();
        SellHarvest(harvest);
    }

    public void SellHarvest(HarvestStruct harvest)
    {
        int value = harvest.Value;
        AddMoney(value);
        collectedHarvest.Remove(harvest);
        UpdateUI();
    }

    public void UpdateUI()
    {
        moneyText.text = "Money: " + collectedMoney.ToString();
        harvestText.text = "Harvest: " + collectedHarvest.Count;
    }

    public void Quit()
    {
        Application.Quit();
    }
}

[System.Serializable]
public struct HarvestStruct
{
    public string Name;
    public int Value;
}
