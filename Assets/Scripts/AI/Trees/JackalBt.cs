using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class JackalBt : Tree
    {
        // Variables

        private NavMeshAgent enemyAgent;
        private GameObject player;
        private Animator animator;

        private Transform playerTransform;

        public GameObject AttackIndicator;

        [Header("Wander Variables")]
        [Tooltip("Distance between random wander points")]
        public float wanderDistance;
        [Tooltip("Time between selecting a new wander point")]
        public float wanderTime;
        public float wanderSpeed;

        [Header("Detection Variables")]
        [Tooltip("Angle of FOV Cone")]
        public float viewAngle;
        [Tooltip("Distance of FOV Cone")]
        public float coneDistance;
        [Tooltip("Obstacle that block FOV detection")]
        public LayerMask obstacleLayer;
        [Tooltip("Layer that we want to detect")]
        public LayerMask playerLayer;

        [Header("Positioning Variables")]
        [Tooltip("Distance enemy will wait before engaging")]
        public float waitingDistance;
        public float runningDistance;
        public float maxChasingDistance;
        public static float standardSpeed = 5f;
        public static float chaseSpeed = 7.5f;
        public float circlePlayerSpeed;

        [Header("Attack Variables")]
        [Range(0.2f, 2f)] public float attackCooldown;
        [Range(1f, 3f)] public float attackRange;


        private AttackPosition[] attackPositions;
        private List<Transform> circlePoints; 

        [Header("References")]
        public Transform weapon;
        private BoxCollider weaponCollider;
        public Color weaponFlashColour;
        private Material weaponMaterial;
        public AudioSource extraSource;
        public bool canAttack;
        private bool isRunning;
        public bool canMoveIn;

        private void Awake()
        {
            weaponCollider = weapon.GetComponent<BoxCollider>();
            animator = GetComponentInChildren<Animator>();    
            enemyAgent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerTransform = player.GetComponent<Transform>();
        }

        protected override void Start()
        {
            attackPositions = FindObjectsOfType<AttackPosition>();
            circlePoints = GameObject.Find("AttackPosition").GetComponentInChildren<CreateCirclePoints>().circlePoints;
            base.Start();
        }

        /// <summary>
        /// Creates the tree with the different node behaviours
        /// </summary>
        /// <returns> Selector root </returns>
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new InAttackRangeNode(enemyAgent, animator, attackRange),
                    //new CanAttackNode(enemyAgent),
                    new AttackNode(animator, playerTransform, transform, attackCooldown),
                }),
                new Sequence(new List<Node>
                {
                    new SearchNode(transform, coneDistance, viewAngle, playerLayer, obstacleLayer, animator),
                    new ApproachPlayerNode(enemyAgent, waitingDistance, maxChasingDistance, animator, runningDistance),

                    //new Selector(new List<Node>
                    //{
                    //    new Sequence(new List<Node>
                    //    {
                    //        //new CheckSpaceNode(attackPositions, enemyAgent),
                    //        new MoveInNode(this),
                    //        new ApproachPlayerNode(enemyAgent, waitingDistance, maxChasingDistance, animator, runningDistance),
                    //    }),
                    //    //new CirclePlayerNode(enemyAgent, playerTransform, animator, circlePoints)
                    //}),
                }),
                // Passive wander state
                new Sequence(new List<Node>
                {
                    new WanderNode(enemyAgent, wanderTime, wanderDistance, wanderSpeed, animator)
                }),
            });; ;

            return root;
        }

        public IEnumerator FlashWeapon()
        {
            weaponMaterial.SetColor("_EmissionColor", weaponFlashColour);
            weaponMaterial.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.15f);
            weaponMaterial.SetColor("_EmissionColor", Color.white);
            weaponMaterial.DisableKeyword("_EMISSION");
        }

        public void EnableCollider()
        {
            weaponCollider.enabled = true;
            AttackIndicator.SetActive(true);
        }

        public void DisableCollider()
        {
            weaponCollider.enabled = false;
            AttackIndicator.SetActive(false);
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}


