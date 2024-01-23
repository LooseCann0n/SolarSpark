using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour tree implementation adapted from: https://hub.packtpub.com/building-your-own-basic-behavior-tree-tutorial/

namespace BehaviourTree
{
    public class Inverter : Node
    {
        /// <summary>
        /// Inverts the returned state, eg SUCCESS becomes FAILURE
        /// </summary>

        private Node m_node; // Node that we want to invert

        public Node node
        {
            get { return m_node; }
        }

        public Inverter (Node node)
        {
            m_node = node;
        }

        public override NodeState EvaluateState() // Override Evaluate state in BtNode
        {
            switch (node.EvaluateState())
            {
                case NodeState.FAILURE: // FAILURE is changed to SUCCESS
                    m_nodeState = NodeState.SUCCESS;
                    return m_nodeState;
                case NodeState.SUCCESS: // SUCCESS is changed to FAILURE
                    m_nodeState = NodeState.FAILURE;
                    return m_nodeState;
                case NodeState.RUNNING: // RUNNING remains the same
                    m_nodeState = NodeState.RUNNING;
                    return m_nodeState;
            }
            m_nodeState = NodeState.SUCCESS;
            return m_nodeState;
        }
    }
}


