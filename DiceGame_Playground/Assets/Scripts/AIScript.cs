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
        if(twoPair || threeOfAKind || fourOfAKind || fullHouse)
        {
            KeepPairs();
        }

        //keep straights after performing the nessecary checks
        if(smallStraight || largeStraight)
        {
            KeepStraights();
        }

        //claim available combos 
        if(twoPair)
        {
            ClaimTwoPair();
        }
        else
            if(threeOfAKind)
        {
            ClaimThreeOfAKind();
        }
        else
            if(fourOfAKind)
        {
            ClaimFourOfAKind();
        }
        else
            if(fullHouse)
        {
            ClaimFullHouse();
        }
        else
            if(smallStraight)
        {
            ClaimSmallStraight();
        }
        else 
            if(largeStraight)
        {
            ClaimLargeStraight();
        }

        //end turn
        diceGameManager.PassTurn();
    }
    
    //methods to check each combo
    private bool CheckTwoPair()
    {
        return true;
    }
    private bool CheckThreeOfAKind()
    {
        return true;
    }
    private bool CheckFourOfAKind()
    {
        return true;
    }
    private bool CheckFullHouse()
    {
        return true;
    }
    private bool CheckSmallStraight()
    {
        return true;
    }
    private bool CheckLargeStraight()
    {
        return true;
    }

    //methods to keep pairs and straights
    private void KeepPairs()
    {

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