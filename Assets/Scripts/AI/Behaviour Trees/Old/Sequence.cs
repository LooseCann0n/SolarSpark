using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour tree implementation adapted from: https://hub.packtpub.com/building-your-own-basic-behavior-tree-tutorial/
// Taken from pacman game example

namespace BehaviourTree
{
    /// <summary>
    /// Runs through all child nodes, all must be positive to return success otherwise return failure
    /// </summary>
    public class Sequence : Node
    {
        public Sequence() : base() { }// The childs nodes for this sequence
        public Sequence(List<Node> children) : base(children) { } // Must provide an initial set of children nodes to work

        public override NodeState EvaluateState() // Override the abstract EvaluateState in BehaviourNode
        {
            bool anyChildRunning = false; // Set to true if we find a running state, so we can continue and not break loop

            foreach (Node node in children) // Iterate through each child node
            {
                switch (node.EvaluateState()) // Will return one of the 3 enum types
                {
                    case NodeState.SUCCESS: // We continue forward evaluating the next child
                        continue;
                    case NodeState.FAILURE: // If any child nodes are FAILURE then the Sequence returns as a failure
                        m_nodeState = NodeState.FAILURE;
                        return m_nodeState;
                    case NodeState.RUNNING: // Continue onward but note that we found a RUNNING node state
                        anyChildRunning = true;
                        continue;
                    default:
                        m_nodeState = NodeState.SUCCESS;
                        continue;
                }
            }
            // This code is only reached if all NodeStates are SUCCESS or RUNNING
            m_nodeState = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS; /* If the anyChildRunning is true, then it is running 
                                                                                otherwise set NodeState to success*/
            return m_nodeState; // Return the NodeState value
        }
    }
}
