using UnityEngine;
using UnityEngine.UI;

public class NewUI : MonoBehaviour
{
    private const string playerTurnMessage = "Your turn";
    private const string aiTurnMessage = "Enemy's turn";
    private const string gameOverMessage = "GAME OVER";
    private const string ActionMessage = "New behaviour";
    private const string FuzzyMessage = "New Fuzzy behaviour";
    private const string WaitMessage = "";
    private const string WinMessage = "You win!";
    private const string LoseMessage = "You Lose!";
    [SerializeField]
    private NewPlayer NearAIPlayer;
    [SerializeField]
    private NewPlayer FarAiPlayer;
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeFar;
    [SerializeField]
    private FuzzyBehaviourScript FuzzyenemyBehaviorTreeNear;
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

    [SerializeField]
    private bool FuzzyNearAIPlayer;
    [SerializeField]
    private bool FuzzyFarAiPlayer;
    [SerializeField]
    private bool VsHuman;
    [SerializeField]
    private NewPlayer Humanplayer;

    [SerializeField]
    private GameObject PlayerButtons;

    private void FixedUpdate()
    {
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
    {
        turnText.text = gameOverMessage;

        if(!VsHuman)
        {
            if (NearAIPlayer.CurrentHealth <= 0)
            {
                playerHealthText.text = LoseMessage;
                enemyHealthText.text = WinMessage;
                AINearStateText.text = "";
            }
            else
            {
                playerHealthText.text = WinMessage;
                enemyHealthText.text = LoseMessage;
                AiFarStateText.text = "";
            }
        }
        else
        if (Humanplayer.CurrentHealth <= 0)
        {
            playerHealthText.text = LoseMessage;
            enemyHealthText.text = WinMessage;
            AINearStateText.text = "";
        }
        else
        {
            playerHealthText.text = WinMessage;
            enemyHealthText.text = LoseMessage;
            AiFarStateText.text = "";
        }


    }

    /* We change the turn message dpending on whose turn it is currently */
    public void SetTurn(int turnNumber)
    {
        if (turnNumber == 0)
        {
            if (FuzzyNearAIPlayer)
            {
                playerHealthText.text = FuzzyMessage;
            }
            else
            {
                playerHealthText.text = ActionMessage;
            }
            turnText.text = playerTurnMessage;
            enemyHealthText.text = WaitMessage;


            if(!VsHuman)
            {
                FuzzyenemyBehaviorTreeFar.ResetSprites();
                FuzzyenemyBehaviorTreeNear.UpdateSprites();
            }    
            else
            {
                PlayerButtons.SetActive(true);
            }
        }
        else
        {
            turnText.text = aiTurnMessage;
            if (FuzzyFarAiPlayer)
            {
                enemyHealthText.text = FuzzyMessage;
            }
            else
            {
                enemyHealthText.text = ActionMessage;
            }
            playerHealthText.text = WaitMessage;

            if(!VsHuman)
            {
                FuzzyenemyBehaviorTreeNear.ResetSprites();
            }
            else
            {
                PlayerButtons.SetActive(false);
            }
            FuzzyenemyBehaviorTreeFar.UpdateSprites();
        }
        if(VsHuman)
        {
            playerHealthText.text = WaitMessage;
            enemyHealthText.text = WaitMessage;
        }
    }

    private void UpdateSliders()
    {

        NearHealth.value = NearAIPlayer.CurrentHealth;
        NearMana.value = NearAIPlayer.CurrentMana;
        NearAttack.value = NearAIPlayer.CurrentAttack;
        NearDefence.value = NearAIPlayer.CurrentDefence;


        //Far AI
        FarHealth.value = FarAiPlayer.CurrentHealth;
        FarMana.value = FarAiPlayer.CurrentMana;
        FarAttack.value = FarAiPlayer.CurrentAttack;
        FarDefence.value = FarAiPlayer.CurrentDefence;

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

        NearHealth.value = Humanplayer.CurrentHealth;
        NearMana.value = Humanplayer.CurrentMana;
        NearAttack.value = Humanplayer.CurrentAttack;
        NearDefence.value = Humanplayer.CurrentDefence;


        //Far AI
        FarHealth.value = FarAiPlayer.CurrentHealth;
        FarMana.value = FarAiPlayer.CurrentMana;
        FarAttack.value = FarAiPlayer.CurrentAttack;
        FarDefence.value = FarAiPlayer.CurrentDefence;

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

    public void SetFuzzyNear()
    {
        FuzzyNearAIPlayer = true;
    }
    public void SetFuzzyFar()
    {
        FuzzyFarAiPlayer = true;
    }
    public void setHuman()
    {
        VsHuman = true;
    }
}
