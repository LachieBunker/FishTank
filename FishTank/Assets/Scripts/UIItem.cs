using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour {

    public string itemName;

    public void GetItemData(out string name)
    {
        name = this.itemName;
    }
}
