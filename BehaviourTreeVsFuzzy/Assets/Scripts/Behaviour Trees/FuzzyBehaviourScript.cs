using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//university Library
using FLS;
using FLS.Rules;
using FLS.MembershipFunctions;

public class FuzzyBehaviourScript : MonoBehaviour
{
    private NewPlayer playerData;
    private NewPlayer ownData;

    private Animator animator;
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


    //Fuzzy
    private LinguisticVariable FuzzyHealth;
    private LinguisticVariable FuzzyHealthRisk;
    private IFuzzyEngine fuzzyHealthEngine;

    private LinguisticVariable FuzzyMana;
    private LinguisticVariable FuzzyManaRisk;
    private IFuzzyEngine fuzzyManaEngine;

    private LinguisticVariable FuzzyDefence;
    private LinguisticVariable FuzzyDefenceRisk;
    private LinguisticVariable FuzzyAttack;
    private LinguisticVariable FuzzyAttackRisk;


    [SerializeField]
    private double HealthResult;


    void Start()
    {
        SetupFuzzy();
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
        RunFuzzy();
        rootNode.Evaluate();
        StartCoroutine(Execute());
    }

    private IEnumerator Execute()
    {
        Debug.Log("The AI is thinking...");
        yield return new WaitForSeconds(2.5f);
        // yield return new WaitForEndOfFrame();
        //low health
        if (HealthCheckSequence.nodeState == NodeStates.SUCCESS)
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
        else if (BuffSelectorNode.nodeState == NodeStates.SUCCESS)
        {
            //determine which buff to use
            if (ManaCheckSequence.nodeState == NodeStates.SUCCESS)
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
            else if (DefenceCheckSequence.nodeState == NodeStates.SUCCESS)
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
            else if (AttackCheckSequence.nodeState == NodeStates.SUCCESS)
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
        else
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

    private void SetupFuzzy()
    {
        Debug.Log("Setup Fuzzy!");
        //define variables and membership functions(graphs)
        //Variables for player data
        FuzzyHealth = new LinguisticVariable("FuzzyHealth");
        var highHealth = FuzzyHealth.MembershipFunctions.AddTrapezoid("high", -50, -50, -5, -1);
        var midHealth = FuzzyHealth.MembershipFunctions.AddTrapezoid("mid", -5, -0.5, 0.5, 5);
        var lowHealth = FuzzyHealth.MembershipFunctions.AddTrapezoid("low", 1, 5, 50, 50);

        FuzzyMana = new LinguisticVariable("FuzzyMana");
        var highMana = FuzzyMana.MembershipFunctions.AddTrapezoid("high", -50, -50, -5, -1);
        var midMana = FuzzyMana.MembershipFunctions.AddTrapezoid("mid", -5, -0.5, 0.5, 5);
        var lowMana = FuzzyMana.MembershipFunctions.AddTrapezoid("low", 1, 5, 50, 50);

        FuzzyDefence = new LinguisticVariable("FuzzyDefence");
        var highDefence = FuzzyDefence.MembershipFunctions.AddTrapezoid("high", -50, -50, -5, -1);
        var midDefence = FuzzyDefence.MembershipFunctions.AddTrapezoid("mid", -5, -0.5, 0.5, 5);
        var lowDefence = FuzzyDefence.MembershipFunctions.AddTrapezoid("low", 1, 5, 50, 50);

        FuzzyAttack = new LinguisticVariable("FuzzyAttack");
        var highAttack = FuzzyAttack.MembershipFunctions.AddTrapezoid("high", -50, -50, -5, -1);
        var midAttack = FuzzyAttack.MembershipFunctions.AddTrapezoid("mid", -5, -0.5, 0.5, 5);
        var lowAttack = FuzzyAttack.MembershipFunctions.AddTrapezoid("low", 1, 5, 50, 50);

        FuzzyHealthRisk = new LinguisticVariable("Healthrisk");
        var highHealthRisk = FuzzyHealthRisk.MembershipFunctions.AddTrapezoid("high", -50, -50, -5, -1);
        var midHealthRisk = FuzzyHealthRisk.MembershipFunctions.AddTrapezoid("mid", -5, -0.5, 0.5, 5);
        var lowHealthRisk = FuzzyHealthRisk.MembershipFunctions.AddTrapezoid("low", 1, 5, 50, 50);

        FuzzyManaRisk = new LinguisticVariable("Manarisk");
        var highManaRisk = FuzzyManaRisk.MembershipFunctions.AddTrapezoid("high", -50, -50, -5, -1);
        var midManaRisk = FuzzyManaRisk.MembershipFunctions.AddTrapezoid("mid", -5, -0.5, 0.5, 5);
        var lowManaRisk = FuzzyManaRisk.MembershipFunctions.AddTrapezoid("low", 1, 5, 50, 50);

        FuzzyDefenceRisk = new LinguisticVariable("Defencerisk");
        var highDefenceRisk = FuzzyDefenceRisk.MembershipFunctions.AddTrapezoid("high", -50, -50, -5, -1);
        var midDefenceRisk = FuzzyDefenceRisk.MembershipFunctions.AddTrapezoid("mid", -5, -0.5, 0.5, 5);
        var lowDefenceRisk = FuzzyDefenceRisk.MembershipFunctions.AddTrapezoid("low", 1, 5, 50, 50);

        FuzzyAttackRisk = new LinguisticVariable("Attackrisk");
        var highAttackRisk = FuzzyAttackRisk.MembershipFunctions.AddTrapezoid("high", -50, -50, -5, -1);
        var midAttackRisk = FuzzyAttackRisk.MembershipFunctions.AddTrapezoid("mid", -5, -0.5, 0.5, 5);
        var lowAttackRisk = FuzzyAttackRisk.MembershipFunctions.AddTrapezoid("low", 1, 5, 50, 50);

        //Fuzzy engine from the library
        fuzzyHealthEngine = new FuzzyEngineFactory().Default();

        //create rules
        //Health Rules
        var rule1 = Rule.If(FuzzyHealth.Is(highHealth)).Then(FuzzyHealthRisk.Is(highHealthRisk));
        var rule2 = Rule.If(FuzzyHealth.Is(midHealth)).Then(FuzzyHealthRisk.Is(midHealthRisk));
        var rule3 = Rule.If(FuzzyHealth.Is(lowHealth)).Then(FuzzyHealthRisk.Is(lowHealthRisk));

        //Mana Rules
        var rule4 = Rule.If(FuzzyMana.Is(highMana)).Then(FuzzyManaRisk.Is(highManaRisk));
        var rule5 = Rule.If(FuzzyMana.Is(midMana)).Then(FuzzyManaRisk.Is(midManaRisk));
        var rule6 = Rule.If(FuzzyMana.Is(lowMana)).Then(FuzzyManaRisk.Is(lowManaRisk));

        //Defence Rules
        var rule7 = Rule.If(FuzzyDefence.Is(highDefence)).Then(FuzzyDefenceRisk.Is(highDefenceRisk));
        var rule8 = Rule.If(FuzzyDefence.Is(midDefence)).Then(FuzzyDefenceRisk.Is(midDefenceRisk));
        var rule9 = Rule.If(FuzzyDefence.Is(lowDefence)).Then(FuzzyDefenceRisk.Is(lowDefenceRisk));

        //Attack Rules
        var rule10 = Rule.If(FuzzyAttack.Is(highAttack)).Then(FuzzyAttackRisk.Is(highAttackRisk));
        var rule11 = Rule.If(FuzzyAttack.Is(midAttack)).Then(FuzzyAttackRisk.Is(midAttackRisk));
        var rule12 = Rule.If(FuzzyAttack.Is(lowAttack)).Then(FuzzyAttackRisk.Is(lowAttackRisk));

        //add rules to engine
        fuzzyHealthEngine.Rules.Add(rule1, rule2, rule3,rule4, rule5,rule6,rule7,rule8,rule9,rule10,rule11,rule12);
    }

    private void RunFuzzy()
    {

        //fuzzy results
        // Convert position of box to value between 0 and 100
        //double result = engine.Defuzzify(new { distance = (double)this.transform.position.x });
        double temphealth = (ownData.CurrentHealth / ownData.MaxHealth) * 100;
       // double tempMana = (ownData.CurrentMana / ownData.MaxMana) * 100;

        HealthResult = fuzzyHealthEngine.Defuzzify(
            new {FuzzyHealth = (double)ownData.CurrentHealth,
            FuzzyMana = (double)ownData.CurrentMana,
            FuzzyDefence = (double)ownData.CurrentDefence,
            FuzzyAttack = (double)ownData.CurrentAttack
            });
        Debug.Log("Run Fuzzy!" + HealthResult.ToString());        
    }

}
