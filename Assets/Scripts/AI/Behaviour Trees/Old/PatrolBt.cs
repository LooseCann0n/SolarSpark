using UnityEngine;
using System;
using System.Collections.Generic;

namespace BehaviourTree
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    
    public class PatrolBt : BaseBt
    {
        /// <summary>
        /// Forms a behaviour tree which patrols and detects any object which matches the targetMask
        /// </summary>

        #region Variables

        [Tooltip("Patrol points the AI will move between")]
        public Transform[] wayPoints;

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
                //new Sequence (new List<Node>
                //{
                //    new AlertedNode(transform, xrPlayer),
                //    new SearchNode(transform, searchRadius, minimumRadius, searchAmount, escapedClip),
                //    new DetectionNode(transform, viewAngle, inRadiusRange, viewRange, detectionTime, targetMask, obstacleMask, tempEnemy, audioSource, detectedClip, detectedEvent), // FOVCheck node passing in variables for constructor
                //    new ChaseNode(agent, walkingSpeed, runningSpeed, maxChasingDistance, audioSource, searchingClip, animator),
                //}),
                //new Sequence ( new List<Node> // Sequence node which contains FOVCheck and ChaseNode, for chaseNode to run, FOVCheck must return SUCCESS
                //{
                //    new LightNode(tempEnemy),
                //    new DetectionNode(transform, viewAngle, inRadiusRange, viewRange, detectionTime, targetMask, obstacleMask, tempEnemy, audioSource, detectedClip, detectedEvent), // FOVCheck node passing in variables for constructor
                //    new ChaseNode(agent, maxChasingDistance, walkingSpeed, runningSpeed, audioSource, searchingClip, animator),
                //}),
                //new Sequence ( new List<Node>
                //{
                //    new SearchNode(transform, searchRadius, minimumRadius, searchAmount, escapedClip),
                //}),
                //new Sequence ( new List<Node>
                //{
                //new Inverter(new AlertedNode(transform, xrPlayer)),
                //new PatrolNode(wayPoints, transform, agent, animator, waitTime), // PatrolNode passing in patrolpoints and d 
                //}),
            });
            return root;
        }
        #endregion
    }
}


