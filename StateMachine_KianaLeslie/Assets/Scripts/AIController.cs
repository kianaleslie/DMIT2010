using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] public NavMeshAgent waterAI;
    [SerializeField] public NavMeshAgent fireAI;
    [SerializeField] public GameObject waterObject;
    [SerializeField] public GameObject fireWoodObject;

    [SerializeField] public GameObject fireSpawn;
    [SerializeField] public GameObject yellowFire;
    [SerializeField] public GameObject pinkFire;
    [SerializeField] public GameObject blueFire;

    float waterAIWalkingSpeed = 4.0f;
    float fireAIWalkingSpeed = 6.0f;
    float waterAIrunningSpeed = 6.0f;
    float fireAIrunningSpeed = 8.0f;

    enum AIState
    {
        Walking,
        Running,
        Dancing,
        Collecting,
        Throwing
    }
    enum FireState
    {
        YellowFlame,
        PinkFlame,
        BlueFlame
    }

    AIState waterAIState = AIState.Walking;
    AIState fireAIState = AIState.Walking;
    FireState currentFlame = FireState.YellowFlame;

    private void Start()
    {
        SetAISpeed(waterAI, waterAIWalkingSpeed);
        SetAISpeed(fireAI, fireAIWalkingSpeed);
    }
    private void Update()
    {
        WaterAIState();
        FireAIState();
        SpawnFire();
        switch (waterAIState)
        {
            case AIState.Walking:
                SetAISpeed(waterAI, waterAIWalkingSpeed);
                break;
            case AIState.Running:
                SetAISpeed(waterAI, waterAIrunningSpeed);
                break;
            case AIState.Dancing:
                break;
            case AIState.Collecting:
                break;
            case AIState.Throwing:
                break;
            default:
                break;
        }

        switch (fireAIState)
        {
            case AIState.Walking:
                SetAISpeed(fireAI, fireAIWalkingSpeed);
                break;
            case AIState.Running:
                SetAISpeed(fireAI, fireAIrunningSpeed);
                break;
            case AIState.Dancing:
                break;
            case AIState.Collecting:
                break;
            case AIState.Throwing:
                break;
            default:
                break;
        }
    }
    void WaterAIState()
    {
        switch (currentFlame)
        {
            case FireState.YellowFlame:
                waterAIState = AIState.Dancing;
                Dance(waterAI);
                break;
            case FireState.PinkFlame:
                waterAIState = AIState.Walking;
                MoveAI(waterAI, fireSpawn.transform.position);
                break;
            case FireState.BlueFlame:
                waterAIState = AIState.Running;
                MoveAI(waterAI, waterObject.transform.position);
                break;
            default:
                break;
        }
    }
    void FireAIState()
    {
        switch (currentFlame)
        {
            case FireState.YellowFlame:
                fireAIState = AIState.Running;
                MoveAI(fireAI, fireWoodObject.transform.position);
                break;
            case FireState.PinkFlame:
                fireAIState = AIState.Walking;
                MoveAI(fireAI, fireSpawn.transform.position);
                break;
            case FireState.BlueFlame:
                fireAIState = AIState.Dancing;
                Dance(fireAI);
                break;
            default:
                break;
        }
    }
    void SpawnFire()
    {
        switch (currentFlame)
        {
            case FireState.PinkFlame:

                Instantiate(pinkFire, fireSpawn.transform.position, Quaternion.identity);

                break;
            case FireState.YellowFlame:

                Instantiate(yellowFire, fireSpawn.transform.position, Quaternion.identity);

                break;
            case FireState.BlueFlame:

                Instantiate(blueFire, fireSpawn.transform.position, Quaternion.identity);

                break;
            default:
                break;
        }
    }
    void SetAISpeed(NavMeshAgent agent, float speed)
    {
        agent.speed = speed;
    }
    void MoveAI(NavMeshAgent agent, Vector3 destination)
    {
        if (!agent.hasPath || (agent.hasPath && agent.remainingDistance < 0.5f))
        {
            agent.SetDestination(destination);
        }
    }
    void Dance(NavMeshAgent agent)
    {
        agent.transform.Rotate(Vector3.up, 360.0f * Time.deltaTime);
    }
}