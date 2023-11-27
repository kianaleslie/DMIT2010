using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
public class UIController : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {

    }
}


//For this assignment you will need to demonstrate a number of scenarios that include two spies and
//some guards.Each spy has an objective that is somewhat different from the other spy. They also each
//have unique qualities to help them evade the guards and infiltrate the area. The area will have locked
//doors and be patrolled by guards.You must decide what happens when the guards capture a spy and
//what the differences there are regarding the skills and behaviors of each spy.

//Each of the following must be demonstrated:

//1. The Blue Spy can find the document and escape the area
//2. The Blue Spy has a unique way to evade capture
//3. The Blue Spy has a way get past a locked a door

//4. The Red Spy can find the document and destroy it
//5. The Red Spy has a unique way to evade capture
//6. The Red Spy has a way get past a locked a door

//7. The Guards patrol a set of Waypoints
//8. The Guards can capture either spy
//9. The Guards will delay the progress of either spy for a period of time after capture

//10. Doors block sight checks when closed
//11. Doors block paths when locked

//12. There is feedback on the screen displaying the state of each spy and guard
//13. There is feedback on the screen displaying any objects that anyone is holding
//14. There is feedback on the screen that shows if a door is locked or not