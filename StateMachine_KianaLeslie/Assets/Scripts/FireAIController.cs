using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class FireAIController : MonoBehaviour
{
    //ai nav mesh agent                                               
    [SerializeField] public NavMeshAgent fireAI;

    //objects for collection
    [SerializeField] public GameObject fireWoodObject;

    [SerializeField] FireController currentFlame;

    [SerializeField] public TMP_Text uiText;

    //speeds for each state
    public float fireAIWalkingSpeed;
    public float fireAIrunningSpeed;

    //condition check for object collection
    bool hasFireWoodObject = false;
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

    AIState fireAIState = AIState.Walking;

    private void Start()
    {
        MoveAITowards(fireAI, fireWoodObject.transform.position);
        fireAIWalkingSpeed = Random.Range(4, 8);
        fireAIrunningSpeed = Random.Range(6, 10);
    }
    private void Update()
    {
        if(isUpdating)
        {
            StartCoroutine(RandomSpeed());
        }
        FireAIState();

        switch (fireAIState)
        {
            case AIState.Walking:
                SetAISpeed(fireAI, fireAIWalkingSpeed);
                if (hasFireWoodObject)
                {
                    MoveAITowards(fireAI, currentFlame.fireSpawn.transform.position);
                }
                else
                {
                    MoveAITowards(fireAI, fireWoodObject.transform.position);
                }
                if (currentFlame.currentFlameState == FireController.FireState.YellowFlame)
                {
                    fireAIState = AIState.Running;
                }
                UpdateUIText();
                break;
            case AIState.Running:
                SetAISpeed(fireAI, fireAIrunningSpeed);
                if(hasFireWoodObject)
                {
                    MoveAITowards(fireAI, currentFlame.fireSpawn.transform.position);
                }
                else
                {
                    MoveAITowards(fireAI, fireWoodObject.transform.position);
                }
                if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                {
                    fireAIState = AIState.Walking;
                }
                UpdateUIText();
                break;
            case AIState.Dancing:
                Dance(fireAI);
                if (currentFlame.currentFlameState != FireController.FireState.BlueFlame)
                {
                    if (currentFlame.currentFlameState == FireController.FireState.YellowFlame)
                    {
                        fireAIState = AIState.Running;
                    }
                    if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                    {
                        fireAIState = AIState.Walking;
                    }
                }
                UpdateUIText();
                break;
            case AIState.Collecting:
                //MoveAITowards(waterAI, currentFlame.fireSpawn.transform.position);
                //if (currentFlame.currentFlameState == FireController.FireState.PinkFlame || currentFlame.currentFlameState == FireController.FireState.BlueFlame)
                //{
                //    MoveAITowards(waterAI, waterBucketObject.transform.position);

                    //    if (hasWaterBucketObject)
                    //    {
                    //        MoveAITowards(waterAI, currentFlame.fireSpawn.transform.position);
                    //    }
                    //}
                //else 
                //if (currentFlame.currentFlameState == FireController.FireState.YellowFlame)
                //{
                //    MoveAITowards(waterAI, waterBucketObject.transform.position);

                //    if (hasWaterBucketObject)
                //    {
                //        if (Vector3.Distance(waterAI.transform.position, currentFlame.fireSpawn.transform.position) > 1.0f)
                //        {
                //            MoveAITowards(waterAI, currentFlame.fireSpawn.transform.position);
                //        }
                //        else
                //        {
                //            waterAIState = AIState.Throwing;
                //        }
                //    }
                //}
                UpdateUIText();
                if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                {
                    fireAIState = AIState.Walking;
                }
                if (currentFlame.currentFlameState == FireController.FireState.YellowFlame)
                {
                    fireAIState = AIState.Running;
                }
                break;
            case AIState.Throwing:
                UpdateUIText();
                currentFlame.ChangeFireFlameColour();
                //if (currentFlame.currentFlameState == FireController.FireState.YellowFlame)
                //{
                //    currentFlame.canCallWait = false;
                //    currentFlame.isChangeState = true;
                //    currentFlame.currentFlameState = FireController.FireState.PinkFlame;
                //}
                //else if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                //{
                //    currentFlame.canCallWait = false;
                //    currentFlame.isChangeState = true;
                //    currentFlame.currentFlameState = FireController.FireState.BlueFlame;
                //}
                if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
                {
                    fireAIState = AIState.Walking;
                }
                if (currentFlame.currentFlameState == FireController.FireState.YellowFlame)
                {
                    fireAIState = AIState.Running;
                }
                break;
            default:
                break;
        }
    }
    IEnumerator RandomSpeed()
    {
        isUpdating = false;
        yield return new WaitForSeconds(10.0f);
        fireAIWalkingSpeed = Random.Range(4, 8);
        fireAIrunningSpeed = Random.Range(6, 10);
        isUpdating = true;  
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("FireWood") && !hasWaterBucketObject)
    //    {
    //        hasWaterBucketObject = true;
    //        waterAIState = AIState.Collecting;
    //        MoveAITowards(waterAI, currentFlame.fireSpawn.transform.position);
    //    }
    //    else if (collision.collider.CompareTag("Fire") && hasWaterBucketObject)
    //    {
    //        hasWaterBucketObject = false;
    //        waterAIState = AIState.Throwing;
    //        //if (currentFlame.currentFlameState == FireController.FireState.YellowFlame)
    //        //{
    //        //    currentFlame.currentFlameState = FireController.FireState.PinkFlame;
    //        //}
    //        //else if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
    //        //{
    //        //    currentFlame.currentFlameState = FireController.FireState.BlueFlame;
    //        //}

    //        //hasWaterBucketObject = false;
    //        //MoveAITowards(waterAI, waterBucketObject.transform.position);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FireWood") && !hasFireWoodObject)
        {
            hasFireWoodObject = true;
            fireAIState = AIState.Collecting;
            MoveAITowards(fireAI, currentFlame.fireSpawn.transform.position);
        }
        else if (other.CompareTag("Fire") && hasFireWoodObject)
        {
            hasFireWoodObject = false;
            fireAIState = AIState.Throwing;
            //if (currentFlame.currentFlameState == FireController.FireState.YellowFlame)
            //{
            //    currentFlame.currentFlameState = FireController.FireState.PinkFlame;
            //}
            //else if (currentFlame.currentFlameState == FireController.FireState.PinkFlame)
            //{
            //    currentFlame.currentFlameState = FireController.FireState.BlueFlame;
            //}

            //hasWaterBucketObject = false;
            //MoveAITowards(waterAI, waterBucketObject.transform.position);
        }
    }
    public void FireAIState()
    {
        switch (currentFlame.currentFlameState)
        {
            case FireController.FireState.Ignited:
                //
                break;
            case FireController.FireState.YellowFlame:
                fireAI.isStopped = false;
                //waterAIState = AIState.Running;
                break;
            case FireController.FireState.PinkFlame:
                fireAI.isStopped = false;
                //waterAIState = AIState.Walking;
                break;
            case FireController.FireState.BlueFlame:
                fireAI.isStopped = true;
                fireAIState = AIState.Dancing;
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
        fireAI.isStopped = true;
        //make the ai spin to dance
        agent.transform.Rotate(Vector3.up, 360.0f * Time.deltaTime / 2);
    }

    void UpdateUIText()
    {
        if (uiText != null)
        {
            switch (fireAIState)
            {
                case AIState.Walking:
                    uiText.text = "fire ai: Walking";
                    break;
                case AIState.Running:
                    uiText.text = "fire ai: Running";
                    break;
                case AIState.Dancing:
                    uiText.text = "fire ai: Dancing";
                    break;
                case AIState.Collecting:
                    uiText.text = "fire ai: Collecting";
                    break;
                case AIState.Throwing:
                    uiText.text = "fire ai: Throwing";
                    break;
            }
        }
    }
}


//reference to updating a with random range using coroutine in update 
//https://forum.unity.com/threads/running-a-coroutine-in-update.519105/