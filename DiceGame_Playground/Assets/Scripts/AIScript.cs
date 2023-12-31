using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    public DiceGameManager diceGameManager;
    public GoalGUIManager goalGUIManager;
    public Dice[] diceList;

    [SerializeField] public ClaimButton[] combosClaimed;

    public enum ComboType
    {
        TwoPair,
        ThreeOfAKind,
        FourOfAKind,
        FullHouse,
        SmallStraight,
        LargeStraight
    }
    //method to handle the AI 
    public void AIPlay()
    {
        //check the available combos
        bool twoPair = CheckTwoPair();
        bool threeOfAKind = CheckThreeOfAKind();
        bool fourOfAKind = CheckFourOfAKind();
        bool fullHouse = CheckFullHouse();
        bool smallStraight = CheckSmallStraight();
        bool largeStraight = CheckLargeStraight();


        //claim available combos 
        if (twoPair && !combosClaimed[(int)ComboType.TwoPair].comboClaimed)
        {
            ClaimTwoPair();
            Debug.Log("claim two pairs");
        }
        else
            if (threeOfAKind && !combosClaimed[(int)ComboType.ThreeOfAKind].comboClaimed)
        {
            ClaimThreeOfAKind();
            Debug.Log("claim 3 of a kind");
        }
        else
            if (fourOfAKind && !combosClaimed[(int)ComboType.FourOfAKind].comboClaimed)
        {
            ClaimFourOfAKind();
            Debug.Log("claim 4 of a kind");
        }
        else
            if (fullHouse && !combosClaimed[(int)ComboType.FullHouse].comboClaimed)
        {
            ClaimFullHouse();
            Debug.Log("claim full house");
        }
        else
            if (smallStraight && !combosClaimed[(int)ComboType.SmallStraight].comboClaimed)
        {
            ClaimSmallStraight();
            Debug.Log("claim small straight");
        }
        else
            if (largeStraight && !combosClaimed[(int)ComboType.LargeStraight].comboClaimed)
        {
            ClaimLargeStraight();
            Debug.Log("claim large straight");
        }

        //keep pairs after performing the nessecary checks
        if (twoPair || threeOfAKind || fourOfAKind || fullHouse)
        {
            KeepPairs();
        }

        //keep straights after performing the nessecary checks
        if (smallStraight || largeStraight)
        {
            KeepRuns();
        }
    }

    //methods to check each combo
    public bool CheckTwoPair()
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
    public bool CheckThreeOfAKind()
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
    public bool CheckFourOfAKind()
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
    public bool CheckFullHouse()
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
            if (faceCount[index] == 3)
            {
                threePair = true;
            }
            if (faceCount[index] == 2)
            {
                twoPair = true;
            }
        }
        return threePair && twoPair;
    }
    public bool CheckSmallStraight()
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
    public bool CheckLargeStraight()
    {
        List<int> diceValue = new List<int>();

        foreach (Dice die in diceList)
        {
            diceValue.Add(die.dieValue);
        }

        diceValue.Sort();

        if (diceValue.Count >= 5)
        {
            for (int index = 0; index < diceValue.Count - 4; index++)
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
        }
        return false;
    }

    //methods to keep pairs and straights
    public void KeepPairs()
    {
        List<int> faceCount = new List<int>(new int[diceList.Length]);

        foreach (Dice die in diceList)
        {
            int faceValue = die.dieValue - 1;
            faceCount[faceValue]++;
        }
        for (int index = 0; index < diceList.Length; index++)
        {
            if (faceCount[index] >= 2)
            {
                foreach (Dice die in diceList)
                {
                    if (die.dieValue - 1 == index)
                    {
                        Debug.Log("keep pairs");
                        DiceButton diceButton = die.GetComponent<DiceButton>();
                        if (diceButton != null)
                        {
                            diceButton.ToggleDice();
                        }
                    }
                }
            }
        }
    }
    public void KeepRuns()
    {
        List<int> diceValue = new List<int>();
        foreach (Dice die in diceList)
        {
            diceValue.Add(die.dieValue);
        }
        diceValue.Sort();

        int run = 1;
        for (int index = 0; index < diceValue.Count - 1; index++)
        {
            if (diceValue[index] == diceValue[index + 1] - 1)
            {
                run++;

                if (run >= 4)
                {
                    foreach (Dice die in diceList)
                    {
                        Debug.Log("keep small run");
                        DiceButton diceButton = die.GetComponent<DiceButton>();
                        if (diceButton != null)
                        {
                            diceButton.ToggleDice();
                        }
                    }
                }
                if (run >= 5)
                {
                    foreach (Dice die in diceList)
                    {
                        Debug.Log("keep large run");
                        DiceButton diceButton = die.GetComponent<DiceButton>();
                        if (diceButton != null)
                        {
                            diceButton.keepDice = true;
                        }

                    }
                }
            }
            else
            {
                run = 1;
            }
        }
    }

    //methods to claim combos
    public void ClaimTwoPair()
    {
        ClaimButton claimButton = GetClaimButton(ComboType.TwoPair);
        if (claimButton != null /*&& !combosClaimed[(int)ComboType.TwoPair]*/)
        {
            claimButton.Claim();
        }
    }
    public void ClaimThreeOfAKind()
    {
        ClaimButton claimButton = GetClaimButton(ComboType.ThreeOfAKind);
        if (claimButton != null /*&& !combosClaimed[(int)ComboType.ThreeOfAKind]*/)
        {
            claimButton.Claim();
        }
    }
    public void ClaimFourOfAKind()
    {
        ClaimButton claimButton = GetClaimButton(ComboType.FourOfAKind);
        if (claimButton != null /*&& !combosClaimed[(int)ComboType.FourOfAKind]*/)
        {
            claimButton.Claim();
        }
    }
    public void ClaimFullHouse()
    {
        ClaimButton claimButton = GetClaimButton(ComboType.FullHouse);
        if (claimButton != null /*&& !combosClaimed[(int)ComboType.FullHouse]*/)
        {
            claimButton.Claim();
        }
    }
    public void ClaimSmallStraight()
    {
        ClaimButton claimButton = GetClaimButton(ComboType.SmallStraight);
        if (claimButton != null /*&& !combosClaimed[(int)ComboType.SmallStraight]*/)
        {
            claimButton.Claim();
        }
    }
    public void ClaimLargeStraight()
    {
        ClaimButton claimButton = GetClaimButton(ComboType.LargeStraight);
        if (claimButton != null /*&& !combosClaimed[(int)ComboType.LargeStraight]*/)
        {
            claimButton.Claim();
        }
    }
    public ClaimButton GetClaimButton(ComboType comboType)
    {
        int index = -1;

        switch (comboType)
        {
            case ComboType.ThreeOfAKind:
                index = 0;
                break;
            case ComboType.FourOfAKind:
                index = 1;
                break;
            case ComboType.SmallStraight:
                index = 2;
                break;
            case ComboType.LargeStraight:
                index = 3;
                break;
            case ComboType.TwoPair:
                index = 4;
                break;
            case ComboType.FullHouse:
                index = 5;
                break;
        }

        if (index != -1 && index < goalGUIManager.goalButtons.Length)
        {
            return goalGUIManager.goalButtons[index];
        }
        return null;
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