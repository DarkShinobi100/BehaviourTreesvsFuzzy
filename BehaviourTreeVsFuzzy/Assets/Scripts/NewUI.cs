using UnityEngine;
using UnityEngine.UI;

public class NewUI : MonoBehaviour
{
    //battle messages
    private const string playerTurnMessage = "Your turn";
    private const string aiTurnMessage = "Enemy's turn";
    private const string gameOverMessage = "GAME OVER\n amount of Turns: ";
    private const string ActionMessage = "New behaviour";
    private const string FuzzyMessage = "New Fuzzy behaviour";
    private const string WaitMessage = "";
    private const string WinMessage = "You win!";
    private const string LoseMessage = "You Lose!";
    //players
    [SerializeField]
    private NewPlayer NearAIPlayer;
    [SerializeField]
    private NewPlayer FarAiPlayer;
    //enemy behaviour trees
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeFar;
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeNear;
    //game text
    [SerializeField]
    private Text turnText;
    [SerializeField]
    private Text playerHealthText;
    [SerializeField]
    private Text enemyHealthText;
    [SerializeField]
    private Text AINearStateText;
    [SerializeField]
    private Text AiFarStateText;

    //user sliders
    [SerializeField]
    private Slider NearHealth;
    [SerializeField]
    private Slider NearMana;
    [SerializeField]
    private Slider NearAttack;
    [SerializeField]
    private Slider NearDefence;
    [SerializeField]
    private Slider FarHealth;
    [SerializeField]
    private Slider FarMana;
    [SerializeField]
    private Slider FarAttack;
    [SerializeField]
    private Slider FarDefence;

    //Fuzzy sliders
    [SerializeField]
    private Slider FarFuzzyHealthX;
    [SerializeField]
    private Slider FarFuzzyHealthY;
    [SerializeField]
    private Slider FarFuzzyHealthZ;
    [SerializeField]
    private Slider FarFuzzyManaX;
    [SerializeField]
    private Slider FarFuzzyManaY;
    [SerializeField]
    private Slider FarFuzzyManaZ;
    [SerializeField]
    private Slider FarFuzzyAttackX;
    [SerializeField]
    private Slider FarFuzzyAttackY;
    [SerializeField]
    private Slider FarFuzzyAttackZ;
    [SerializeField]
    private Slider FarFuzzyDefenceX;
    [SerializeField]
    private Slider FarFuzzyDefenceY;
    [SerializeField]
    private Slider FarFuzzyDefenceZ;

    [SerializeField]
    private Slider NearFuzzyHealthX;
    [SerializeField]
    private Slider NearFuzzyHealthY;
    [SerializeField]
    private Slider NearFuzzyHealthZ;
    [SerializeField]
    private Slider NearFuzzyManaX;
    [SerializeField]
    private Slider NearFuzzyManaY;
    [SerializeField]
    private Slider NearFuzzyManaZ;
    [SerializeField]
    private Slider NearFuzzyAttackX;
    [SerializeField]
    private Slider NearFuzzyAttackY;
    [SerializeField]
    private Slider NearFuzzyAttackZ;
    [SerializeField]
    private Slider NearFuzzyDefenceX;
    [SerializeField]
    private Slider NearFuzzyDefenceY;
    [SerializeField]
    private Slider NearFuzzyDefenceZ;

    //toggle for AI players
    [SerializeField]
    private bool FuzzyNearAIPlayer;
    [SerializeField]
    private bool FuzzyFarAiPlayer;
    //player data
    [SerializeField]
    private bool VsHuman;
    [SerializeField]
    private NewPlayer Humanplayer;

    //player buttons for gameplay
    [SerializeField]
    private GameObject PlayerButtons;
    [SerializeField]
    private GameObject QuitButton;
    [SerializeField]
    private GameObject ResetButton;

    //animators
    [SerializeField]
    private Animator NearAnimator;
    [SerializeField]
    private Animator FarAnimator;

    private int TurnCount = 0;

    private void FixedUpdate()
    {//update sliders values
        if(!VsHuman)
        {
            UpdateSliders();
        }
        else
        {
            UpdateHumanSliders();
        }
    }

    public void EndGame()
    {//end of the game, Display gameover message and options
        turnText.text = gameOverMessage + TurnCount.ToString();
        QuitButton.SetActive(true);
        ResetButton.SetActive(true);

        //Not fighting a player
        if (!VsHuman)
        {//left player has died
            if (NearAIPlayer.CurrentHealth <= 0)
            {
                playerHealthText.text = LoseMessage;
                NearAnimator.SetTrigger("Lose");
                enemyHealthText.text = WinMessage;
                FarAnimator.SetTrigger("Win");
                AINearStateText.text = "";
            }
            else //right player has died
            {
                playerHealthText.text = WinMessage;
                NearAnimator.SetTrigger("Win");
                enemyHealthText.text = LoseMessage;
                FarAnimator.SetTrigger("Lose");
                AiFarStateText.text = "";
            }
        }
        else//we are fighting a player
        if (Humanplayer.CurrentHealth <= 0)
        {//human has lost the battle
            playerHealthText.text = LoseMessage;
            NearAnimator.SetTrigger("Lose");
            enemyHealthText.text = WinMessage;
            FarAnimator.SetTrigger("Win");
            AINearStateText.text = "";
        }
        else//Ai has lost
        {
            playerHealthText.text = WinMessage;
            NearAnimator.SetTrigger("Win");
            enemyHealthText.text = LoseMessage;
            FarAnimator.SetTrigger("Lose");
            AiFarStateText.text = "";
        }            
    }

    // Change turn message based on whose acting
    public void SetTurn(int turnNumber)
    {//increase turn count
        TurnCount++;
        //left player turn
        if (turnNumber == 0)
        {
            //if its a fuzzy AI, use new message
            if (FuzzyNearAIPlayer)
            {
                playerHealthText.text = FuzzyMessage;
            }
            else
            {//use default message
                playerHealthText.text = ActionMessage;
            }
            turnText.text = playerTurnMessage;
            enemyHealthText.text = WaitMessage;


            if(!VsHuman)//not a human playing
            {//reset the sprites for the tree
                FuzzyenemyBehaviorTreeFar.ResetSprites();
                FuzzyenemyBehaviorTreeNear.UpdateSprites();
            }    
            else
            {//enable the players buttons
                PlayerButtons.SetActive(true);
            }
        }
        else
        {//Right player turn
            turnText.text = aiTurnMessage;
            //if its a fuzzy AI, use new message
            if (FuzzyFarAiPlayer)
            {
                enemyHealthText.text = FuzzyMessage;
            }
            else
            {//use default message
                enemyHealthText.text = ActionMessage;
            }
            playerHealthText.text = WaitMessage;

            if(!VsHuman)//not a human playing
            {//reset the sprites for the tree
                FuzzyenemyBehaviorTreeNear.ResetSprites();
            }
            else
            {//disable the players buttons
                PlayerButtons.SetActive(false);
            }
            FuzzyenemyBehaviorTreeFar.UpdateSprites();
        }
        if(VsHuman)
        {//if human playing disable the text
            playerHealthText.text = WaitMessage;
            enemyHealthText.text = WaitMessage;
        }
    }

    private void UpdateSliders()
    {
        //update sliders values for left player
        NearHealth.value = NearAIPlayer.CurrentHealth;
        NearMana.value = NearAIPlayer.CurrentMana;
        NearAttack.value = NearAIPlayer.CurrentAttack;
        NearDefence.value = NearAIPlayer.CurrentDefence;

        //update sliders values for right player
        FarHealth.value = FarAiPlayer.CurrentHealth;
        FarMana.value = FarAiPlayer.CurrentMana;
        FarAttack.value = FarAiPlayer.CurrentAttack;
        FarDefence.value = FarAiPlayer.CurrentDefence;

        //Fuzzy values for Right AI
        FarFuzzyHealthX.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentHealth / FarAiPlayer.MaxHealth).x;
        FarFuzzyHealthY.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentHealth / FarAiPlayer.MaxHealth).y;
        FarFuzzyHealthZ.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentHealth / FarAiPlayer.MaxHealth).z;

        FarFuzzyManaX.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentMana / FarAiPlayer.MaxMana).x;
        FarFuzzyManaY.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentMana / FarAiPlayer.MaxMana).y;
        FarFuzzyManaZ.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentMana / FarAiPlayer.MaxMana).z;

        FarFuzzyDefenceX.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentDefence / 15.0f).x;
        FarFuzzyDefenceY.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentDefence / 15.0f).y;
        FarFuzzyDefenceZ.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentDefence / 15.0f).z;

        FarFuzzyAttackX.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentAttack / 15.0f).x;
        FarFuzzyAttackY.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentAttack / 15.0f).y;
        FarFuzzyAttackZ.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentAttack / 15.0f).z;

        //Fuzzy values for left AI
        NearFuzzyHealthX.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentHealth / NearAIPlayer.MaxHealth).x;
        NearFuzzyHealthY.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentHealth / NearAIPlayer.MaxHealth).y;
        NearFuzzyHealthZ.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentHealth / NearAIPlayer.MaxHealth).z;

        NearFuzzyManaX.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentMana / NearAIPlayer.MaxMana).x;
        NearFuzzyManaY.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentMana / NearAIPlayer.MaxMana).y;
        NearFuzzyManaZ.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentMana / NearAIPlayer.MaxMana).z;

        NearFuzzyDefenceX.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentDefence / 15.0f).x;
        NearFuzzyDefenceY.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentDefence / 15.0f).y;
        NearFuzzyDefenceZ.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentDefence / 15.0f).z;

        NearFuzzyAttackX.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentAttack / 15.0f).x;
        NearFuzzyAttackY.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentAttack / 15.0f).y;
        NearFuzzyAttackZ.value = FuzzyenemyBehaviorTreeNear.BasicFuzzy(NearAIPlayer.CurrentAttack / 15.0f).z;

    }
    private void UpdateHumanSliders()
    {
        //update sliders values for left player
        NearHealth.value = Humanplayer.CurrentHealth;
        NearMana.value = Humanplayer.CurrentMana;
        NearAttack.value = Humanplayer.CurrentAttack;
        NearDefence.value = Humanplayer.CurrentDefence;
        
        //update sliders values for right player
        FarHealth.value = FarAiPlayer.CurrentHealth;
        FarMana.value = FarAiPlayer.CurrentMana;
        FarAttack.value = FarAiPlayer.CurrentAttack;
        FarDefence.value = FarAiPlayer.CurrentDefence;

        //Fuzzy values for Right AI
        FarFuzzyHealthX.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentHealth / FarAiPlayer.MaxHealth).x;
        FarFuzzyHealthY.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentHealth / FarAiPlayer.MaxHealth).y;
        FarFuzzyHealthZ.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentHealth / FarAiPlayer.MaxHealth).z;

        FarFuzzyManaX.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentMana / FarAiPlayer.MaxMana).x;
        FarFuzzyManaY.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentMana / FarAiPlayer.MaxMana).y;
        FarFuzzyManaZ.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentMana / FarAiPlayer.MaxMana).z;

        FarFuzzyDefenceX.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentDefence / 15.0f).x;
        FarFuzzyDefenceY.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentDefence / 15.0f).y;
        FarFuzzyDefenceZ.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentDefence / 15.0f).z;

        FarFuzzyAttackX.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentAttack / 15.0f).x;
        FarFuzzyAttackY.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentAttack / 15.0f).y;
        FarFuzzyAttackZ.value = FuzzyenemyBehaviorTreeFar.BasicFuzzy(FarAiPlayer.CurrentAttack / 15.0f).z;

        //Fuzzy values for player
        NearFuzzyHealthX.value = 1.0f; 
        NearFuzzyHealthY.value = 1.0f; 
        NearFuzzyHealthZ.value = 1.0f;  

        NearFuzzyManaX.value = 1.0f;
        NearFuzzyManaY.value = 1.0f;  
        NearFuzzyManaZ.value = 1.0f;

        NearFuzzyDefenceX.value = 1.0f;
        NearFuzzyDefenceY.value = 1.0f;
        NearFuzzyDefenceZ.value = 1.0f;

        NearFuzzyAttackX.value = 1.0f;
        NearFuzzyAttackY.value = 1.0f;
        NearFuzzyAttackZ.value = 1.0f;

    }
    //sets if near AI is fuzzy
    public void SetFuzzyNear()
    {
        FuzzyNearAIPlayer = true;
    }
    //sets if far AI is fuzzy
    public void SetFuzzyFar()
    {
        FuzzyFarAiPlayer = true;
    }
    //set if human playing
    public void setHuman()
    {
        VsHuman = true;
    }
}
