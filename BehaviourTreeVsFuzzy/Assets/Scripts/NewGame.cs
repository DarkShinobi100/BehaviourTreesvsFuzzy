using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{//state machine for turn order
    [SerializeField]
    private Animator stateMachineAI;
    //bool for fighting a human
    [SerializeField]
    bool FightHumanPlayer = false;
    //players data
    [SerializeField]
    private PlayerController playerController;
    //if AI have fuzzy logic enabled
    [SerializeField]
    bool FarFuzzyPlayer = false;
    [SerializeField]
    bool NearFuzzyPlayer = false;
    //AI decision Trees
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeFar;
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeNear;
    //Both players
    [SerializeField]
    private NewPlayer humanPlayer;
    [SerializeField]
    private NewPlayer aiPlayer;
    //Ui controller for HUD
    [SerializeField]
    private NewUI uiController;
    private int turn = 0;

    //values for timers
    [SerializeField]
    private float Neartimer = 0.0f;
    [SerializeField]
    private float Fartimer = 0.0f;
    private float StartTimeNear = 0.0f;
    private float StartTimeFar = 0.0f;

    private void Start()
    {//if not fighting a human, set up AIs
        if (!FightHumanPlayer)
        {
            FuzzyenemyBehaviorTreeFar.SetPlayerData(humanPlayer, aiPlayer);
            FuzzyenemyBehaviorTreeFar.onTreeExecuted += EndTurn;
            FuzzyenemyBehaviorTreeNear.SetPlayerData(aiPlayer, humanPlayer);
            FuzzyenemyBehaviorTreeNear.onTreeExecuted += EndTurn;
        }
        else //only set up the far enemy and enable the buttons for the player
        {
            FuzzyenemyBehaviorTreeFar.SetPlayerData(humanPlayer, aiPlayer);
            FuzzyenemyBehaviorTreeFar.onTreeExecuted += EndTurn;
            playerController.onActionExecuted += EndTurn;
        }
    }

    public void EvaluateAITree()
    {//start timing from this call for the Far AI
        StartTimeFar = Time.time;
        //call the correct function for if using Fuzzy or not
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
    {//start timing from this call for the Far AI
        StartTimeNear = Time.time;
        //call the correct function for if using Fuzzy or not
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
    {//Turn end after decision has been made
        Neartimer = Time.time - StartTimeNear;
        Fartimer = Time.time - StartTimeFar;
        //check if player or AI has "Died"
        if (humanPlayer.CurrentHealth <= 0 || aiPlayer.CurrentHealth <= 0)
        {//trigger end game state
            stateMachineAI.SetTrigger("EndGame");
            uiController.EndGame();
            return;
        }
        if (!FightHumanPlayer) //if not fighting a human, update both trees values
        {
            FuzzyenemyBehaviorTreeNear.UpdateSprites();
            FuzzyenemyBehaviorTreeFar.UpdateSprites();
            stateMachineAI.SetTrigger("EndTurn");
        }
        else
        {//if fighting a human only update the far trees spites
            FuzzyenemyBehaviorTreeFar.UpdateSprites();
            stateMachineAI.SetTrigger("EndTurn");
        }
        //increase the turn on the UI controller
        turn ^= 1;
        uiController.SetTurn(turn);
    }
    //set if human is playing thegae
    public void SetHumanplayer()
    {
        FightHumanPlayer = true;
    }
    //Set if far AI is using fuzzy logic
    public void SetFarFuzzy()
    {
        FarFuzzyPlayer = true;
    }
    //Set if near AI is using fuzzy logic
    public void SetNearFuzzy()
    {
        NearFuzzyPlayer = true;
    }
    //reload the game
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     
            
    }
    //quit the game
    public void Quit()
    {
        Application.Quit();
    }
}
