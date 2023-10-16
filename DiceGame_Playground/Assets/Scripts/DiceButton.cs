using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


//controls the behaviour of a die
public class DiceButton : MonoBehaviour
{
    [SerializeField] Dice parentDice;
    [Tooltip("If true, the die will not roll when the roll button is pressed.")]
    public bool keepDice = false;

    [SerializeField] TMP_Text buttonText;
    [SerializeField] Button button;
    private void Awake()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        button.interactable = false;
    }

    public void ToggleDice()
    {
        if (keepDice)
        {
            keepDice = false;
        }
        else
        {
            keepDice = true;
        }
        buttonText.text = keepDice ? "Keep" : "Roll";
    }

    public void ResetDice()
    {
        keepDice = false;
        buttonText.text = "Roll";
    }
}