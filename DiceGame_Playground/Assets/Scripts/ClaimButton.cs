using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


//controls the behaviour of the claim buttons
public class ClaimButton : MonoBehaviour
{
    [SerializeField/*,HideInInspector*/] TMP_Text buttonText;
    [SerializeField/*,HideInInspector*/] Button thisClaimButton;
    public bool comboClaimed = false;

    private void Awake()
    {
        thisClaimButton = GetComponent<Button>();
        thisClaimButton.gameObject.SetActive(false); 
    }

    public void Claim()
    {
        comboClaimed = true;
        thisClaimButton.gameObject.SetActive(true);
        buttonText.text = "Claimed";
        thisClaimButton.interactable = false;
    }
}