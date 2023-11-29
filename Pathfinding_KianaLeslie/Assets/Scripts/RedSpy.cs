using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class RedSpy : MonoBehaviour
{
    [SerializeField] GameObject currentNode;
    [SerializeField] GameObject nextNode;
    [SerializeField] GameObject startNode;
    [SerializeField] GameObject destinationNode;
    [SerializeField] GameObject destinationNode2;
    [SerializeField] GameObject destinationNode3;
    [SerializeField] GameObject previousNode;

    [SerializeField] float baseMovementSpeed;
    float currentMovementSpeed;
    int speedBoostCount = 0;
    int maxSpeedBoosts = 3;
    [SerializeField] TMP_Text uiText;

    enum SpyState
    {
        Spying,
        SpeedBoost,
        Captured,
        DestroyedDocument
    }
    SpyState currentState = SpyState.Spying;

    void Start()
    {
        currentNode = startNode;
        nextNode = currentNode;
        currentMovementSpeed = baseMovementSpeed;

        transform.position = currentNode.transform.position;
        UpdateUIText();
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
                transform.Translate((nextNode.transform.position - transform.position).normalized * currentMovementSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Guard"))
        {
            while(speedBoostCount <= maxSpeedBoosts)
            {
                currentState = SpyState.SpeedBoost;
                ApplySpeedBoost();
                StartCoroutine(RemoveSpeedBoost(3.0f));
                UpdateUIText();
            }
            StopMovingForDuration(6.0f);
            //if (speedBoostCount >= maxSpeedBoosts)
            //{
            //    currentState = SpyState.SpeedBoost;
            //    ApplySpeedBoost();
            //    StartCoroutine(RemoveSpeedBoost(3.0f));
            //    UpdateUIText();
            //}
            //else
            //{
            //    StopMovingForDuration(6.0f);
            //}
        }
        else
        if (other.CompareTag("Document"))
        {
            DestroyDocument(other.gameObject);
        }
    }
    void ApplySpeedBoost()
    {
        currentMovementSpeed += 5.0f;
        currentState = SpyState.SpeedBoost;
        UpdateUIText();
    }
    IEnumerator RemoveSpeedBoost(float timeToHaveBoost)
    {
        yield return new WaitForSeconds(timeToHaveBoost);
        currentMovementSpeed -= 5.0f;
        currentState = SpyState.SpeedBoost;
        UpdateUIText();
        StartCoroutine(Wait());
    }
    IEnumerator StopMovingForDuration(float duration)
    {
        currentMovementSpeed = 0.0f;
        yield return new WaitForSeconds(duration);
        currentMovementSpeed = baseMovementSpeed;
        currentState = SpyState.Captured;
        UpdateUIText();
        StartCoroutine(Wait());
    }
    void DestroyDocument(GameObject document)
    {
        Destroy(document);
        currentState = SpyState.DestroyedDocument;
        UpdateUIText();
        StartCoroutine(Wait());
    }
    void UpdateUIAfterEvent()
    {
        currentState = SpyState.Spying;
        UpdateUIText();
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
        UpdateUIAfterEvent();
    }
    void UpdateUIText()
    {
        if (uiText != null)
        {
            switch (currentState)
            {
                case SpyState.Spying:
                    uiText.text = "Red Spy: Spying";
                    break;
                case SpyState.SpeedBoost:
                    uiText.text = "Red Spy: Evading with Glitching through the Matrix";
                    break;
                case SpyState.Captured:
                    uiText.text = "Red Spy: Captured";
                    break;
                case SpyState.DestroyedDocument:
                    uiText.text = "Red Spy: Document Destroyed";
                    break;
            }
        }
    }
}