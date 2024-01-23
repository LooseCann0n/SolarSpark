using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        /// <summary>
        /// Abstract class that is overwritten for new trees behaviours
        /// </summary>
        
        private Node _root = null; // Node that is underneath the current node

        protected virtual void Start()
        {
            _root = SetupTree(); // Setup the tree
        }

        private void Update()
        {
            if (_root != null) // If there is a root
                _root.EvaluateState(); // Run roots EvaluateState
        }

        protected abstract Node SetupTree(); // Overwritten by new behaviour tree code
    }
}

