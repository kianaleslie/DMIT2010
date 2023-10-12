using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


//controls the behaviour of the buttons
public class ClaimButton : MonoBehaviour
{
    [SerializeField/*,HideInInspector*/] TMP_Text buttonText;
    [SerializeField/*,HideInInspector*/] Button thisClaimButton;
    public bool goalClaimed = false;

    private void Awake()
    {
        thisClaimButton = GetComponent<Button>();
    }

    public void Claim()
    {
        goalClaimed = true;
        buttonText.text = "Claimed";
        thisClaimButton.interactable = false;
    }
}