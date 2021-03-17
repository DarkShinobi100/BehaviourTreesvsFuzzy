using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private float Neartimer = 0.0f;
    [SerializeField]
    private float Fartimer = 0.0f;
    private float StartTimeNear = 0.0f;
    private float StartTimeFar = 0.0f;

    private void Start()
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
        StartTimeFar = Time.time;
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
        StartTimeNear = Time.time;
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
        Neartimer = Time.time - StartTimeNear;
        Fartimer = Time.time - StartTimeFar;
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
            FuzzyenemyBehaviorTreeFar.UpdateSprites();

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

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     
            
    }

    public void Quit()
    {
        Application.Quit();
    }
}
