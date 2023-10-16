using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


//handles the UI elements related to the combinations
public class GoalGUIManager : MonoBehaviour
{

    [SerializeField] public ClaimButton[] goalButtons;
    public static GoalGUIManager Instance;
    public DiceGameManager diceList;
    public AIScript ai; 

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
        //foreach (ClaimButton button in goalButtons)
        //{
        //    if (!button.comboClaimed)
        //    {
        //        button.GetComponent<Button>().interactable = true;
        //    }

        //}
    }
    
    #region -- CLAIMING COMBOS

    /// Create logic in each section that prevents you from claiming the combination before it's valid.  
    /// 

    public void TryClaimingThreeOfAKind()
    {
        ai.ClaimThreeOfAKind();
        //goalButtons[0].Claim();
    }

    public void TryClaimingFourOfAKind()
    {
        ai.ClaimFourOfAKind();
        //goalButtons[1].Claim();
    }

    public void TryClaimingSmallStraight()
    {
        ai.ClaimSmallStraight();
        //goalButtons[2].Claim();
    }

    public void TryClaimingLargeStraight()
    {
        ai.ClaimLargeStraight();
        //goalButtons[3].Claim();
    }

    public void TryClaimingTwoPairs()
    {
        ai.ClaimTwoPair();
        //goalButtons[4].Claim();
    }

    public void TryClaimingFullHouse()
    {
        ai.ClaimFullHouse();
        //goalButtons[5].Claim();
    }
    #endregion
}