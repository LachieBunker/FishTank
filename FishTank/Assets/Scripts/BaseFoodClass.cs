using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFoodClass : MonoBehaviour {

    public int amount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int EatFood()
    {
        StartCoroutine(DestroyFood());
        return amount;
    }

    public IEnumerator DestroyFood()
    {
        yield return new WaitForEndOfFrame();
        GameObject.FindWithTag("TankManager").GetComponent<TankManager>().RemoveObjectFromTank(gameObject);
        Destroy(gameObject);
    }
}
