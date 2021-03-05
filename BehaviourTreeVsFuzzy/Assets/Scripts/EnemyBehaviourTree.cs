using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourTree : MonoBehaviour
{
    private Player playerData;
    private Player ownData;

    //Nodes for enemy behaviour tree
    public RandomBinaryNode buffCheckRandomNode;
    public ActionNode buffCheckNode;
    public ActionNode healthCheckNode;
    public ActionNode attackCheckNode;
    public Sequence buffCheckSequence;
    public Selector rootNode;

    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExectuted;

    private void Start()
    {
        healthCheckNode = new ActionNode(CriticalHealthCheck);

        attackCheckNode = new ActionNode(CheckPlayerHealth);

        //layer 3
        buffCheckRandomNode = new RandomBinaryNode();
        buffCheckNode = new ActionNode(BuffCheck);

        //layer 2
        buffCheckSequence = new Sequence(new List<Node>
        {
          buffCheckRandomNode,
          buffCheckNode,
        });

        //root node
        rootNode = new Selector(new List<Node>
        {
            healthCheckNode,
            attackCheckNode,
            buffCheckSequence,
        });
    }

    public void SetPlayerData(Player human, Player ai)
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
        Debug.Log("The AI is Thinking....");
        yield return new WaitForSeconds(2.5f);

        if (healthCheckNode.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to heal itself");
            ownData.Heal();
        }
        else if (attackCheckNode.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to attack the player!");
            playerData.Damage();
        }
        else if (buffCheckSequence.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("The AI decided to defend itself");
            ownData.Buff();
        }
        else
        {
            Debug.Log("the AI finally decided to attack the player");
            playerData.Damage();
        }
        if (onTreeExectuted != null)
        {
            onTreeExectuted();
        }
    }

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

    private NodeStates BuffCheck()
    {
        if (!ownData.IsBuffed)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }
}
