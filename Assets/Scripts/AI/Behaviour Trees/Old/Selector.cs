using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour tree implementation adapted from: https://hub.packtpub.com/building-your-own-basic-behavior-tree-tutorial/

namespace BehaviourTree
{
    /// <summary>
    /// Finds the first child node that doesn't fail
    /// </summary>
    public class Selector : Node
    {
        /// <summary>
        /// Finds the first child node that doesn't fail
        /// </summary>
        public Selector() : base() { }// The childs nodes for this sequence
        public Selector(List<Node> children) : base(children) { } // Must provide an initial set of children nodes to work

        public override NodeState EvaluateState()
        {
            foreach (Node node in children) // Iterarte through child nodes
            {
                switch (node.EvaluateState()) // Returns one of the 3 enum types
                {
                    case NodeState.SUCCESS: // If success return immediately
                        m_nodeState = NodeState.SUCCESS;
                        return m_nodeState;
                    case NodeState.FAILURE: // Continuing checking rest of the child nodes
                        continue;
                    case NodeState.RUNNING: // If running immediately return
                        m_nodeState = NodeState.RUNNING;
                        return m_nodeState;
                    default:
                        continue;
                }
            }
            // All child nodes must be failures
            m_nodeState = NodeState.FAILURE; // Return NodeState as FAILURE
            return m_nodeState; // Return nodeState
        }
    }
}

