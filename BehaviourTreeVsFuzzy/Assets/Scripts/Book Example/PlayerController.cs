using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private NewPlayer ownData;
    [SerializeField]
    private NewPlayer enemyData;

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
    private GameObject PlayerButtons;

    [SerializeField]
    private AudioClip[] SFX = new AudioClip[6];
    private AudioSource audioPlayer;
    [SerializeField]
    private Text StateText;
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
        PlayerButtons.SetActive(true);

        BuffAttackButton.onClick.AddListener(BuffAttack);
        BuffDefenceButton.onClick.AddListener(BuffDefence);
        BuffManaButton.onClick.AddListener(BuffMana);
        healButton.onClick.AddListener(Heal);
        attackButton.onClick.AddListener(Attack);
    }

    private void Attack()
    {
        enemyData.Damage(ownData.CurrentAttack);
        ownData.DecreaseAttack();
        ownData.DecreaseMana(1);
        //animation for attacking
        animator.SetTrigger("Attack");
        StateText.text = "Direct Attack!";
        //sound effect
        audioPlayer.PlayOneShot(SFX[4]);
        EndTurn();
    }

    private void Heal()
    {
        ownData.DecreaseMana(5);
        ownData.Heal();

        //animation for healing
        animator.SetTrigger("Heal");
        StateText.text = "Heal!";

        //sound effect
        audioPlayer.PlayOneShot(SFX[0]);
        EndTurn();
    }

    private void BuffDefence()
    {
        ownData.DecreaseMana(2);
        ownData.increaseDefence();

        //animation for increasing Defence
        animator.SetTrigger("BuffDefence");
        StateText.text = "Increase Defence!";
        //sound effect
        audioPlayer.PlayOneShot(SFX[2]);
        EndTurn();
    }

    private void BuffAttack()
    {
        ownData.DecreaseMana(2);
        ownData.increaseAttack();

        //animation for increasing Attack
        animator.SetTrigger("BuffAttack");
        StateText.text = "Increase Attack!";
        //sound effect
        audioPlayer.PlayOneShot(SFX[3]);
        EndTurn();
    }

    private void BuffMana()
    {
        ownData.increaseMana();

        //animation for increasing mana
        animator.SetTrigger("BuffMana");

        StateText.text = "Increase mana!";
        //sound effect
        audioPlayer.PlayOneShot(SFX[1]);
        EndTurn();
    }

    private void EndTurn()
    {
        if(onActionExecuted != null)
        {
            onActionExecuted();
            PlayerButtons.SetActive(false);
        }
    }
}
