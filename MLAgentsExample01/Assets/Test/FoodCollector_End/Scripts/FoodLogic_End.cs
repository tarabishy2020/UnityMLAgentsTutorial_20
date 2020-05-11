using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLogic_End : MonoBehaviour
{
    public bool respawn;
    public FoodCollectorArea_End myArea;
    // Start is called before the first frame update
    public void OnEaten()
    {
        if (respawn)
        {
            transform.position = new Vector3(Random.Range(-myArea.range, myArea.range),
                3f,
                Random.Range(-myArea.range, myArea.range)) + myArea.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
