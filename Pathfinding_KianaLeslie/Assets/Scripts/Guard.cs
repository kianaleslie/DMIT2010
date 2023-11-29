using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Guard : MonoBehaviour
{
    [SerializeField] GameObject currentNode;
    [SerializeField] GameObject nextNode;
    [SerializeField] GameObject startNode;
    [SerializeField] GameObject destinationNode;
    [SerializeField] GameObject destinationNode2;
    [SerializeField] GameObject destinationNode3;
    [SerializeField] GameObject previousNode;

    [SerializeField] float movementSpeed;
    [SerializeField] TMP_Text uiText;

    public enum GuardState
    {
        Patrollling,
        Delaying,
        Capturing,
    }
    public GuardState currentState = GuardState.Patrollling;
    void Start()
    {
        currentNode = startNode;
        nextNode = currentNode;

        transform.position = currentNode.transform.position;
        currentState = GuardState.Patrollling;
    }
    void Update()
    {
        if (currentNode == destinationNode)
        {
            destinationNode = destinationNode2;
            destinationNode2 = currentNode;
        }
        if (currentNode == destinationNode2)
        {
            destinationNode2 = destinationNode3;
            destinationNode3 = currentNode;
        }
        else
        {
            if (Vector3.Distance(transform.position, nextNode.gameObject.transform.position) < 0.1f)
            {
                previousNode = currentNode;
                currentNode = nextNode;

                float closest = 10000.0f;
                Pathnode pathnode = currentNode.GetComponent<Pathnode>();
                GameObject targetNode = currentNode;

                for (int i = 0; i < pathnode.connections.Count; i++)
                {
                    if (Vector3.Distance(destinationNode.transform.position, pathnode.connections[i].transform.position) < closest && pathnode.connections[i] != previousNode)
                    {
                        closest = Vector3.Distance(destinationNode.transform.position, pathnode.connections[i].transform.position);
                        targetNode = pathnode.connections[i];
                    }
                }
                nextNode = targetNode;
            }
            else
            {
                transform.Translate((nextNode.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime);
            }
        }
        UpdateUIText();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spy"))
        {
            currentState = GuardState.Capturing;
            UpdateUIText();
            StartCoroutine(Wait());
        }
    }
    void UpdateUIAfterEvent()
    {
        currentState = GuardState.Patrollling;
        UpdateUIText();
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
        UpdateUIAfterEvent();
    }
    public void UpdateUIText()
    {
        if (uiText != null)
        {
            switch (currentState)
            {
                case GuardState.Patrollling:
                    uiText.text = "Guard: Patrolling";
                    break;
                case GuardState.Delaying:
                    uiText.text = "Guard: Delaying";
                    break;
                case GuardState.Capturing:
                    uiText.text = "Guard: Capturing";
                    break;
            }
        }
    }
}
