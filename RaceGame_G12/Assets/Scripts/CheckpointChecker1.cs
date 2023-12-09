using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointChecker1 : MonoBehaviour
{
    public CarController1 theCar;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Checkpoint")
        {
            //Debug.Log("Hit cp " + other.GetComponent<Checkpoint>().cpNumber); 

            theCar.CheckpointHit(other.GetComponent<Checkpoint1>().cpNumber);

        }
    }
}
