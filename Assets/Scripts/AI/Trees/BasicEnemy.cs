using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class BasicEnemy : BaseBt
    {
        #region Variables
        public Transform player;
        public float detectionRange;
        public float attackRange;
        public float randomPosDist;
        public int weaponDamage;
        public Transform weapon;
        private Material weaponMaterial;
        public Color weaponFlashColour;
        public CreateAttackLocations createAttacks;
        public bool playerEngaged;
        public float attackCooldown;

        [Tooltip("Positions the AI will try to attack the player")]
        public List<AttackPosition> attackPositions;

        private bool canAttack;
        public float attackSpeed;
        #endregion

        #region Awake
        protected override void Awake()
        {
            weaponMaterial = weapon.GetComponent<MeshRenderer>().material;
            
            //attackPositions = player.GetComponent<AttackManager>().attackingLocations;
        }
        #endregion

        protected override void Start()
        {
            attackPositions = createAttacks.attackPositions;
            base.Start();
        }

        #region Tree
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
                new DetectionNode(player, transform, detectionRange),
                new ChaseNode(agent, attackRange, playerEngaged, animator),
                //new PositionNode(attackPositions, agent, player, animator, ),
                new AttackNode(animator, player, agent.transform, attackCooldown),
                }),

                new Sequence(new List<Node>
                {
                //new DetectionNode(player, transform, detectionRange),
                //new ChaseNode(agent, attackRange, playerEngaged, animator),
                new RandomPosition(player, agent, randomPosDist),
                }),
            });;

            return root;
        }
        #endregion

        /// <summary>
        /// Change weapon colour for duration of coroutine
        /// </summary>
        public IEnumerator FlashWeapon()
        {
            weaponMaterial.SetColor("_EmissionColor", weaponFlashColour);
            weaponMaterial.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.15f);
            weaponMaterial.SetColor("_EmissionColor", Color.white);
            weaponMaterial.DisableKeyword("_EMISSION");
        }

        public IEnumerator AttackCooldown()
        {
            canAttack = false;
            yield return new WaitForSeconds(attackSpeed);
            canAttack = true;
        }
    }

}


