using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdatedEnemyBehaviorTree : MonoBehaviour
{
    private NewPlayer playerData;
    private NewPlayer ownData;

    public ActionNode healthCheckNode;
    public ActionNode DefenceCheckNode;
    public ActionNode AttackValueCheckNode;
    public ActionNode attackCheckNode;
    public ActionNode ManaCheckNode;

    public Selector rootNode;

    //maybe

    public RandomBinaryNode buffCheckRandomNode;
    public ActionNode buffCheckNode;
    public Sequence buffCheckSequence;

    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExecuted;

    public delegate void NodePassed(string trigger);

    void Start()
    {
        //Check low health, if its low it will decide to heal
        healthCheckNode = new ActionNode(CriticalHealthCheck);
        //          if cannot afford to heal it will regen mana

        //Check low defence
        DefenceCheckNode = new ActionNode(CheckDefence);

        //Check low Attack
        AttackValueCheckNode = new ActionNode(CheckAttack);
        //Check player health, if it is low, it will go for the kill
        attackCheckNode = new ActionNode(CheckPlayerHealth);

        //Check Mana
        ManaCheckNode = new ActionNode(CheckMana);


        /* If neither the player nor the AI are low in health, the AI will 
         * prioritize using a defensive buff. To avoid having the AI buff every turn, 
         we use a binary randomizer to only do it half the time. */
        buffCheckRandomNode = new RandomBinaryNode();
     //   buffCheckNode = new ActionNode(BuffCheck);
        buffCheckSequence = new Sequence(new List<Node> {
            buffCheckRandomNode,
            buffCheckNode,
        });

        rootNode = new Selector(new List<Node> {
            healthCheckNode,
            attackCheckNode,
            buffCheckSequence,
        });
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

    private IEnumerator Execute()
    {
        Debug.Log("The AI is thinking...");
       yield return new WaitForSeconds(0.00001f);

        if (healthCheckNode.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to heal itself");
            ownData.Heal();
        }
        else if (attackCheckNode.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to attack the player!");
            playerData.Damage(ownData.CurrentAttack);
        }
        else if (buffCheckSequence.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to defend itself");
          //  ownData.Buff();
        }
        else
        {
            Debug.Log("The AI finally decided to attack the player");
            playerData.Damage(ownData.CurrentAttack);
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
        if (playerData.HasLowDefence)
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
        if (playerData.HasLowAttack)
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
        if (playerData.HasLowMana)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }
}
