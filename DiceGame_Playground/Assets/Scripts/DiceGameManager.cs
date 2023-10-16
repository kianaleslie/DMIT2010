using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


//handles dice rolls, tracking the player's score, and controlling the game's state
public class DiceGameManager : MonoBehaviour
{
    public Dice[] Dicelist;
    public DiceButton[] KeepDiceButtons;
    public static DiceGameManager Instance;
    public AIScript ai;

    public bool isRolling;

    public int rollCount = 0;
    public int score = 0;
    public int rollsLeft = 0;
    private int rollsMax = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            rollCount = 0;
            score = 0;
            rollsLeft = rollsMax;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(ai != null)
        {
            StartAIPlay();
        }
    }
    void StartAIPlay()
    {
        ai.AIPlay();
    }
    public void Roll()
    {
        if (!isRolling)
        {
            StartCoroutine(RollAllDice());
        }
    }
    IEnumerator RollAllDice()
    {
        isRolling = true;
        CheckRollsLeft();
        GoalGUIManager.Instance.ProtectButtons();
        for (int d = 0; d < Dicelist.Length; d++)
        {
            if (KeepDiceButtons[d].keepDice) 
            {
                continue;
            }
            else
            {
                Dicelist[d].RollToRandomSide();
            }
            yield return new WaitForSeconds(0.125f);
        }
        isRolling = false;
        GoalGUIManager.Instance.ReleaseButtons();
        rollCount += 1;
        StatsGUI.Instance.UpdateStatsGUI();
    }
    void CheckRollsLeft()
    {
        rollsLeft -= 1;
        if (rollsLeft < 0)
        {
            foreach (var d in KeepDiceButtons)
            {
                d.ResetDice();
            }
            rollsLeft = rollsMax;
        }
    }
}