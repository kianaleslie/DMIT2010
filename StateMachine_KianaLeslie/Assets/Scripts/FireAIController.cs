using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireAIController : MonoBehaviour                                                   
{                                                                                               
    //ai nav mesh agent                                               
    [SerializeField] public NavMeshAgent fireAI;                                                  

    //objects for collection
    [SerializeField] public GameObject fireWoodObject;
    //[SerializeField] public Transform holdArea;
    //[SerializeField] public GameObject heldObj;
    Rigidbody heldObjectRb;

    //fire variables 
    [SerializeField] public GameObject fireSpawn;
    [SerializeField] public GameObject yellowFire;
    [SerializeField] public GameObject pinkFire;
    [SerializeField] public GameObject blueFire;

    //speeds for each state
    float fireAIWalkingSpeed = 4.0f;
    float fireAIrunningSpeed = 6.0f;

    float pickUpRange = 5.0f;
    float pickUpForce = 150.0f;

    //condition check for object collection
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
    //yellow makes fireAI run and waterAI dance, pink is neutral so they both walk, blue makes waterAI run and fireAI dance, extinguished when waterAI puts it out 
    public enum FireState
    {
        Ignited,
        YellowFlame,
        PinkFlame,
        BlueFlame,
        Extinguished
    }

    AIState fireAIState = AIState.Walking;
    FireState currentFlame = FireState.PinkFlame;

    private void Start()
    {
        SpawnFire();
    }
    private void Update()
    {
        FireAIState();

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
                //if (currentFlame == FireState.Ignited || currentFlame == FireState.YellowFlame)
                //{
                //    fireAIState = AIState.Running;
                //    if (fireWoodObject == null)
                //    {
                //        RaycastHit hit;
                //        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                //        {
                //            CollectObject(hit.transform.gameObject);
                //        }
                //    }
                //}
                //if (currentFlame == FireState.PinkFlame)
                //{
                //    fireAIState = AIState.Running;
                //    if (heldObj == null)
                //    {
                //        RaycastHit hit;
                //        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                //        {
                //            CollectObject(hit.transform.gameObject);
                //        }
                //    }
                //    if (heldObj != null)
                //    {
                //        MoveObject();
                //    }
                //}
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
                //ThrowObject();
                break;
            default:
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
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
    void FireAIState()
    {
        switch (currentFlame)
        {
            case FireState.Ignited:
                //fireAIState = AIState.Collecting;
                break;
            case FireState.YellowFlame:
                fireAIState = AIState.Running;
                SetAISpeed(fireAI, fireAIrunningSpeed);
                MoveAI(fireAI, fireWoodObject.transform.position);
                break;
            case FireState.PinkFlame:
                fireAIState = AIState.Walking;
                SetAISpeed(fireAI, fireAIWalkingSpeed);
                MoveAI(fireAI, fireWoodObject.transform.position);
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
                //
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
    //void MoveObject()
    //{
    //    if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1)
    //    {
    //        Vector3 moveDirection = (holdArea.position = heldObj.transform.position);
    //        heldObjectRb.AddForce(moveDirection * pickUpForce);
    //    }
    //}
    void Dance(NavMeshAgent agent)
    {
        //make the ai spin to dance
        agent.transform.Rotate(Vector3.up, 360.0f * Time.deltaTime / 2);
    }
    //void CollectObject(GameObject pickObject)
    //{
    //    if (pickObject.GetComponent<Rigidbody>())
    //    {
    //        heldObjectRb = pickObject.GetComponent<Rigidbody>();
    //        heldObjectRb.useGravity = false;
    //        heldObjectRb.drag = 2;
    //        heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation;

    //        heldObjectRb.transform.parent = holdArea;
    //        heldObj = pickObject;
    //    }
    //}
    //private void ThrowObject()
    //{
    //    heldObjectRb.useGravity = true;
    //    heldObjectRb.drag = 1;
    //    heldObjectRb.constraints = RigidbodyConstraints.None;

    //    heldObjectRb.transform.parent = null;
    //    heldObj = null;
    //}
}