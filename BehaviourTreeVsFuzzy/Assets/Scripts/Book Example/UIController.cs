using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private const string playerTurnMessage = "Your turn";
    private const string aiTurnMessage = "Enemy's turn";
    private const string gameOverMessage = "GAME OVER";

    [SerializeField]
    private Player humanPlayer;
    [SerializeField]
    private Player aiPlayer;
    [SerializeField]
    private NewPlayer NewHumanPlayer;
    [SerializeField]
    private NewPlayer NewAiPlayer;
    [SerializeField]
    private Text turnText;
    [SerializeField]
    private Text playerHealthText;
    [SerializeField]
    private Text enemyHealthText;
    [SerializeField]
    private GameObject playerControls;
    [SerializeField]
    private bool HumanPlayer;

    private void FixedUpdate()
    {
        if (!HumanPlayer)
        {
            playerHealthText.text = "Hit points " + NewHumanPlayer.CurrentHealth.ToString();
            enemyHealthText.text = "Hit points " + NewAiPlayer.CurrentHealth.ToString();
        }
        else
        {
            playerHealthText.text = "Hit points " + humanPlayer.CurrentHealth.ToString();
            enemyHealthText.text = "Hit points " + aiPlayer.CurrentHealth.ToString();
        }       
    }

    public void EndGame() {
        turnText.text = gameOverMessage;
    }

    /* We change the controls and turn message dpending on whose turn it is currently */
    public void SetTurn(int turnNumber) {
        if(turnNumber == 0) {
            turnText.text = playerTurnMessage;
            SetPlayerControlState(true);
        } else {
            turnText.text = aiTurnMessage;
            SetPlayerControlState(false);
        }
    }

    /* We disable/enable the player controls based on whose turn it is currently */
    private void SetPlayerControlState(bool active) {
        if (humanPlayer == true)
        {
            playerControls.SetActive(active);
        }
        else
        {
            playerControls.SetActive(false);
        }
    }
}
