using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    public DiceGameManager diceGameManager;
    public Dice[] diceList;

    //method to handle the AI's turn 
    public void AITurn()
    {
        //ai will roll the dice
        diceGameManager.Roll();

        //check the available combos
        bool twoPair = CheckTwoPair();
        bool threeOfAKind = CheckThreeOfAKind();
        bool fourOfAKind = CheckFourOfAKind();
        bool fullHouse = CheckFullHouse();
        bool smallStraight = CheckSmallStraight();
        bool largeStraight = CheckLargeStraight();

        //keep pairs after performing the nessecary checks
        if (twoPair || threeOfAKind || fourOfAKind || fullHouse)
        {
            KeepPairs();
        }

        //keep straights after performing the nessecary checks
        if (smallStraight || largeStraight)
        {
            KeepStraights();
        }

        //claim available combos 
        if (twoPair)
        {
            ClaimTwoPair();
        }
        else
            if (threeOfAKind)
        {
            ClaimThreeOfAKind();
        }
        else
            if (fourOfAKind)
        {
            ClaimFourOfAKind();
        }
        else
            if (fullHouse)
        {
            ClaimFullHouse();
        }
        else
            if (smallStraight)
        {
            ClaimSmallStraight();
        }
        else
            if (largeStraight)
        {
            ClaimLargeStraight();
        }

        //end turn
        diceGameManager.PassTurn();
    }

    //methods to check each combo
    private bool CheckTwoPair()
    {
        //count each face on the dice
        int[] faceCount = new int[diceList.Length];

        foreach (Dice die in diceList)
        {
            //-1 for the array index 
            int faceValue = die.dieValue - 1;
            faceCount[faceValue]++;
        }

        //checking if there are two pairs in the array 
        int pair = 0;
        for (int index = 0; index < diceList.Length; index++)
        {
            if (faceCount[index] >= 2)
            {
                pair++;
            }
        }
        return pair == 2;
    }
    private bool CheckThreeOfAKind()
    {
        int[] faceCount = new int[diceList.Length];

        foreach (Dice die in diceList)
        {
            int faceValue = die.dieValue - 1;
            faceCount[faceValue]++;
        }
        for (int index = 0; index < diceList.Length; index++)
        {
            if (faceCount[index] >= 3)
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckFourOfAKind()
    {
        int[] faceCount = new int[diceList.Length];

        foreach (Dice die in diceList)
        {
            int faceValue = die.dieValue - 1;
            faceCount[faceValue]++;
        }
        for (int index = 0; index < diceList.Length; index++)
        {
            if (faceCount[index] >= 4)
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckFullHouse()
    {
        int[] faceCount = new int[diceList.Length];

        foreach (Dice die in diceList)
        {
            int faceValue = die.dieValue - 1;
            faceCount[faceValue]++;
        }

        bool threePair = false;
        bool twoPair = false;
        for (int index = 0; index < diceList.Length; index++)
        {
            if (faceCount[index] >= 3)
            {
                threePair = true;
            }
            if (faceCount[index] >= 2)
            {
                twoPair = true;
            }
        }
        return threePair && twoPair;
    }
    private bool CheckSmallStraight()
    {
        List<int> diceValue = new List<int>();

        foreach (Dice die in diceList)
        {
            diceValue.Add(die.dieValue);
        }

        diceValue.Sort();
        //diceValue.Count - 3 makes sure that there are at least 4 dice values to check
        for (int index = 0; index < diceValue.Count - 3; index++)
        {
            //check if there is at least 4 numbers in order 
            if (diceValue[index + 1] == diceValue[index] + 1 &&
                diceValue[index + 2] == diceValue[index] + 2 &&
                diceValue[index + 3] == diceValue[index] + 3)
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckLargeStraight()
    {
        //List<int> diceValue = new List<int>();

        //foreach (Dice die in diceList)
        //{
        //    diceValue.Add(die.dieValue);
        //}

        //for (int index = 0; index < diceList.Length; index++)
        //{
        //    diceValue[index] = diceList[index].dieValue;
        //}

        //diceValue.Sort();
        //bool isLargeStraight = true;

        ////check large straight 1-5
        //for (int index = 0; index < 5; index++)
        //{
        //    if (diceValue[index] != index + 1)
        //    {
        //        isLargeStraight = false;
        //        break;
        //    }
        //}
        //if (isLargeStraight)
        //{
        //    return true;
        //}

        ////check 2-6
        //isLargeStraight = true;
        //for (int index = 0; index < 5; index++)
        //{
        //    if (diceValue[index] != index + 2)
        //    {
        //        isLargeStraight = false;
        //        break;
        //    }
        //}
        //return isLargeStraight;
        List<int> diceValue = new List<int>();

        foreach (Dice die in diceList)
        {
            diceValue.Add(die.dieValue);
        }

        diceValue.Sort();
        for (int index = 0; index < diceValue.Count - 3; index++)
        {
            //check if there is at least 5 numbers in order 
            if (diceValue[index + 1] == diceValue[index] + 1 &&
                diceValue[index + 2] == diceValue[index] + 2 &&
                diceValue[index + 3] == diceValue[index] + 3 &&
                diceValue[index + 4] == diceValue[index] + 4)
            {
                return true;
            }
        }
        return false;
    }

    //methods to keep pairs and straights
    private void KeepPairs()
    {
        int[] faceCount = new int[diceList.Length];

        foreach (Dice die in diceList)
        {
            int faceValue = die.dieValue - 1;
            faceCount[faceValue]++;
        }
        for (int index = 0; index < diceList.Length; index++)
        {
            if (faceCount[index] >= 2)
            {
                DiceButton diceButton = diceList[index].GetComponent<DiceButton>();

                if (diceButton != null)
                {
                    diceButton.ToggleDice();
                }
            }
        }
    }
    private void KeepStraights()
    {

    }

    //methods to claim combos
    private void ClaimTwoPair()
    {

    }
    private void ClaimThreeOfAKind()
    {

    }
    private void ClaimFourOfAKind()
    {

    }
    private void ClaimFullHouse()
    {

    }
    private void ClaimSmallStraight()
    {

    }
    private void ClaimLargeStraight()
    {

    }
}

//Dice can be rolled for a random result.	

//Dice roll can be modified to produce a specific result.	

//Combo selections can be activated/deactivated. When deactivated that act as if they have been selected.	

//The combos will be activated when the dice roll supports the combo.	

//The AI can select each combo when it is active.	

//The AI will keep pairs when at least one of the following combos are available. Two Pair, Three of a Kind, Four of a Kind, Full House.	

//The AI will keep a run of three or four consecutive numbers when a straight is needed.	

//The AI will keep a run of three or four consecutive numbers with a gap.