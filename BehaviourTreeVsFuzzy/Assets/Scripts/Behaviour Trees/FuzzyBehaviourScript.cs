using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FuzzyBehaviourScript : MonoBehaviour
{
    private NewPlayer playerData;
    private NewPlayer ownData;
    private Animator animator;

    [SerializeField]
    bool HighSpeed = false;
    [SerializeField]
    private AudioClip[] SFX = new AudioClip[6];
    private AudioSource audioPlayer;
    [SerializeField]
    private Text StateText;


    // Spritees for Visuals
    //attack buff sequence
    [SerializeField]
    private Image AttackCheckNodeSprite;
    [SerializeField]
    private Image AttackValueCheckNodeSprite;

    //defence buff sequence
    [SerializeField]
    private Image DefenceCheckNodeSprite;
    [SerializeField]
    private Image DefenceValueCheckNodeSprite;

    //mana buff sequence
    [SerializeField]
    private Image ManaCheckNodeSprite;
    [SerializeField]
    private Image ManaValueCheckNodeSprite;

    //layer 2
    [SerializeField]
    private Image AttackCheckSequenceSprite;
    [SerializeField]
    private Image DefenceCheckSequenceSprite;
    [SerializeField]
    private Image ManaCheckSequenceSprite;

    //health sequence
    [SerializeField]
    private Image ManaCheckHealthNodeSprite;
    [SerializeField]
    private Image HealthCheckNodeSprite;

    //layer 1
    [SerializeField]
    private Image AttackPlayerNodeSprite;
    [SerializeField]
    private Image BuffSelectorNodeSprite;
    [SerializeField]
    private Image HealthCheckSequenceSprite;

    //root
    [SerializeField]
    private Image rootNodeSprite;



    //layer 3
    //Attack Buff Sequence
    public ActionNode AttackCheckNode;
    public ActionNode AttackValueCheckNode;

    //Defence Buff Sequence
    public ActionNode DefenceCheckNode;
    public ActionNode DefenceValueCheckNode;

    //Mana Buff Sequence
    public ActionNode ManaCheckNode;
    public ActionNode ManaValueCheckNode;

    //layer 2
    public Sequence AttackCheckSequence;
    public Sequence DefenceCheckSequence;
    public Sequence ManaCheckSequence;
    //Health Sequence
    public ActionNode ManaCheckHealthNode;
    public ActionNode HealthCheckNode;

    //layer 1
    public ActionNode AttackPlayerNode;
    public Selector BuffSelectorNode;
    public Sequence HealthCheckSequence;

    //root
    public Selector rootNode;

    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExecuted;

    public delegate void NodePassed(string trigger);


    public AnimationCurve critical;
    public AnimationCurve hurt;
    public AnimationCurve healthy;

    private bool HealthRisk = false;
    private bool ManaRisk = false;
    private bool DefenceRisk = false;
    private bool AttackRisk = false;

    private bool NoRisk = true;

    void Start()
    {
        //Check low health, if its low it will decide to heal
        ManaCheckHealthNode = new ActionNode(CanUseMana);
        HealthCheckNode = new ActionNode(CriticalHealthCheck);
        HealthCheckSequence = new Sequence(new List<Node> {
            ManaCheckHealthNode,
            HealthCheckNode,
        });
        //          if cannot afford to heal it will regen mana

        //Check low Mana, if its low it will decide to buff
        ManaCheckNode = new ActionNode(CheckMana);
        ManaValueCheckNode = new ActionNode(CheckMana);
        ManaCheckSequence = new Sequence(new List<Node> {
            ManaCheckNode,
            ManaValueCheckNode,
        });


        //Check low Defence, if its low it will decide to buff
        DefenceCheckNode = new ActionNode(CanUseMana);
        DefenceValueCheckNode = new ActionNode(CheckDefence);
        DefenceCheckSequence = new Sequence(new List<Node> {
            DefenceCheckNode,
            DefenceValueCheckNode,
        });


        //Check low Defence, if its low it will decide to buff
        AttackCheckNode = new ActionNode(CanUseMana);
        AttackValueCheckNode = new ActionNode(CheckAttack);
        AttackCheckSequence = new Sequence(new List<Node> {
            AttackCheckNode,
            AttackValueCheckNode,
        });


        //select the buff
        BuffSelectorNode = new Selector(new List<Node> {
            ManaCheckSequence,
            DefenceCheckSequence,
            AttackCheckSequence,
        });

        //Check player health, if it is low, it will go for the kill
        AttackPlayerNode = new ActionNode(CheckPlayerHealth);

        rootNode = new Selector(new List<Node> {
            HealthCheckSequence,
            BuffSelectorNode,
            AttackPlayerNode,
        });


        //own animator
        //Get the Animator attached to the GameObject you are intending to animate.
        animator = gameObject.GetComponent<Animator>();

        //Get the AudioSource attached to the GameObject
        audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    public void SetPlayerData(NewPlayer human, NewPlayer ai)
    {
        playerData = human;
        ownData = ai;
    }

    public void Evaluate()
    {
        rootNode.Evaluate();
        StartCoroutine(Execute());
        
    }

    public void FuzzyEvaluate()
    {
       if (RunFuzzy())
        {
            rootNode.Evaluate();
            StartCoroutine(Execute());
        }
    }

    private IEnumerator Execute()
    {
        Debug.Log("The AI is thinking...");
        if (HighSpeed)
        {
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(2.5f);
        }

        //low health
        if (HealthCheckSequence.nodeState == NodeStates.SUCCESS || HealthRisk)
        {
            Debug.Log("The AI decided to heal itself");
            UpdateSprites();
            ownData.DecreaseMana(5);
            ownData.Heal();

            //animation for healing
            animator.SetTrigger("Heal");
            StateText.text = "Heal!";

            //sound effect
            audioPlayer.PlayOneShot(SFX[0]);
        }//apply a buff
        else if (BuffSelectorNode.nodeState == NodeStates.SUCCESS || (ManaRisk || DefenceRisk || AttackRisk))
        {
            //determine which buff to use
            if (ManaCheckSequence.nodeState == NodeStates.SUCCESS || ManaRisk)
            {
                Debug.Log("The AI decided to Increase mana!");
                UpdateSprites();
                ownData.increaseMana();

                //animation for increasing mana
                animator.SetTrigger("BuffMana");

                StateText.text = "Increase mana!";
                //sound effect
                audioPlayer.PlayOneShot(SFX[1]);
            }
            else if (DefenceCheckSequence.nodeState == NodeStates.SUCCESS || DefenceRisk)
            {
                Debug.Log("The AI decided to Increase Defence!");
                UpdateSprites();
                ownData.DecreaseMana(2);
                ownData.increaseDefence();

                //animation for increasing Defence
                animator.SetTrigger("BuffDefence");
                StateText.text = "Increase Defence!";
                //sound effect
                audioPlayer.PlayOneShot(SFX[2]);
            }
            else if (AttackCheckSequence.nodeState == NodeStates.SUCCESS || AttackRisk)
            {
                Debug.Log("Increase Attack!");

                UpdateSprites();
                ownData.DecreaseMana(2);
                ownData.increaseAttack();

                //animation for increasing Attack
                animator.SetTrigger("BuffAttack");
                StateText.text = "Increase Attack!";
                //sound effect
                audioPlayer.PlayOneShot(SFX[3]);
            }
        }//attack the player
        else if (AttackPlayerNode.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to attack the player");
            UpdateSprites();
            playerData.Damage(ownData.CurrentAttack);
            ownData.DecreaseAttack();
            ownData.DecreaseMana(1);
            //animation for attacking
            animator.SetTrigger("Attack");
            StateText.text = "Direct Attack!";
            //sound effect
            audioPlayer.PlayOneShot(SFX[4]);
        }
        else if(NoRisk && !HealthRisk && !ManaRisk && !AttackRisk && !DefenceRisk)
        {
            Debug.Log("Default Attack");
            UpdateSprites();
            playerData.Damage(ownData.CurrentAttack);
            ownData.DecreaseAttack();
            ownData.DecreaseMana(1);

            //animation for attacking
            animator.SetTrigger("Attack");
            StateText.text = "Default Attack";
            //sound effect
            audioPlayer.PlayOneShot(SFX[5]);
        }
        if (onTreeExecuted != null)
        {
            onTreeExecuted();
        }
    }

    //Check low health
    private NodeStates CriticalHealthCheck()
    {
        if (ownData.HasLowHealth)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    //Check low defence
    private NodeStates CheckDefence()
    {
        if (ownData.HasLowDefence)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    //Check low Attack
    private NodeStates CheckAttack()
    {
        if (ownData.HasLowAttack)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    //Check player health
    private NodeStates CheckPlayerHealth()
    {
        if (playerData.HasLowHealth)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    //Check Mana
    private NodeStates CheckMana()
    {
        if (ownData.HasLowMana)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    private NodeStates CanUseMana()
    {
        if (ownData.CurrentMana > 5)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    public void UpdateSprites()
    {
        if (rootNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(rootNodeSprite);
        }
        else if (rootNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(rootNodeSprite);
        }

        if (HealthCheckSequence.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(HealthCheckSequenceSprite);
        }
        else if (HealthCheckSequence.nodeState == NodeStates.FAILURE)
        {
            SetFailed(HealthCheckSequenceSprite);
        }

        if (BuffSelectorNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(BuffSelectorNodeSprite);
        }
        else if (BuffSelectorNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(BuffSelectorNodeSprite);
        }

        if (AttackPlayerNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(AttackPlayerNodeSprite);
        }
        else if (AttackPlayerNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(AttackPlayerNodeSprite);
        }

        if (HealthCheckNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(HealthCheckNodeSprite);
        }
        else if (HealthCheckNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(HealthCheckNodeSprite);
        }

        if (ManaCheckHealthNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(ManaCheckHealthNodeSprite);
        }
        else if (ManaCheckHealthNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(ManaCheckHealthNodeSprite);
        }

        if (ManaCheckSequence.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(ManaCheckSequenceSprite);
        }
        else if (ManaCheckSequence.nodeState == NodeStates.FAILURE)
        {
            SetFailed(ManaCheckSequenceSprite);
        }

        if (DefenceCheckSequence.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(DefenceCheckSequenceSprite);
        }
        else if (DefenceCheckSequence.nodeState == NodeStates.FAILURE)
        {
            SetFailed(DefenceCheckSequenceSprite);
        }

        if (AttackCheckSequence.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(AttackCheckSequenceSprite);
        }
        else if (AttackCheckSequence.nodeState == NodeStates.FAILURE)
        {
            SetFailed(AttackCheckSequenceSprite);
        }

        if (ManaValueCheckNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(ManaValueCheckNodeSprite);
        }
        else if (ManaValueCheckNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(ManaValueCheckNodeSprite);
        }

        if (ManaCheckNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(ManaCheckNodeSprite);
        }
        else if (ManaCheckNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(ManaCheckNodeSprite);
        }

        if (DefenceValueCheckNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(DefenceValueCheckNodeSprite);
        }
        else if (DefenceValueCheckNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(DefenceValueCheckNodeSprite);
        }

        if (DefenceCheckNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(DefenceCheckNodeSprite);
        }
        else if (DefenceCheckNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(DefenceCheckNodeSprite);
        }

        if (AttackValueCheckNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(AttackValueCheckNodeSprite);
        }
        else if (AttackValueCheckNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(AttackValueCheckNodeSprite);
        }

        if (AttackCheckNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(AttackCheckNodeSprite);
        }
        else if (AttackCheckNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(AttackCheckNodeSprite);
        }

    }

    public void ResetSprites()
    {
        StateText.text = "";
        rootNodeSprite.color = new Color(255, 255, 255);
        HealthCheckSequenceSprite.color = new Color(255, 255, 255);
        BuffSelectorNodeSprite.color = new Color(255, 255, 255);
        AttackPlayerNodeSprite.color = new Color(255, 255, 255);
        HealthCheckNodeSprite.color = new Color(255, 255, 255);
        ManaCheckHealthNodeSprite.color = new Color(255, 255, 255);
        ManaCheckSequenceSprite.color = new Color(255, 255, 255);
        DefenceCheckSequenceSprite.color = new Color(255, 255, 255);
        AttackCheckSequenceSprite.color = new Color(255, 255, 255);
        ManaValueCheckNodeSprite.color = new Color(255, 255, 255);
        ManaCheckNodeSprite.color = new Color(255, 255, 255);
        DefenceValueCheckNodeSprite.color = new Color(255, 255, 255);
        DefenceCheckNodeSprite.color = new Color(255, 255, 255);
        AttackValueCheckNodeSprite.color = new Color(255, 255, 255);
        AttackCheckNodeSprite.color = new Color(255, 255, 255);
    }
    private void SetEvaluating(Image Sprite)
    {
        Sprite.color = new Color(255, 255, 0, 255);
    }

    private void SetSucceeded(Image Sprite)
    {
        Sprite.color = new Color(0, 255, 0, 255);
    }

    private void SetFailed(Image Sprite)
    {
        Sprite.color = new Color(255, 0, 0, 255);
    }
     
    private bool RunFuzzy()
    {      
        Vector3 FuzzyHealth = BasicFuzzy(ownData.CurrentHealth / ownData.MaxHealth);
        Vector3 FuzzyMana = BasicFuzzy(ownData.CurrentMana / ownData.MaxMana);
        Vector3 FuzzyDefence = BasicFuzzy(ownData.CurrentDefence / 15.0f);
        Vector3 FuzzyAttack = BasicFuzzy(ownData.CurrentAttack / 15.0f);

        Vector3 FuzzyMinHealth = BasicFuzzy(ownData.MinimumHealth / ownData.MaxHealth);
        Vector3 FuzzyMinMana = BasicFuzzy(ownData.MinimumMana / ownData.MaxMana);
        Vector3 FuzzyMinDefence = BasicFuzzy(ownData.MinimumDefence / 15.0f);
        Vector3 FuzzyMinAttack = BasicFuzzy(ownData.MinimumAttack / 15.0f);

        if (FuzzyHealth.z > FuzzyHealth.x)
        {
            if (FuzzyHealth.z > FuzzyMana.z)
            {
                if (FuzzyHealth.z > FuzzyDefence.z)
                {
                    if (FuzzyHealth.z > FuzzyAttack.z)
                    {
                        if (FuzzyHealth.z >= FuzzyMinHealth.z)
                        {
                            Debug.Log("Health Risk");
                            HealthRisk = true;
                            ManaRisk = false;
                            DefenceRisk = false;
                            AttackRisk = false;
                            NoRisk = false;
                            return true;
                        }
                    }
                }
            }
        }

        if (FuzzyMana.z > FuzzyMana.x)
        {
            if (FuzzyMana.z > FuzzyHealth.z)
            {
                if (FuzzyMana.z > FuzzyDefence.z)
                {
                    if (FuzzyMana.z > FuzzyAttack.z)
                    {
                        if (FuzzyMana.z >= FuzzyMinMana.z)
                        {
                            Debug.Log("Mana Risk");
                            HealthRisk = false;
                            ManaRisk = true;
                            DefenceRisk = false;
                            AttackRisk = false;
                            NoRisk = false;
                            return true;
                        }
                    }
                }
            }
        }

        if (FuzzyDefence.z > FuzzyDefence.x)
        {
            if (FuzzyDefence.z > FuzzyHealth.z)
            {
                if (FuzzyDefence.z > FuzzyMana.z)
                {
                    if (FuzzyDefence.z > FuzzyAttack.z)
                    {
                        if (FuzzyDefence.z >= FuzzyMinDefence.z)
                        {
                            Debug.Log("Defence Risk");
                            HealthRisk = false;
                            ManaRisk = false;
                            DefenceRisk = true;
                            AttackRisk = false;
                            NoRisk = false;
                            return true;
                        }
                    }
                }
            }
        }

        if (FuzzyAttack.z > FuzzyAttack.x)
        {
            if (FuzzyAttack.z > FuzzyHealth.z)
            {
                if (FuzzyAttack.z > FuzzyMana.z)
                {
                    if (FuzzyAttack.z >= FuzzyMinAttack.z)
                    {
                        Debug.Log("Attack Risk");
                        HealthRisk = false;
                        ManaRisk = false;
                        DefenceRisk = false;
                        AttackRisk = true;
                        NoRisk = false;
                        return true;
                    }
                }
            }
        }

        HealthRisk = false;
        ManaRisk = false;
        DefenceRisk = false;
        AttackRisk = false;
        NoRisk = true;
        return true;
    }

    public Vector3 BasicFuzzy(float inputValue)
    {
        float healthyValue = healthy.Evaluate(inputValue);
        float hurtValue = hurt.Evaluate(inputValue);
        float criticalValue = critical.Evaluate(inputValue);

        return new Vector3(healthyValue, hurtValue, criticalValue);
    }    
}
