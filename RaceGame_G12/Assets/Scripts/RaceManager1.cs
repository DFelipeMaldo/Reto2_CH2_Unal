using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager1 : MonoBehaviour
{
    public static RaceManager1 instance;

    public Checkpoint1[] allCheckpoints;

    public int totalLaps;

    public CarController1 playerCar;
    public List<CarController1> allAICars = new List<CarController1>();
    public int playerPosition;
    public float timeBetweenPosCheck = .2f;
    private float posChkCounter;

    public float aiDefaultSpeed = 30f, playerDefaultSpeed = 30f, rubberBandSpeedMod = 3.5f, rubBandAccel = .5f;

    public bool isStarting;
    public float timeBetweenStartCount = 1f;
    private float startCounter;
    public int countdownCurrent = 3;

    public bool raceCompleted;

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

        isStarting = true;
        startCounter = timeBetweenStartCount;

        UIManager1.instance.countDownText.text = countdownCurrent + "!";

    }

    // Update is called once per frame
    void Update()
    {

        if (isStarting)
        {
            startCounter -= Time.deltaTime;
            if (startCounter <= 0)
            {
                countdownCurrent--;
                startCounter = timeBetweenStartCount;

                UIManager1.instance.countDownText.text = countdownCurrent + "!";

                if (countdownCurrent == 0)
                {
                    isStarting = false;

                    UIManager1.instance.countDownText.gameObject.SetActive(false);
                    UIManager1.instance.goText.gameObject.SetActive(true);
                }
            }
        }
        else
        {

            posChkCounter -= Time.deltaTime;
            if (posChkCounter <= 0)
            {

                playerPosition = 1;

                foreach (CarController1 aiCar in allAICars)
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

                UIManager1.instance.positionText.text = playerPosition + "/" + (allAICars.Count + 1);
            }

            if (playerPosition == 1)
            {
                foreach (CarController1 aiCar in allAICars)
                {
                    aiCar.maxSpeed = Mathf.MoveTowards(aiCar.maxSpeed, aiDefaultSpeed + rubberBandSpeedMod, rubBandAccel * Time.deltaTime);
                }

                playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefaultSpeed - rubberBandSpeedMod, rubBandAccel * Time.deltaTime);
            }
            else
            {
                foreach (CarController1 aiCar in allAICars)
                {
                    aiCar.maxSpeed = Mathf.MoveTowards(aiCar.maxSpeed, aiDefaultSpeed - (rubberBandSpeedMod * ((float)playerPosition / ((float)allAICars.Count + 1))), rubBandAccel * Time.deltaTime);
                }

                playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefaultSpeed + (rubberBandSpeedMod * ((float)playerPosition / ((float)allAICars.Count + 1))), rubBandAccel * Time.deltaTime);
            }

        }
    }
    public void FinishRace()
    {
        raceCompleted = true;


        switch (playerPosition)
        {
            case 1:
                UIManager1.instance.raceResultText.text = "You finished 1st";

                break;

            case 2:
                UIManager1.instance.raceResultText.text = "You finished 2nd";

                break;

            case 3:
                UIManager1.instance.raceResultText.text = "You finished 3rd";

                break;

            default:

                UIManager1.instance.raceResultText.text = "You finished " + playerPosition + "th";

                break;
        }


        UIManager1.instance.resultsScreen.SetActive(true);
    }
}