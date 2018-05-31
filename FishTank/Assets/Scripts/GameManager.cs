using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private int collectedMoney;
    public List<HarvestStruct> collectedHarvest; //change to private {get;} after debugging
    public Dictionary<string, ItemDataStruct> collectedItems;
    public Dictionary<string, ItemDataStruct> gameItems;
    public Text moneyText;
    public Text harvestText;
    public GameObject shopScreen;

    // Use this for initialization
    void Start()
    {
        collectedItems = new Dictionary<string, ItemDataStruct>();
        gameItems = new Dictionary<string, ItemDataStruct>();
        gameItems.Add("FishScales", new ItemDataStruct("FishScales", 20, 0, new List<string> { "" }));
        gameItems.Add("FishFood", new ItemDataStruct("FishFood", 10, 0, new List<string> { "Consumable" }));
        gameItems.Add("FishTreat", new ItemDataStruct("FishTreat", 20, 0, new List<string> { "Consumable" }));
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

    public int GetMoney()
    {
        return collectedMoney;
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

    public ItemDataStruct GetItemData(string itemName)
    {
        ItemDataStruct item = gameItems[itemName];
        if(collectedItems.ContainsKey(itemName))
        {
            item.numOwned = collectedItems[itemName].numOwned;
        }
        return item;
    }

    public void AddItemToInventory(string itemName)
    {

        ItemDataStruct item;
        if (collectedItems.ContainsKey(itemName))
        {
            item = collectedItems[itemName];
            item.numOwned++;
            collectedItems[itemName] = item;
        }
        else
        {
            item = gameItems[itemName];
            item.numOwned++;
            collectedItems.Add(item.Name, item);
        }
        //Debug.Log("Now have: " + collectedItems[itemName].numOwned + " " + itemName);
        UpdateUI();
    }

    public void RemoveItemFromInventory(string itemName)
    {
        if (collectedItems.ContainsKey(itemName))
        {
            ItemDataStruct item = collectedItems[itemName];
            if (item.numOwned > 1)
            {
                item.numOwned--;
                collectedItems[item.Name] = item;
            }
            else
            {
                collectedItems.Remove(item.Name);
            }
        }
        else
        {
            Debug.Log("Error: collectedItems doesn't contain: " + itemName);
        }

        UpdateUI();
    }

    public bool CanAffordItem(string itemName)
    {
        ItemDataStruct item = gameItems[itemName];
        if(collectedMoney >= item.Value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PurchaseItem(string itemName)
    {
        ItemDataStruct item = gameItems[itemName];
        if(CanAffordItem(item.Name))
        {
            RemoveMoney(item.Value);
            AddItemToInventory(item.Name);
        }
        else
        {
            Debug.Log("Can't afford: " + item.Name);
        }

        UpdateUI();
    }

    public void SellItem(string itemName)
    {
        if(collectedItems.ContainsKey(itemName))
        {
            int value = collectedItems[itemName].Value;
            AddMoney(value);
            RemoveItemFromInventory(itemName);
        }
        else
        {
            Debug.Log("Error: collectedItems doesn't contain: " + itemName);
        }
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        moneyText.text = "Money: " + collectedMoney.ToString();
        harvestText.text = "Harvest: " + collectedHarvest.Count;
    }

    public void GoToShop()
    {
        shopScreen.SetActive(true);
    }

    public void GoToTank()
    {
        shopScreen.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

[System.Serializable]
public struct ItemDataStruct//add gameObject?
{
    public string Name;
    public int Value;
    public int numOwned;
    public List<string> Tags;
    
    public ItemDataStruct(string name, int value, int numOwned, List<string> tags)
    {
        this.Name = name;
        this.Value = value;
        this.numOwned = numOwned;
        this.Tags = tags;
    }
}

[System.Serializable]
public struct HarvestStruct
{
    public string Name;
    public int Value;
}
