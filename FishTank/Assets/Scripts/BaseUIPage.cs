using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIPage : MonoBehaviour {

    public string title;
    public GameObject parentPage;
    public List<BaseUIPage> childPages;
    public ShopManager shopManager;

	// Use this for initialization
	void Start () {
        shopManager = GameObject.FindWithTag("ShopManager").GetComponent<ShopManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Returns data for current UI page
    public virtual void GetPageData(out string title, out string parentTitle, out List<string> childTitles, out bool itemPage)
    {
        title = this.title;
        parentTitle = parentPage.GetComponent<BaseUIPage>().title;
        childTitles = new List<string>();
        for(int i = 0; i < childPages.Count; i++)
        {
            childTitles.Add(childPages[i].title);
        }
        itemPage = false;
    }

    //Redirects to parent page
    public virtual void GoToParent()
    {
        shopManager.SetUIPage(parentPage.GetComponent<BaseUIPage>());
    }

    //Redirect to child page based on index
    public virtual void ButtonClicked(int index)
    {
        shopManager.SetUIPage(childPages[index]);
    }
}
