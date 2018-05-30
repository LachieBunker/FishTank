using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour {

    private Vector3 tankDimensions = new Vector3(10, 5, 7.5f);
    private float spawnBuffer = 1;
    public List<GameObject> tankContents;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddObjectToTank(GameObject _object)
    {
        Vector3 pos = new Vector3(
            Random.Range(-(tankDimensions.x-spawnBuffer), (tankDimensions.x-spawnBuffer)),
            5 + Random.Range(-(tankDimensions.y-spawnBuffer), (tankDimensions.y-spawnBuffer)),
            Random.Range(-(tankDimensions.z-spawnBuffer), (tankDimensions.z-spawnBuffer)));
        GameObject temp = (GameObject)Instantiate(_object, pos, Quaternion.identity);
        tankContents.Add(temp);
    }

    public void AddObjectToTank(GameObject _object, Vector3 position)
    {
        GameObject temp = (GameObject)Instantiate(_object, position, Quaternion.identity);
        tankContents.Add(temp);
    }

    public void RemoveObjectFromTank(GameObject _object)
    {
        tankContents.Remove(_object);
    }

    public void AddFood(GameObject food)
    {
        if(GameObject.FindWithTag("GameController").GetComponent<GameManager>().RemoveMoney(2))
        {
            AddObjectToTank(food);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}


