using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class AttackNode : Node
    {
        private Animator animator;
        private Transform player;
        private Transform enemy;
        private float attackCooldown;
        private float attackTimer;

        private int selectedAttack;

        public AttackNode(Animator animator, Transform player, Transform enemy, float attackCooldown)
        {
            this.animator = animator;
            this.player = player;
            this.enemy = enemy;
            this.attackCooldown = attackCooldown;
            attackTimer = attackCooldown;

        }

        

        public override NodeState EvaluateState()
        {
            attackTimer += Time.deltaTime;
            Vector3 targetPosition = new Vector3(player.position.x, enemy.position.y, player.position.z);
            enemy.LookAt(targetPosition);
            if (attackTimer >= attackCooldown)
                StartCooldown();


            return NodeState.RUNNING;
        }

        private void StartCooldown()
        {
            animator.SetTrigger("Attack");


            if (animator.GetLayerName(0) == "Axe")
            {
                if (selectedAttack == 0)
                {
                    selectedAttack = Random.Range(1, 4);
                    animator.SetTrigger("Attack " + selectedAttack);
                }
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                {
                    selectedAttack = 0;
                }
                selectedAttack = 0;
            }                             
            attackTimer = 0f;
        }

        //private void AttackCooldown()
        //{
        //    animator.SetTrigger("AttackCooldown");
        //    timer += Time.deltaTime;
        //    if (timer >= attackSpeed)
        //    {                
        //        animator.ResetTrigger("AttackCooldown");
        //        timer = 0;
        //    }
        //    else
        //        canAttack = true;
        //}
    }
}

