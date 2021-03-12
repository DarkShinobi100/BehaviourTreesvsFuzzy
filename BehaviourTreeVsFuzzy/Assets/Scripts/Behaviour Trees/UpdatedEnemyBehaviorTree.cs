﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdatedEnemyBehaviorTree : MonoBehaviour
{
    private NewPlayer playerData;
    private NewPlayer ownData;
    
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

    void Start()
    {
        //Check low health, if its low it will decide to heal
        ManaCheckHealthNode = new ActionNode(CheckMana);
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
        DefenceCheckNode = new ActionNode(CheckMana);
        DefenceValueCheckNode = new ActionNode(CheckDefence);
        DefenceCheckSequence = new Sequence(new List<Node> {
            DefenceCheckNode,
            DefenceValueCheckNode,
        });


        //Check low Defence, if its low it will decide to buff
        AttackCheckNode = new ActionNode(CheckMana);
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

        //low health
        if (HealthCheckSequence.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to heal itself");

            ownData.DecreaseMana();
            ownData.Heal();
        }//apply a buff
        else if (BuffSelectorNode.nodeState == NodeStates.SUCCESS)
        {
            //determine which buff to use
            if (ManaCheckSequence.nodeState == NodeStates.SUCCESS)
            {
                Debug.Log("The AI decided to Increase mana!");
                ownData.increaseMana();                
            }
            else if (DefenceCheckSequence.nodeState == NodeStates.SUCCESS)
            {
                Debug.Log("The AI decided to Increase Defence!");
                ownData.DecreaseMana();
                ownData.increaseDefence();
            }
            else if(AttackCheckSequence.nodeState == NodeStates.SUCCESS)
            {
                Debug.Log("The AI decided to Increase Attack!");
                ownData.DecreaseMana();
                ownData.increaseAttack();
            }
        }//attack the player
        else if (AttackPlayerNode.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to attack the player");
            playerData.Damage(ownData.CurrentAttack);
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
