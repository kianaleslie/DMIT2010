using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class RedSpy : MonoBehaviour
{
    [SerializeField] GameObject currentNode;
    [SerializeField] GameObject nextNode;
    [SerializeField] GameObject startNode;
    [SerializeField] GameObject destinationNode;
    [SerializeField] GameObject destinationNode2;
    [SerializeField] GameObject destinationNode3;
    [SerializeField] GameObject previousNode;
    [SerializeField] TMP_Text uiText;
    [SerializeField] float baseMovementSpeed;
    float currentMovementSpeed;
    float captureProbability = 0.6f;
    bool isCaptured = false;
    Keyboard kb;

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
        kb = Keyboard.current;
        currentNode = startNode;
        nextNode = currentNode;
        currentMovementSpeed = baseMovementSpeed;

        transform.position = currentNode.transform.position;
        UpdateUIText();
    }

    void Update()
    {
        if (isCaptured)
        {
            currentMovementSpeed = 0.0f;
            return;
        }
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
        if (kb.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Guard"))
        {
            if (currentState != SpyState.Captured)
            {
                Capture();
            }

            if (currentState == SpyState.SpeedBoost)
            {
                currentState = SpyState.SpeedBoost;
                ApplySpeedBoost();
                StartCoroutine(RemoveSpeedBoost(3.0f));
                UpdateUIText();
            }
        }
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
    void Capture()
    {
        bool isCaptured = Random.Range(0f, 1f) < captureProbability;

        if (isCaptured)
        {
            currentState = SpyState.Captured;
            UpdateUIText();

            StartCoroutine(DelayAfterCapture(10.0f));
        }
        else
        {
            ApplySpeedBoost();
            StartCoroutine(RemoveSpeedBoost(3.0f));
        }
    }
    IEnumerator DelayAfterCapture(float delayTime)
    {
        currentMovementSpeed = 0.0f;
        yield return new WaitForSeconds(delayTime);
        isCaptured = false;
        currentMovementSpeed = baseMovementSpeed;
        currentState = SpyState.Spying;
        UpdateUIText();
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