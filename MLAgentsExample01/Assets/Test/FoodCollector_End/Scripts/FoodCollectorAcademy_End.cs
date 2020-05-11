using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class FoodCollectorAcademy_End : Academy
{
    public override void InitializeAcademy()
    {
        Monitor.SetActive(true);
    }
    public override void AcademyReset()
    {
        // Claer all food objects
        ClearObjects(GameObject.FindGameObjectsWithTag("food"));
        ClearObjects(GameObject.FindGameObjectsWithTag("badFood"));
        // Find all agents in the scene
        // notice if we didn't set them to "Agent" Tag 
        // we will not be able to find them this way
        // There are other ways of doing this but finiding with tag is faster
        var agents = GameObject.FindGameObjectsWithTag("agent");
        // This will then do the same for all Areas
        var listArea = FindObjectsOfType<FoodCollectorArea_End>();
        // And feed each of them all the avaliable Agents
        // So that the area can reset its own agents' positions 
        foreach (var fa in listArea)
        {
            fa.ResetFoodArea(agents);
        }
    }

    void ClearObjects(GameObject[] objects)
    {
        foreach (var food in objects)
        {
            Destroy(food);
        }
    }
}
