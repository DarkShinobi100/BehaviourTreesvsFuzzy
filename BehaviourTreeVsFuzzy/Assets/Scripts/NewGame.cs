using UnityEngine;

public class NewGame : MonoBehaviour
{
    [SerializeField]
    private Animator stateMachineAI;
    [SerializeField]
    bool FightHumanPlayer = false;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    bool FarFuzzyPlayer = false;
    [SerializeField]
    bool NearFuzzyPlayer = false;
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeFar;
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeNear;
    [SerializeField]
    private NewPlayer humanPlayer;
    [SerializeField]
    private NewPlayer aiPlayer;
    [SerializeField]
    private NewUI uiController;
    private int turn = 0;

    private void Awake()
    {
        if (!FightHumanPlayer)
        {
            FuzzyenemyBehaviorTreeFar.SetPlayerData(humanPlayer, aiPlayer);
            FuzzyenemyBehaviorTreeFar.onTreeExecuted += EndTurn;
            FuzzyenemyBehaviorTreeNear.SetPlayerData(aiPlayer, humanPlayer);
            FuzzyenemyBehaviorTreeNear.onTreeExecuted += EndTurn;
        }
        else
        {
            FuzzyenemyBehaviorTreeFar.SetPlayerData(humanPlayer, aiPlayer);
            FuzzyenemyBehaviorTreeFar.onTreeExecuted += EndTurn;
            playerController.onActionExecuted += EndTurn;
        }
    }

    public void EvaluateAITree()
    {
        if (!FarFuzzyPlayer)
        {
            FuzzyenemyBehaviorTreeFar.Evaluate();
        }
        else
        {
            FuzzyenemyBehaviorTreeFar.FuzzyEvaluate();
        }
    }

    public void EvaluateAITree2()
    {
        if (!NearFuzzyPlayer)
        {
            FuzzyenemyBehaviorTreeNear.Evaluate();
        }
        else
        {
            FuzzyenemyBehaviorTreeNear.FuzzyEvaluate();
        }
    }

    private void EndTurn()
    {
        if (humanPlayer.CurrentHealth <= 0 || aiPlayer.CurrentHealth <= 0)
        {
            if (!FightHumanPlayer)
            {
                stateMachineAI.SetTrigger("EndGame");
            }
            else
            {
                stateMachineAI.SetTrigger("EndGame");
            }
            uiController.EndGame();
            return;
        }
        if (!FightHumanPlayer)
        {
            FuzzyenemyBehaviorTreeNear.UpdateSprites();
            FuzzyenemyBehaviorTreeFar.UpdateSprites();

            stateMachineAI.SetTrigger("EndTurn");
        }
        else
        {
            FuzzyenemyBehaviorTreeNear.UpdateSprites();

            stateMachineAI.SetTrigger("EndTurn");
        }

        turn ^= 1;
        uiController.SetTurn(turn);
    }

    public void SetHumanplayer()
    {
        FightHumanPlayer = true;
    }

    public void SetFarFuzzy()
    {
        FarFuzzyPlayer = true;
    }

    public void SetNearFuzzy()
    {
        NearFuzzyPlayer = true;
    }
}
