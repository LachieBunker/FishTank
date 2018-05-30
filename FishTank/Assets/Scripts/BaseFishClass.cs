using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFishClass : MonoBehaviour, ObjectInteractionInterface {

    public float moveSpeed; //Change to {get;} after debugging
    private int dirBinary = 1;
    private Vector3 dirAngle;
    private float bufferRange = 2;
    private Vector3 moveTarget;
    private float timeSpeed = 5;
    private int hunger;
    private int hungerDecay = 5;
    private int happiness;
    private int happinessDecay = 5;
    private int happinessInteractions;
    private int happinessInteractionLimit = 10;
    private int harvestChance;
    private int harvestChanceIncreaseRate = 5;
    public GameObject harvest;

    // Use this for initialization
    void Start()
    {
        hunger = 60;
        happiness = 30;
        happinessInteractions = 0;
        harvestChance = 0;
        //StartCoroutine(Movement());
        StartCoroutine(SimulateTime());
    }

    // Update is called once per frame
    void Update()
    {
        GameObject nearbyFood = FoodNearby();
        if (nearbyFood != null)
        {
            moveTarget = nearbyFood.transform.position;
            if (Vector3.Distance(transform.position, moveTarget) < bufferRange)
            {
                //yield return new WaitForSeconds(2);
                if (nearbyFood != null)
                {
                    EatFood(nearbyFood);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed);
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.right * dirBinary, out hit, bufferRange))
            {
                //Debug.Log("Hit: " + hit.transform.gameObject);
                if (hit.transform.tag == "Tank")
                {
                    dirBinary *= -1;
                }
                else
                {
                    transform.Translate(moveSpeed * dirBinary, 0, 0);
                }
            }
            else
            {
                transform.Translate(moveSpeed * dirBinary, 0, 0);
            }
        }
    }

    //Check if there is food nearby
    public GameObject FoodNearby()
    {
        RaycastHit[] hits;
        GameObject target = null;
        float distance = 30;
        hits = Physics.SphereCastAll(transform.position, 10, Vector3.forward);
        if(hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                if(hit.transform.tag == "Food")
                {
                    float tempDistance = Vector3.Distance(transform.position, hit.transform.position);
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        target = hit.transform.gameObject;
                    }
                }
            }
        }
        return target;
    }


    private IEnumerator Movement()
    {
        GameObject nearbyFood = FoodNearby();
        if(nearbyFood != null)
        {
            moveTarget = nearbyFood.transform.position;
            if (Vector3.Distance(transform.position, moveTarget) < bufferRange)
            {
                yield return new WaitForSeconds(2);
                if(nearbyFood != null)
                {
                    EatFood(nearbyFood);
                }
            }
            else
            {
                Vector3.MoveTowards(transform.position, moveTarget, moveSpeed);
            }
        }
        else
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.right * dirBinary, out hit))
            {
                if(hit.transform.tag == "Tank")
                {
                    dirBinary *= -1;
                }
                else
                {
                    transform.Translate(moveSpeed * dirBinary, 0, 0);
                }
            }
        }
    }

    private IEnumerator SimulateTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(timeSpeed);
            hunger -= hungerDecay;
            happiness -= happinessDecay;
            harvestChance += harvestChanceIncreaseRate;
            Debug.Log("Hunger now: " + hunger + " HarvestChance: " + harvestChance);
            if (harvestChance > 20 && hunger > 80)
            {
                int spawnChance = Random.Range(0, 100);
                if (spawnChance < (harvestChance * (happiness/100)))
                {
                    ProduceHarvest();
                    harvestChance = (hunger / 100) * 20;
                    hunger -= 20;
                }
            }
        }
        
    }

    public bool IncreaseHappiness(int amount, bool ignoreLimit = false)
    {
        if(ignoreLimit)
        {
            happiness += amount;
            happiness = Mathf.Clamp(happiness, 0, 100);
            return true;
        }
        else if(happinessInteractions <happinessInteractionLimit)
        {
            happiness += amount;
            happinessInteractions++;
            happiness = Mathf.Clamp(happiness, 10, 500);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EatFood(GameObject food)
    {
        int amount = food.GetComponent<BaseFoodClass>().EatFood();
        hunger += amount;
    }

    public void ProduceHarvest()
    {
        GameObject.FindWithTag("TankManager").GetComponent<TankManager>().AddObjectToTank(harvest, new Vector3(transform.position.x + (-dirBinary * 2), transform.position.y, transform.position.z));
    }

    public void Dead()
    {
        GameObject.FindWithTag("TankManager").GetComponent<TankManager>().RemoveObjectFromTank(gameObject);
        //Trigger fish dead command/option in game manager
        Destroy(gameObject);
    }

    public void Interact()//stop from triggering every frame
    {
        int happinessIncrease = Random.Range(10, (15 - Mathf.FloorToInt((float) (happiness / 100))));
        Debug.Log("Happiness increase: " + happinessIncrease);
        if(IncreaseHappiness(10))
        {
            Debug.Log("Increased happiness. Happiness now: " + happiness);
        }
        else
        {
            //Debug.Log("Didn't increase happiness. Happiness now: " + happiness);
        }
        
    }
}
