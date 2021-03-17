using UnityEngine;

public class EnemyTurnState2 : StateMachineBehaviour
{//enemy number to control which logic to run
    [SerializeField]
    private string EnemyNumber;
    [SerializeField]
    private int AINumber;
    [SerializeField]
    private bool Human;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Human)
        {
            animator.SetTrigger("EndTurn");
        }
        else
        {//print out that it is starting its turn
            Debug.Log("********************* \n Strating the" + EnemyNumber + " enemy's turn!");
            if (AINumber == 1)
            {//call the decision tree for the AI
                animator.gameObject.GetComponent<NewGame>().EvaluateAITree();
            }
            else
            {//call the decision tree for AI
                animator.gameObject.GetComponent<NewGame>().EvaluateAITree2();
            }
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Ending the " + EnemyNumber + " enemy's turn. \n *********************");

    }    
}
