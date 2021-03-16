using UnityEngine;

public class NewGame : MonoBehaviour
{
    [SerializeField]
    private Animator stateMachine;
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
        FuzzyenemyBehaviorTreeFar.SetPlayerData(humanPlayer, aiPlayer);
        FuzzyenemyBehaviorTreeFar.onTreeExecuted += EndTurn;
        FuzzyenemyBehaviorTreeNear.SetPlayerData(aiPlayer, humanPlayer);
        FuzzyenemyBehaviorTreeNear.onTreeExecuted += EndTurn;
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
            stateMachine.SetTrigger("EndGame");
            uiController.EndGame();
            return;
        }

        FuzzyenemyBehaviorTreeNear.UpdateSprites();
        FuzzyenemyBehaviorTreeFar.UpdateSprites();

        stateMachine.SetTrigger("EndTurn");
        turn ^= 1;
        uiController.SetTurn(turn);
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
