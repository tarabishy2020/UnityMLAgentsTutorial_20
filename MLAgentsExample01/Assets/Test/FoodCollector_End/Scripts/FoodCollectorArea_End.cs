using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class FoodCollectorArea_End : Area
{
    public GameObject food;
    public GameObject badFood;

    public int numFood = 25;
    public int numBadFood = 25;
    public bool respawnFood = true;
    // this is the range where food will be spawned
    // the court is 100 in width
    // so we will generate food between -45 and 45
    public float range = 45;

    void CreateFood(int num, GameObject type)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, new Vector3(Random.Range(-range, range), 1f,
                Random.Range(-range, range)) + transform.position,
                Quaternion.Euler(Vector3.zero));
            f.GetComponent<FoodLogic_End>().respawn = respawnFood;
            f.GetComponent<FoodLogic_End>().myArea = this;
        }
    }

    public void ResetFoodArea(GameObject[] agents)
    {
        foreach (GameObject agent in agents)
        {
            // Find the agents that are part of this area/"classroom"
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(Random.Range(-range, range), 2f,
                    Random.Range(-range, range))
                    + transform.position; // So that its relative to the current area's position
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }

        CreateFood(numFood, food);
        CreateFood(numBadFood, badFood);
    }
}
