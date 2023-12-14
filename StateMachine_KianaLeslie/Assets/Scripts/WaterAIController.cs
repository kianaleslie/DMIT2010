using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class WaterAIController : MonoBehaviour
{   
    //ai nav mesh agent                                               
    [SerializeField] public NavMeshAgent waterAI;

    //objects for collection
    [SerializeField] public GameObject waterBucketObject;

    [SerializeField] FireController currentFlame;
    [SerializeField] FireAIController text;

    [SerializeField] public TMP_Text uiText;
    [SerializeField] public GameObject uiObject;
    [SerializeField] public TMP_Text pressEText;

    //speeds for each state
    public float waterAIWalkingSpeed;
    public float waterAIrunningSpeed;

    //condition check for object collection
    bool hasWaterBucketObject = false;
    bool isUpdating = true;

    //ai walks during pink flame event, runs during yellow(waterAI) and blue(waterAI) flame events, dances if yellow or blue as that is the secondary goal to igniting the fire and extinguishing it, throwing either water or fire wood on the fire 
    public enum AIState
    {
        Walking,
        Running,
        Dancing,
        Collecting,
        Throwing
    }

    AIState waterAIState = AIState.Walking;

    private void Start()
    {
        MoveAITowards(waterAI, waterBucketObject.transform.position);
        uiObject.SetActive(false);
        waterAIWalkingSpeed = Random.Range(4, 8);
        waterAIrunningSpeed = Random.Range(6, 10);
    }
    private void Update()
    {
        if (isUpdating)
        {
            StartCoroutine(RandomSpeed());
        }
        WaterAIState();

        switch (waterAIState)
        {
            case AIState.Walking:
                SetAISpeed(waterAI, waterAIWalkingSpeed);
                if (hasWaterBucketObject)
                {
                    MoveAITowards(waterAI, currentFlame.fireSpawn.transform.position);
                }
                else
                {
                    MoveAITowards(waterAI, waterBucketObject.transform.position);
                }
                if (currentFlame.currentFlameState == FireController.FireState.BlueFlame)
                {
                    waterAIState = AIState.Running;
                }
                UpdateUIText();
                break;
            case AIState.Running:
                SetAISpeed(waterAI, waterAIrunningSpeed);
                if (hasWaterBucketObject)
                {
                    MoveAITowards(waterAI, currentFlame.fireSpawn.transform.position);
                }
                else
                {
                    MoveAITowards(waterAI, waterBucketObject.transform.position);
                }
                if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                {
                    waterAIState = AIState.Walking;
                }
                UpdateUIText();
                break;
            case AIState.Dancing:
                Dance(waterAI);
                if (currentFlame.currentFlameState != FireController.FireState.YellowFlame)
                {
                    if (currentFlame.currentFlameState == FireController.FireState.BlueFlame)
                    {
                        waterAIState = AIState.Running;
                    }
                    if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                    {
                        waterAIState = AIState.Walking;
                    }
                }
                UpdateUIText();
                break;
            case AIState.Collecting:
                UpdateUIText();
                if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                {
                    waterAIState = AIState.Walking;
                }
                if (currentFlame.currentFlameState == FireController.FireState.BlueFlame)
                {
                    waterAIState = AIState.Running;
                }
                break;
            case AIState.Throwing:
                UpdateUIText();
                currentFlame.ChangeWaterFlameColour();

                if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                {
                    waterAIState = AIState.Walking;
                }
                if (currentFlame.currentFlameState == FireController.FireState.BlueFlame)
                {
                    waterAIState = AIState.Running;
                }
                break;
            default:
                break;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (uiObject.activeSelf)
            {
                uiObject.SetActive(false);
                Time.timeScale = 1f;
                uiText.enabled = true;
                pressEText.enabled = true;
                text.uiText.enabled = true;
            }
            else
            {
                uiObject.SetActive(true);
                Time.timeScale = 0f;
                uiText.enabled = false;
                pressEText.enabled = false;
                text.uiText.enabled = false;
            }
        }
    }
    IEnumerator RandomSpeed()
    {
        isUpdating = false;
        yield return new WaitForSeconds(10.0f);
        waterAIWalkingSpeed = Random.Range(4, 8);
        waterAIrunningSpeed = Random.Range(6, 10);
        isUpdating = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterBucket") && !hasWaterBucketObject)
        {
            hasWaterBucketObject = true;
            waterAIState = AIState.Collecting;
            MoveAITowards(waterAI, currentFlame.fireSpawn.transform.position);
        }
        else if (other.CompareTag("Fire") && hasWaterBucketObject)
        {
            hasWaterBucketObject = false;
            waterAIState = AIState.Throwing;
        }
    }
    public void WaterAIState()
    {
        switch (currentFlame.currentFlameState)
        {
            case FireController.FireState.Ignited:
                //
                break;
            case FireController.FireState.YellowFlame:
                waterAI.isStopped = true;
                waterAIState = AIState.Dancing;
                break;
            case FireController.FireState.PinkFlame:
                waterAI.isStopped = false;
                break;
            case FireController.FireState.BlueFlame:
                waterAI.isStopped = false;
                break;
            case FireController.FireState.Extinguished:
                //
                break;
            default:
                break;
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
    }
    void SetAISpeed(NavMeshAgent agent, float speed)
    {
        //the ai will either be running or walking
        agent.speed = speed;
    }
    void MoveAITowards(NavMeshAgent agent, Vector3 destination)
    {
        if (agent.enabled)
        {
            agent.SetDestination(destination);
        }
    }
    void Dance(NavMeshAgent agent)
    {
        waterAI.isStopped = true;
        //make the ai spin to dance
        agent.transform.Rotate(Vector3.up, 360.0f * Time.deltaTime / 2);
    }

    void UpdateUIText()
    {
        if (uiText != null)
        {
            switch (waterAIState)
            {
                case AIState.Walking:
                    uiText.text = "water ai: Walking";
                    break;
                case AIState.Running:
                    uiText.text = "water ai: Running";
                    break;
                case AIState.Dancing:
                    uiText.text = "water ai: Dancing";
                    break;
                case AIState.Collecting:
                    uiText.text = "water ai: Collecting";
                    break;
                case AIState.Throwing:
                    uiText.text = "water ai: Throwing";
                    break;
            }
        }
    }
}