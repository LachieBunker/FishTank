using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

    GameManager gManager;

    //UIPageElements
    public BaseUIPage currentPage;
    public Text titleText;
    public Text parentText;
    public List<Text> childTexts;

    //UIItemPopoutElements
    private UIItem currentItem;
    private ItemDataStruct currentItemData;
    public GameObject itemPopout;
    public Text itemName;
    public Text itemOwned;
    public Text itemCost;

	// Use this for initialization
	void Start () {
        gManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        if(currentPage != null)
        {
            SetUIPage(currentPage);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //UIPageFunctions
    public void SetUIPage(BaseUIPage newPage)
    {
        currentPage = newPage;
        UpdateUIPage();
    }

    private void UpdateUIPage()
    {
        string title;
        string parentTitle;
        List<string> childTitles;
        bool itemPage;
        currentPage.GetPageData(out title, out parentTitle, out childTitles, out itemPage);
        titleText.text = title;
        parentText.text = parentTitle;
        for (int i = 0; i < childTexts.Count; i++)
        {
            if (childTitles.Count <= i)
            {
                childTexts[i].transform.parent.gameObject.SetActive(false);

            }
            else
            {
                childTexts[i].transform.parent.gameObject.SetActive(true);
                childTexts[i].text = childTitles[i];
                if(itemPage)
                {
                    if(gManager.CanAffordItem(childTitles[i]))
                    {
                        childTexts[i].transform.parent.GetComponent<Image>().color = Color.white;
                        childTexts[i].transform.parent.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        childTexts[i].transform.parent.GetComponent<Image>().color = Color.gray;
                        childTexts[i].transform.parent.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    childTexts[i].transform.parent.GetComponent<Image>().color = Color.white;
                    childTexts[i].transform.parent.GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void ParentClicked()
    {
        currentPage.GoToParent();
        
    }

    public void ButtonClicked(int index)
    {
        currentPage.ButtonClicked(index);
    }

    //UIItemPopoutFunctions
    public void PopoutItem(UIItem item)
    {
        currentItem = item;
        UpdatePopout();
    }

    private void UpdatePopout()
    {
        currentItemData = gManager.GetItemData(currentItem.itemName);
        itemPopout.SetActive(true);
        itemName.text = currentItemData.Name;
        itemOwned.text = currentItemData.numOwned.ToString();
        itemCost.text = currentItemData.Value.ToString();
    }

    public void PurchseItem()
    {
        gManager.PurchaseItem(currentItem.itemName);
        UpdatePopout();
    }

    public void ClosePopout()
    {
        currentItem = null;
        itemPopout.SetActive(false);
        UpdateUIPage();
    }
}
