using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] GameObject waterAI;
    [SerializeField] GameObject fireAI;

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

}