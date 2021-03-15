using UnityEngine;
using UnityEngine.UI;

public class NewUI : MonoBehaviour
{
    private const string playerTurnMessage = "Your turn";
    private const string aiTurnMessage = "Enemy's turn";
    private const string gameOverMessage = "GAME OVER";
    private const string ActionMessage = "New behaviour";
    private const string WaitMessage = "";
    private const string WinMessage = "You win!";
    private const string LoseMessage = "You Lose!";
    [SerializeField]
    private NewPlayer NearAIPlayer;
    [SerializeField]
    private NewPlayer FarAiPlayer;
    [SerializeField]
    private UpdatedEnemyBehaviorTree enemyBehaviorTreeFar;
    [SerializeField]
    private UpdatedEnemyBehaviorTree enemyBehaviorTreeNear;
    [SerializeField]
    bool Fuzzy = false;
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


    private void FixedUpdate()
    {
        UpdateSliders();
    }

    public void EndGame()
    {
        turnText.text = gameOverMessage;

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

    /* We change the turn message dpending on whose turn it is currently */
    public void SetTurn(int turnNumber)
    {
        if (turnNumber == 0)
        {
            turnText.text = playerTurnMessage;
            playerHealthText.text = ActionMessage;
            enemyHealthText.text = WaitMessage;

            if (!Fuzzy)
            {
                enemyBehaviorTreeFar.ResetSprites();
                enemyBehaviorTreeNear.UpdateSprites();
            }
            else
            {
                FuzzyenemyBehaviorTreeFar.ResetSprites();
                FuzzyenemyBehaviorTreeNear.UpdateSprites();
            }
        }
        else
        {
            turnText.text = aiTurnMessage;
            enemyHealthText.text = ActionMessage;
            playerHealthText.text = WaitMessage;
            if(!Fuzzy)
            {
                enemyBehaviorTreeNear.ResetSprites();
                enemyBehaviorTreeFar.UpdateSprites();
            }
            else
            {
                FuzzyenemyBehaviorTreeNear.ResetSprites();
                FuzzyenemyBehaviorTreeFar.UpdateSprites();
            }
        }
    }

    private void UpdateSliders()
    {
        //Near AI
        NearHealth.value = NearAIPlayer.CurrentHealth;
        NearMana.value = NearAIPlayer.CurrentMana;
        NearAttack.value = NearAIPlayer.CurrentAttack;
        NearDefence.value = NearAIPlayer.CurrentDefence;
        //Far AI
        FarHealth.value = FarAiPlayer.CurrentHealth;
        FarMana.value = FarAiPlayer.CurrentMana;
        FarAttack.value = FarAiPlayer.CurrentAttack;
        FarDefence.value = FarAiPlayer.CurrentDefence;

        if (Fuzzy)
        {
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

    }
}
