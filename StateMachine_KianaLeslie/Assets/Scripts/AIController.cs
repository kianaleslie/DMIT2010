using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
                                                                                         
public class AIController : MonoBehaviour                                                         /* Ignited - fire starts */
{                                                                                                 /* Yellow - "small fire" */
    //ai nav mesh agents                                                                          /* Pink - "medium fire"  */
    [SerializeField] public NavMeshAgent waterAI;                                                 /* Blue - "big fire"     */
    [SerializeField] public NavMeshAgent fireAI;                                                  /* Extinguished - fire ends */

    //objects for collection
    [SerializeField] public GameObject waterBucketObject;
    [SerializeField] public GameObject fireWoodObject;

    //fire variables 
    [SerializeField] public GameObject fireSpawn;
    [SerializeField] public GameObject yellowFire;
    [SerializeField] public GameObject pinkFire;
    [SerializeField] public GameObject blueFire;

    //speeds for each ai 
    float waterAIWalkingSpeed = 4.0f;
    float fireAIWalkingSpeed = 6.0f;
    float waterAIrunningSpeed = 6.0f;
    float fireAIrunningSpeed = 8.0f;

    //condition check for object collection
    bool hasWaterObject = false;
    bool hasFireWoodObject = false;

    //ai walks during pink flame event, runs during yellow(waterAI) and blue(fireAI) flame events, dances if yellow or blue as that is the secondary goal to igniting the fire and extinguishing it, throwing either water or fire wood on the fire 
    public enum AIState
    {
        Walking,
        Running,
        Dancing,
        Collecting,
        Throwing
    }
    //fire is ignited when fireAI starts it, yellow makes fireAI run and waterAI dance, pink is neutral so they both walk, blue makes waterAI run and fireAI dance, extinguished when waterAI puts it out 
    public enum FireState
    {
        Ignited,
        YellowFlame,
        PinkFlame,
        BlueFlame,
        Extinguished
    }

    AIState waterAIState = AIState.Walking;
    AIState fireAIState = AIState.Walking;
    FireState currentFlame;

    private void Start()
    {
        SetAISpeed(waterAI, waterAIWalkingSpeed);
        SetAISpeed(fireAI, fireAIWalkingSpeed);
        SpawnFire();
    }
    private void Update()
    {
        WaterAIState();
        FireAIState();

        switch (waterAIState)
        {
            case AIState.Walking:
                SetAISpeed(waterAI, waterAIWalkingSpeed);
                break;
            case AIState.Running:
                SetAISpeed(waterAI, waterAIrunningSpeed);
                break;
            case AIState.Dancing:
                Dance(waterAI);
                break;
            case AIState.Collecting:
                if(currentFlame == FireState.PinkFlame)
                {
                    waterAIState = AIState.Walking;
                    MoveAI(waterAI, waterBucketObject.transform.position);
                    if (hasWaterObject)
                    {
                        MoveAI(waterAI, fireSpawn.transform.position);
                    }
                }
                if (currentFlame == FireState.BlueFlame)
                {
                    waterAIState = AIState.Running;
                    MoveAI(waterAI, waterBucketObject.transform.position);
                    if (hasWaterObject)
                    {
                        MoveAI(waterAI, fireSpawn.transform.position);
                    }
                }
                if (currentFlame == FireState.Extinguished)
                {
                    waterAIState = AIState.Running;
                    MoveAI(waterAI, waterBucketObject.transform.position);
                    if (hasWaterObject)
                    {
                        MoveAI(waterAI, fireSpawn.transform.position);
                    }
                }
                break;
            case AIState.Throwing:
                ThrowObject(waterBucketObject, fireSpawn.transform.position);
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
                Dance(fireAI);
                break;
            case AIState.Collecting:
                if (currentFlame == FireState.Ignited)
                {
                    fireAIState = AIState.Running;
                    MoveAI(fireAI, fireWoodObject.transform.position);
                    if (hasFireWoodObject)
                    {
                        MoveAI(fireAI, fireSpawn.transform.position);
                    }
                }
                if (currentFlame == FireState.PinkFlame)
                {
                    fireAIState = AIState.Walking;
                    MoveAI(fireAI, fireWoodObject.transform.position);
                    if (hasFireWoodObject)
                    {
                        MoveAI(fireAI, fireSpawn.transform.position);
                    }
                }
                if (currentFlame == FireState.YellowFlame)
                {
                    fireAIState = AIState.Running;
                    MoveAI(fireAI, fireWoodObject.transform.position);
                    if (hasFireWoodObject)
                    {
                        MoveAI(fireAI, fireSpawn.transform.position);
                    }
                }
                break;
            case AIState.Throwing:
                ThrowObject(fireWoodObject, fireSpawn.transform.position);
                break;
            default:
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("WaterBucket") && !hasWaterObject)
        {
            hasWaterObject = true;
            waterAIState = AIState.Collecting;
            MoveAI(waterAI, fireSpawn.transform.position);
        }
        else if (collision.collider.CompareTag("Fire") && waterAIState == AIState.Collecting)
        {
            if (currentFlame == FireState.BlueFlame)
            {
                currentFlame = FireState.PinkFlame;
            }
            else if (currentFlame == FireState.PinkFlame)
            {
                currentFlame = FireState.YellowFlame;
            }

            hasWaterObject = false;
            waterAIState = AIState.Walking;
            MoveAI(waterAI, waterBucketObject.transform.position);
        }
        if (collision.collider.CompareTag("FireWood") && !hasFireWoodObject)
        {
            hasFireWoodObject = true;
            fireAIState = AIState.Collecting;
            MoveAI(fireAI, fireSpawn.transform.position);
        }
        else if (collision.collider.CompareTag("Fire") && fireAIState == AIState.Collecting)
        {
            if (currentFlame == FireState.YellowFlame)
            {
                currentFlame = FireState.PinkFlame;
            }
            else if (currentFlame == FireState.PinkFlame)
            {
                currentFlame = FireState.BlueFlame;
            }

            hasFireWoodObject = false;
            fireAIState = AIState.Walking;
            MoveAI(fireAI, fireWoodObject.transform.position);
        }
    }
    void WaterAIState()
    {
        switch (currentFlame)
        {
            case FireState.Ignited:
                break;
            case FireState.YellowFlame:
                waterAIState = AIState.Dancing;
                Dance(waterAI);
                break;
            case FireState.PinkFlame:
                waterAIState = AIState.Walking;
                SetAISpeed(waterAI, waterAIWalkingSpeed);
                MoveAI(waterAI, waterBucketObject.transform.position);
                if (waterAI.transform.position == waterBucketObject.transform.position)
                {
                    waterAIState = AIState.Collecting;
                }
                break;
            case FireState.BlueFlame:
                waterAIState = AIState.Running;
                SetAISpeed(waterAI, waterAIrunningSpeed);
                MoveAI(waterAI, waterBucketObject.transform.position);
                break;
            case FireState.Extinguished:
                waterAIState = AIState.Collecting;
                break;
            default:
                break;
        }
    }
    void FireAIState()
    {
        switch (currentFlame)
        {
            case FireState.Ignited:
                fireAIState = AIState.Collecting;
                break;
            case FireState.YellowFlame:
                fireAIState = AIState.Running;
                SetAISpeed(fireAI, fireAIrunningSpeed);
                MoveAI(fireAI, fireWoodObject.transform.position);
                break;
            case FireState.PinkFlame:
                fireAIState = AIState.Walking;
                SetAISpeed(fireAI, fireAIWalkingSpeed);
                MoveAI(fireAI, fireSpawn.transform.position);
                break;
            case FireState.BlueFlame:
                fireAIState = AIState.Dancing;
                Dance(fireAI);
                break;
                case FireState.Extinguished:
                break;
            default:
                break;
        }
    }
    void SpawnFire()
    {
        switch (currentFlame)
        {
            case FireState.Ignited:
                Instantiate(yellowFire, fireSpawn.transform.position, Quaternion.identity);
                break;
            case FireState.YellowFlame:
                Instantiate(yellowFire, fireSpawn.transform.position, Quaternion.identity);
                break;
            case FireState.PinkFlame:
                Instantiate(pinkFire, fireSpawn.transform.position, Quaternion.identity);
                break;
            case FireState.BlueFlame:
                Instantiate(blueFire, fireSpawn.transform.position, Quaternion.identity);
                break;
            case FireState.Extinguished:
                Destroy(yellowFire);
                Destroy(pinkFire);
                Destroy(blueFire);
                break;
            default:
                break;
        }
    }
    void SetAISpeed(NavMeshAgent agent, float speed)
    {
        //the ai will either be running or walking
        agent.speed = speed;
    }
    void MoveAI(NavMeshAgent agent, Vector3 destination)
    {
        //move the ai to the specified destination 
        if (agent.enabled)
        {
            agent.SetDestination(destination);
        }
    }
    void Dance(NavMeshAgent agent)
    {
        //make the ai spin to dance
        agent.transform.Rotate(Vector3.up, 360.0f * Time.deltaTime / 2);
    }
    private void ThrowObject(GameObject item, Vector3 firePosition)
    {
        //instantiate the water bucket or firewood above the fire and add a rigidbody to give it gravity to fall in 
        GameObject thrownItem = Instantiate(item, firePosition + Vector3.up * 2.0f, Quaternion.identity);

        Rigidbody objectRb = thrownItem.AddComponent<Rigidbody>();
        objectRb.AddForce(Vector3.down * 10.0f, ForceMode.Impulse);
        Destroy(thrownItem, 2.0f);
    }
}