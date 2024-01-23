using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour tree implementation adapted from: https://hub.packtpub.com/building-your-own-basic-behavior-tree-tutorial/

// Blueprint for all other nodes to be built upon

namespace BehaviourTree
{
    public enum NodeState
    {
        SUCCESS,
        FAILURE,
        RUNNING
    };
    public abstract class Node
    {
        public Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public delegate NodeState nodeReturn();

        protected NodeState m_nodeState; // The current state of the node

        public NodeState nodeState
        {
            get { return m_nodeState; }
        }

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
                _Attach(child);
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool clear = node.ClearData(key);
                if (clear)
                    return true;
                node = node.parent;
            }
            return false;
        }
        // Used to determine the state of the node
        public abstract NodeState EvaluateState();
    }
}


