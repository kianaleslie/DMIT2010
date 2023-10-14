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
    public bool comboClaimed = false;

    private void Awake()
    {
        thisClaimButton = GetComponent<Button>();
    }

    public void Claim()
    {
        comboClaimed = true;
        buttonText.text = "Claimed";
        thisClaimButton.interactable = false;
    }
}