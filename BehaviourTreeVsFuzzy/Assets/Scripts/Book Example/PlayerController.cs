using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private NewPlayer ownData;
    [SerializeField]
    private NewPlayer enemyData;
    [SerializeField]
    private Text EnemyStateText;

    [Header("Buttons")]
    [SerializeField]
    private Button BuffAttackButton;
    [SerializeField]
    private Button BuffDefenceButton;
    [SerializeField]
    private Button BuffManaButton;
    [SerializeField]
    private Button healButton;
    [SerializeField]
    private Button attackButton;

    [SerializeField]
    private AudioClip[] SFX = new AudioClip[6];
    private AudioSource audioPlayer;
    private Animator animator;

    public delegate void ActionExecuted();
    public event ActionExecuted onActionExecuted;

    private void Start()
    {
        //own animator
        //Get the Animator attached to the GameObject you are intending to animate.
        animator = gameObject.GetComponent<Animator>();

        //Get the AudioSource attached to the GameObject
        audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    private void Awake()
    {
        BuffAttackButton.onClick.AddListener(BuffAttack);
        BuffDefenceButton.onClick.AddListener(BuffDefence);
        BuffManaButton.onClick.AddListener(BuffMana);
        healButton.onClick.AddListener(Heal);
        attackButton.onClick.AddListener(Attack);
    }

    private void Attack()
    {
        EnemyStateText.text = "";
        enemyData.Damage(ownData.CurrentAttack);
        ownData.DecreaseAttack();
        ownData.DecreaseMana(1);
        //animation for attacking
        animator.SetTrigger("Attack");
        //sound effect
        audioPlayer.PlayOneShot(SFX[4]);
        EndTurn();
    }

    private void Heal()
    {
        EnemyStateText.text = "";
        ownData.DecreaseMana(5);
        ownData.Heal();

        //animation for healing
        animator.SetTrigger("Heal");

        //sound effect
        audioPlayer.PlayOneShot(SFX[0]);
        EndTurn();
    }

    private void BuffDefence()
    {
        EnemyStateText.text = "";
        ownData.DecreaseMana(2);
        ownData.increaseDefence();

        //animation for increasing Defence
        animator.SetTrigger("BuffDefence");
        //sound effect
        audioPlayer.PlayOneShot(SFX[2]);
        EndTurn();
    }

    private void BuffAttack()
    {
        EnemyStateText.text = "";
        ownData.DecreaseMana(2);
        ownData.increaseAttack();

        //animation for increasing Attack
        animator.SetTrigger("BuffAttack");
        //sound effect
        audioPlayer.PlayOneShot(SFX[3]);
        EndTurn();
    }

    private void BuffMana()
    {
        EnemyStateText.text = "";
        ownData.increaseMana();

        //animation for increasing mana
        animator.SetTrigger("BuffMana");

        //sound effect
        audioPlayer.PlayOneShot(SFX[1]);
        EndTurn();
    }

    private void EndTurn()
    {
        if(onActionExecuted != null)
        {
            onActionExecuted();
        }
    }
}
