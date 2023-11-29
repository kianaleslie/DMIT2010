using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlueSpy : MonoBehaviour
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
    [SerializeField] Collider detectionCollider;
    [SerializeField] Renderer spyRenderer;
    float currentMovementSpeed;
    float captureProbability = 0.6f;
    bool isCaptured = false;

    enum SpyState
    {
        Spying,
        Disguise,
        Captured,
        HoldDocument
    }
    SpyState currentState = SpyState.Spying;

    void Start()
    {
        currentMovementSpeed = movementSpeed;
        currentNode = startNode;
        nextNode = currentNode;

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
                transform.Translate((nextNode.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime);
            }
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
            if (currentState == SpyState.Disguise)
            {
                MakeSpyInvisible();
            }
                
        }
        if (other.CompareTag("Document"))
        {
            HoldDocument(other.gameObject);
        }
    }
    void MakeSpyInvisible()
    {
        if (spyRenderer != null)
        {
            spyRenderer.enabled = false;
        }
        if (detectionCollider != null)
        {
            detectionCollider.enabled = false;
        }
        currentState = SpyState.Disguise;
        UpdateUIText();
        StartCoroutine(WaitAfterInvisble());
    }

    void MakeSpyVisible()
    {
        if (spyRenderer != null)
        {
            spyRenderer.enabled = true;
        }
        if (detectionCollider != null)
        {
            detectionCollider.enabled = true;
        }
    }
    void HoldDocument(GameObject document)
    {
        Destroy(document);
        currentState = SpyState.HoldDocument;
        UpdateUIText();
        StartCoroutine(WaitAfterDocument());
    }
    void UpdateUIAfterDocument()
    {
        currentState = SpyState.Spying;
        UpdateUIText();
    }
    IEnumerator WaitAfterDocument()
    {
        yield return new WaitForSeconds(2.0f);
        UpdateUIAfterDocument();
    }
    void UpdateUIAfterInvisble()
    {
        currentState = SpyState.Spying;
        UpdateUIText();
        MakeSpyVisible();
    }
    IEnumerator WaitAfterInvisble()
    {
        yield return new WaitForSeconds(5.0f);
        UpdateUIAfterInvisble();
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
            MakeSpyInvisible();
            StartCoroutine(WaitAfterInvisble());
        }
    }
    IEnumerator DelayAfterCapture(float delayTime)
    {
        currentMovementSpeed = 0.0f;
        yield return new WaitForSeconds(delayTime);
        isCaptured = false;
        currentMovementSpeed = movementSpeed;
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
                    uiText.text = "Blue Spy: Spying";
                    break;
                case SpyState.Disguise:
                    uiText.text = "Blue Spy: Evading with Invisibility";
                    break;
                case SpyState.Captured:
                    uiText.text = "Blue Spy: Captured";
                    break;
                case SpyState.HoldDocument:
                    uiText.text = "Blue Spy: Holding Document";
                    break;
            }
        }
    }
}