using System.Collections;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] public GameObject fireSpawn;
    [SerializeField] public GameObject yellowFire;
    [SerializeField] public GameObject pinkFire;
    [SerializeField] public GameObject blueFire;
    private GameObject currentFlame;

    public bool isChangeState = true;
    public bool canCallWait = true;

    public enum FireState
    {
        Ignited,
        YellowFlame,
        PinkFlame,
        BlueFlame,
        Extinguished
    }

    public FireState currentFlameState = FireState.PinkFlame;
    public void Start()
    {
        currentFlameState = FireState.PinkFlame;
        currentFlame = Instantiate(pinkFire, fireSpawn.transform.position, Quaternion.identity); 
    }

    void Update()
    {
        //switch (currentFlameState)
        //{
        //    case FireState.Ignited:
        //        //
        //        break;
        //    case FireState.YellowFlame:
        //        if (isChangeState) 
        //        {
        //            if (canCallWait)
        //                StartCoroutine(WaitToChange());
        //            if (isChangeState)
        //            {
        //                Destroy(currentFlame);
        //                currentFlame = Instantiate(pinkFire, fireSpawn.transform.position, Quaternion.identity);
        //                currentFlameState = FireState.PinkFlame;
        //                canCallWait = true;
        //            }
        //        }
        //        break;
        //    case FireState.PinkFlame:
        //        if (isChangeState)
        //        {
        //            if (canCallWait)
        //            StartCoroutine(WaitToChange());
        //            if (isChangeState) 
        //            {
        //                Destroy(currentFlame);
        //                currentFlame = Instantiate(blueFire, fireSpawn.transform.position, Quaternion.identity);
        //                currentFlameState = FireState.BlueFlame;
        //                canCallWait = true;
        //            }
        //        }
        //        break;
        //    case FireState.BlueFlame:
        //        if (isChangeState)
        //        {
        //            if (canCallWait)
        //                StartCoroutine(WaitToChange());
        //            if (isChangeState)
        //            {
        //                Destroy(currentFlame);
        //                currentFlame = Instantiate(yellowFire, fireSpawn.transform.position, Quaternion.identity);
        //                currentFlameState = FireState.YellowFlame;
        //                canCallWait = true;
        //            }
        //        }
        //        break;
        //    case FireState.Extinguished:
        //        //
        //        break;
        //    default:
        //        break;
        //}
       
    }
    public IEnumerator WaitToChange()
    {
        canCallWait = false;
        isChangeState = false;
        yield return new WaitForSeconds(10.0f);
        isChangeState = true;
    }
    public void ChangeFireFlameColour()
    {
        Destroy(currentFlame);
        if (currentFlameState == FireState.YellowFlame)
        {
            currentFlame = Instantiate(pinkFire, fireSpawn.transform.position, Quaternion.identity);
            currentFlameState = FireState.PinkFlame;
        }
        else 
            if (currentFlameState == FireState.PinkFlame)
        {
            currentFlame = Instantiate(blueFire, fireSpawn.transform.position, Quaternion.identity);
            currentFlameState = FireState.BlueFlame;
        }
        //if (currentFlameState == FireState.BlueFlame)
        //{
        //    currentFlame = Instantiate(yellowFire, fireSpawn.transform.position, Quaternion.identity);
        //    currentFlameState = FireState.YellowFlame;
        //}
    }
    public void ChangeWaterFlameColour()
    {
        Destroy(currentFlame);
        //if (currentFlameState == FireState.YellowFlame)
        //{
        //    currentFlame = Instantiate(pinkFire, fireSpawn.transform.position, Quaternion.identity);
        //    currentFlameState = FireState.PinkFlame;
        //}
        if (currentFlameState == FireState.PinkFlame)
        {
            currentFlame = Instantiate(yellowFire, fireSpawn.transform.position, Quaternion.identity);
            currentFlameState = FireState.YellowFlame;
        }
        else
            if (currentFlameState == FireState.BlueFlame)
        {
            currentFlame = Instantiate(pinkFire, fireSpawn.transform.position, Quaternion.identity);
            currentFlameState = FireState.PinkFlame;
        }
    }
}