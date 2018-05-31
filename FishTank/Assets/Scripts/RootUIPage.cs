using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootUIPage : BaseUIPage {

    GameManager gManager;

	// Use this for initialization
	void Start ()
    {
        shopManager = GameObject.FindWithTag("ShopManager").GetComponent<ShopManager>();
        gManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void GetPageData(out string title, out string parentTitle, out List<string> childTitles, out bool itemPage)
    {
        title = this.title;
        parentTitle = "Return to Tank";
        childTitles = new List<string>();
        for (int i = 0; i < childPages.Count; i++)
        {
            childTitles.Add(childPages[i].title);
        }
        itemPage = false;
    }

    public override void GoToParent()
    {
        gManager.GoToTank();
    }
}
