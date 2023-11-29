using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen = true;
    private Vector3 originalPosition;
    private float moveDistance = 5.0f;

    [SerializeField] TMP_Text uiText;

    enum DoorState
    {
        Locked,
        Unlocked,
    }
    DoorState currentState = DoorState.Locked;

    private void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(MoveDoorTimer());
        UpdateUIText();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spy"))
        {
            isOpen = true;
        }
    }
    public bool IsOpen()
    {
        return isOpen;
    }
    public bool IsClosed()
    {
        return isOpen = false;
    }
    private IEnumerator MoveDoorTimer()
    {
        while (isOpen == true)
        {
            yield return new WaitForSeconds(7.0f);
            OpenDoor();
            yield return new WaitForSeconds(5.0f);
            CloseDoor();
        }
    }
    private void OpenDoor()
    {
        currentState = DoorState.Unlocked;
        UpdateUIText();
        transform.Translate(Vector3.forward * moveDistance);
    }
    private void CloseDoor()
    {
        currentState = DoorState.Locked;
        UpdateUIText();
        transform.position = originalPosition;
    }
    void UpdateUIText()
    {
        if (uiText != null)
        {
            switch (currentState)
            {
                case DoorState.Locked:
                    uiText.text = "Door: Locked";
                    break;
                case DoorState.Unlocked:
                    uiText.text = "Door: Unlocked";
                    break;
            }
        }
    }
}