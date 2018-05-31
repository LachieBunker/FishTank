using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIPage : BaseUIPage {

    public List<UIItem> childItems;

	// Use this for initialization
	void Start () {
        shopManager = GameObject.FindWithTag("ShopManager").GetComponent<ShopManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void GetPageData(out string title, out string parentTitle, out List<string> childTitles, out bool itemPage)
    {
        title = this.title;
        parentTitle = parentPage.GetComponent<BaseUIPage>().title;
        childTitles = new List<string>();
        for (int i = 0; i < childItems.Count; i++)
        {
            childTitles.Add(childItems[i].itemName);
        }

        itemPage = true;
    }

    public override void ButtonClicked(int index)
    {
        shopManager.PopoutItem(childItems[index]);
    }
}
