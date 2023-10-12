using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


//handles the UI elements related to the game's goals and combinations
public class GoalGUIManager : MonoBehaviour
{

    [SerializeField]
    ClaimButton[] goalButtons;
    public static GoalGUIManager Instance;
    public DiceGameManager diceList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ProtectButtons()
    {
        foreach (ClaimButton button in goalButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }
    public void ReleaseButtons()
    {
        foreach (ClaimButton button in goalButtons)
        {
            if (!button.goalClaimed)
            {
                button.GetComponent<Button>().interactable = true;
            }

        }
    }
    #region -- CLAIMING COMBOS

    /// Create logic in each section that prevents you from claiming the combination before it's valid.  
    /// 

    public void TryClaimingThreeOfAKind()
    {
        if (!goalButtons[0].goalClaimed)
        {
            //call to DiceGameMangager to get current list of dice
            List<int> diceValues = diceList.DiceValuesList();

            bool validThreeOfAKind = false;
            for (int i = 1; i <= 6; i++)
            {
                int count = diceList.CountDiceWithValueList(diceValues, i);
                if (count >= 3)
                {
                    validThreeOfAKind = true;
                    break;
                }
            }
            if (validThreeOfAKind)
            {
                goalButtons[0].Claim();
                Debug.Log("Three of a Kind claimed!");
            }

            //else
            //{
            //    Debug.Log("Invalid Three of a Kind!"); 
            //}
        }
        //else
        //{
        //    Debug.Log("Three of a Kind already claimed!");
        //}
    }

    public void TryClaimingFourOfAKind()
    {
        if (!goalButtons[1].goalClaimed)
        {
            List<int> diceValues = diceList.DiceValuesList();

            bool validFourOfAKind = false;
            for (int i = 1; i <= 6; i++)
            {
                int count = diceList.CountDiceWithValueList(diceValues, i);
                if (count >= 4)
                {
                    validFourOfAKind = true;
                    break;
                }
            }
            if (validFourOfAKind)
            {
                goalButtons[1].Claim();
                Debug.Log("Four of a Kind claimed!");
            }
            // else
            // {
            //     Debug.Log("Invalid Four of a Kind!");
            // }
        }
        // else
        // {
        //     Debug.Log("Four of a Kind already claimed!");
        // }
    }

    public void TryClaimingSmallStraight()
    {
        if (!goalButtons[2].goalClaimed)
        {
            List<int> diceValues = diceList.DiceValuesList();
            bool validSmallStraight = CheckForSmallStraight(diceValues);

            if (validSmallStraight)
            {
                goalButtons[2].Claim();
                Debug.Log("Small Straight claimed!");
            }
            // else
            // {
            //     Debug.Log("Invalid Small Straight!");
            // }
        }
        // else
        // {
        //     Debug.Log("Small Straight already claimed!");
        // }
    }

    public void TryClaimingLargeStraight()
    {
        if (!goalButtons[3].goalClaimed)
        {
            List<int> diceValues = diceList.DiceValuesList();
            bool validLargeStraight = CheckForLargeStraight(diceValues);


            if (validLargeStraight)
            {
                goalButtons[3].Claim();
                Debug.Log("Large Straight claimed!");
            }
            // else
            // {
            //     Debug.Log("Invalid Large Straight!");
            // }
        }
        // else
        // {
        //     Debug.Log("Large Straight already claimed!");
        // }
    }

    public void TryClaimingTwoPairs()
    {
        if (!goalButtons[4].goalClaimed)
        {
           
            List<int> diceValues = diceList.DiceValuesList();
            bool validTwoPairs = CheckForTwoPairs(diceValues);

            if (validTwoPairs)
            {
                goalButtons[4].Claim();
                Debug.Log("Two Pairs claimed!");
            }
            // else
            // {
            //     Debug.Log("Invalid Two Pairs!");
            // }
        }
        // else
        // {
        //     Debug.Log("Two Pairs already claimed!");
        // }
    }

    public void TryClaimingFullHouse()
    {
        if (!goalButtons[5].goalClaimed)
        {
            List<int> diceValues = diceList.DiceValuesList();
            bool validFullHouse = CheckForFullHouse(diceValues);

            if (validFullHouse)
            {
                goalButtons[5].Claim();
                Debug.Log("Full House claimed!");
            }
            // else
            // {
            //     Debug.Log("Invalid Full House!");
            // }
        }
        // else
        // {
        //     Debug.Log("Full House already claimed!");
        // }
    }
    #endregion
    public bool CheckForSmallStraight(List<int> diceValues)
    {
        diceValues.Sort();

        for (int i = 0; i < diceValues.Count - 3; i++)
        {
            if (diceValues[i + 1] == diceValues[i] + 1 &&
                diceValues[i + 2] == diceValues[i] + 2 &&
                diceValues[i + 3] == diceValues[i] + 3)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckForLargeStraight(List<int> diceValues)
    {
        diceValues.Sort();

        bool isLargeStraight = true;

        //1 - 5
        for (int i = 0; i < 5; i++)
        {
            if (diceValues[i] != i + 1)
            {
                isLargeStraight = false;
                break;
            }
        }

        if (isLargeStraight)
        {
            return true;
        }

        //2 - 6
        isLargeStraight = true;
        for (int i = 0; i < 5; i++)
        {
            if (diceValues[i] != i + 2)
            {
                isLargeStraight = false;
                break;
            }
        }
        return isLargeStraight;
    }
    public bool CheckForTwoPairs(List<int> diceValues)
    {
        List<int> pairedValues = new List<int>();

        foreach (int value in diceValues)
        {
            if (pairedValues.Contains(value))
            {
                continue;
            }

            int count = diceValues.Count(v => v == value);

            if (count >= 2)
            {
                pairedValues.Add(value);
            }
        }
        return pairedValues.Count == 2;
    }
    private bool CheckForFullHouse(List<int> diceValues)
    {
        List<int> valueCounts = new List<int>();

        foreach (int value in diceValues)
        {
            valueCounts.Add(diceValues.Count(v => v == value));
        }

        bool threeOfAKind = valueCounts.Contains(3);
        bool twoOfAKind = valueCounts.Contains(2);

        return threeOfAKind && twoOfAKind;
    }
}