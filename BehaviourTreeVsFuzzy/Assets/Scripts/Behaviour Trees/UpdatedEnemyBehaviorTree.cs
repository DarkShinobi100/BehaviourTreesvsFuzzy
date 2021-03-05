using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedEnemyBehaviorTree : MonoBehaviour {

    //necessary data
    private Player playerData;
    private Player ownData;

    //nodes
    public RandomBinaryNode buffCheckRandomNode;
    public ActionNode buffCheckNode;
    public ActionNode healthCheckNode;
    public ActionNode attackCheckNode;
    public Sequence buffCheckSequence;
    public Selector rootNode;

    //called when Tree has reached a result
    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExecuted;

    public delegate void NodePassed(string trigger);
}
