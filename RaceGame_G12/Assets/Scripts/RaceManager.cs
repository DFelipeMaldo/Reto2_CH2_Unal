using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;

    public Checkpoint[] allCheckpoints;

    public int totalLaps;

    public CarController playerCar;
    public List<CarController> allAICars = new List<CarController>();
    public int playerPosition;
    public float timeBetweenPosCheck = .2f;
    private float posChkCounter;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            allCheckpoints[i].cpNumber = i;
        }


    }

    // Update is called once per frame
    void Update()
    {

        posChkCounter -= Time.deltaTime;
        if (posChkCounter <= 0)
        {

            playerPosition = 1;

            foreach (CarController aiCar in allAICars)
            {
                if (aiCar.currentLap > playerCar.currentLap)
                {
                    playerPosition++;
                }
                else if (aiCar.currentLap == playerCar.currentLap)
                {
                    if (aiCar.nextCheckpoint > playerCar.nextCheckpoint)
                    {
                        playerPosition++;
                    }
                    else if (aiCar.nextCheckpoint == playerCar.nextCheckpoint)
                    {
                        if (Vector3.Distance(aiCar.transform.position, allCheckpoints[aiCar.nextCheckpoint].transform.position) < Vector3.Distance(playerCar.transform.position, allCheckpoints[aiCar.nextCheckpoint].transform.position))
                        {
                            playerPosition++;
                        }
                    }
                }
            }
            posChkCounter = timeBetweenPosCheck;
            UIManager.instance.positionText.text = playerPosition + "/" + (allAICars.Count + 1);
        }
    }
}