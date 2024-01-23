using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class BaseBt : Tree
    {
        /// <summary>
        /// Based behaviour tree which others trees will inherit from
        /// </summary>



        #region Component References

        // Component References

        public Animator animator;

        public UnityEngine.AI.NavMeshAgent agent;


        #endregion

        #region Awake
        protected virtual void Awake()
        {

        }
        #endregion

        #region Tree
        /// <summary>
        /// Creates the tree with the different node behaviours
        /// </summary>
        /// <returns> Selector root </returns>
        protected override Node SetupTree()
        {
            // Selector which has 3 child nodes, Sequence with detection, Sequence with SearchNode and PatrolNode
            Node root = new Selector(new List<Node>
            {

            });

            return root;
        }
        #endregion
    }
}