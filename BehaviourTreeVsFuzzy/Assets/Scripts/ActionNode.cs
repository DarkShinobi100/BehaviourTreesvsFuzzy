using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node {

    /* method sigrature for the action */
    public delegate NodeStates ActionNodeDelegate();

    /* The delgate that is called to evaluate this node */
    private ActionNodeDelegate m_action;

    /* Because this node contains no logic itself, the logic must 
     * be passed in in the form of a delgate. As the signature states,
     * the actopm needs to return a NodeStates enum */

    public ActionNode(ActionNodeDelegate action)
    {
        m_action = action;
    }

    /*Evaluated the node using the passed in delegate and reports
     * the resulting state as sppropriate */
    public override NodeStates Evaluate()
    {
       switch (m_action())
        {
            case NodeStates.SUCCESS:
                m_nodeState = NodeStates.SUCCESS;
                return m_nodeState;

            case NodeStates.FAILURE:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;

            case NodeStates.RUNNING:
                m_nodeState = NodeStates.RUNNING;
                return m_nodeState;

            default:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
        }
    }
}
