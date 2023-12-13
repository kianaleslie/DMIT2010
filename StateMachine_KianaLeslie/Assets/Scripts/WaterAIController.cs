using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WaterAIController : MonoBehaviour                                                    /* Ignited - fire starts */
{                                                                                                 /* Yellow - "small fire" */
    //ai nav mesh agents                                                                          /* Pink - "medium fire"  */
    [SerializeField] public NavMeshAgent waterAI;                                                 /* Blue - "big fire"     */
    /* Extinguished - fire ends */
    //objects for collection
    [SerializeField] public GameObject waterBucketObject;
    [SerializeField] public Transform holdArea;
    Rigidbody heldObjectRb;

    //fire variables 
    [SerializeField] public GameObject fireSpawn;
    [SerializeField] public GameObject yellowFire;
    [SerializeField] public GameObject pinkFire;
    [SerializeField] public GameObject blueFire;

    [SerializeField] public TMP_Text uiText;
    [SerializeField] public GameObject uiObject;
    [SerializeField] public TMP_Text pressEText;

    //speeds for each state
    float waterAIWalkingSpeed = 5.0f;
    float waterAIrunningSpeed = 7.0f;

    float pickUpRange = 5.0f;
    float pickUpForce = 150.0f;

    //condition check for object collection
    bool hasWaterObject = false;

    //ai walks during pink flame event, runs during yellow(waterAI) and blue(fireAI) flame events, dances if yellow or blue as that is the secondary goal to igniting the fire and extinguishing it, throwing either water or fire wood on the fire 
    public enum AIState
    {
        Walking,
        Running,
        Dancing,
        Collecting,
        Throwing
    }
    //yellow makes fireAI run and waterAI dance, pink is neutral so they both walk, blue makes waterAI run and fireAI dance, extinguished when waterAI puts it out 
    public enum FireState
    {
        Ignited,
        YellowFlame,
        PinkFlame,
        BlueFlame,
        Extinguished
    }

    AIState waterAIState = AIState.Walking;
    FireState currentFlame = FireState.PinkFlame;

    private void Start()
    {
        SpawnFire();
        uiObject.SetActive(false);
    }
    private void Update()
    {
        WaterAIState();

        switch (waterAIState)
        {
            case AIState.Walking:
                SetAISpeed(waterAI, waterAIWalkingSpeed);
                UpdateUIText();
                break;
            case AIState.Running:
                SetAISpeed(waterAI, waterAIrunningSpeed);
                UpdateUIText();
                break;
            case AIState.Dancing:
                Dance(waterAI);
                UpdateUIText();
                break;
            case AIState.Collecting:
                if (currentFlame == FireState.BlueFlame)
                {
                    waterAIState = AIState.Running;
                    if (waterBucketObject == null)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                        {
                            CollectObject(hit.transform.gameObject);
                        }
                    }
                }
                if (currentFlame == FireState.PinkFlame)
                {
                    waterAIState = AIState.Walking;
                    if (waterBucketObject == null)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                        {
                            CollectObject(hit.transform.gameObject);
                        }
                    }
                    if (waterBucketObject != null)
                    {

                        MoveObject();
                    }
                }
                UpdateUIText();
                //if (currentFlame == FireState.PinkFlame)
                //{
                //    waterAIState = AIState.Walking;
                //    MoveAI(waterAI, waterBucketObject.transform.position);
                //    if (hasWaterObject)
                //    {
                //        MoveAI(waterAI, fireSpawn.transform.position);
                //    }
                //}
                //if (currentFlame == FireState.BlueFlame)
                //{
                //    waterAIState = AIState.Running;
                //    MoveAI(waterAI, waterBucketObject.transform.position);
                //    if (hasWaterObject)
                //    {
                //        MoveAI(waterAI, fireSpawn.transform.position);
                //    }
                //}
                //if (currentFlame == FireState.Extinguished)
                //{
                //    waterAIState = AIState.Running;
                //    MoveAI(waterAI, waterBucketObject.transform.position);
                //    if (hasWaterObject)
                //    {
                //        MoveAI(waterAI, fireSpawn.transform.position);
                //    }
                //}
                break;
            case AIState.Throwing:
                ThrowObject();
                UpdateUIText();
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
            }
            else
            {
                uiObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    void WaterAIState()
    {
        switch (currentFlame)
        {
            case FireState.Ignited:
                waterAIState = AIState.Running;
                SetAISpeed(waterAI, waterAIrunningSpeed);
                MoveAI(waterAI, waterBucketObject.transform.position);
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

    void SpawnFire()
    {
        switch (currentFlame)
        {
            case FireState.Ignited:

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
    void MoveObject()
    {
        if (Vector3.Distance(waterBucketObject.transform.position, holdArea.position) > 0.1)
        {
            Vector3 moveDirection = (holdArea.position = waterBucketObject.transform.position);
            heldObjectRb.AddForce(moveDirection * pickUpForce);
        }
    }
    void Dance(NavMeshAgent agent)
    {
        //make the ai spin to dance
        agent.transform.Rotate(Vector3.up, 360.0f * Time.deltaTime / 2);
    }
    void CollectObject(GameObject pickObject)
    {
        if (pickObject.GetComponent<Rigidbody>())
        {
            heldObjectRb = pickObject.GetComponent<Rigidbody>();
            heldObjectRb.useGravity = false;
            heldObjectRb.drag = 2;
            heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRb.transform.parent = holdArea;
            waterBucketObject = pickObject;
        }
    }
    private void ThrowObject()
    {
        heldObjectRb.useGravity = true;
        heldObjectRb.drag = 1;
        heldObjectRb.constraints = RigidbodyConstraints.None;

        heldObjectRb.transform.parent = null;
        waterBucketObject = null;
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