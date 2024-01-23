using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveInNode : Node
    {
        private JackalBt jackalBt;

        public MoveInNode(JackalBt jackalBt)
        {
            this.jackalBt = jackalBt;
        }

        public override NodeState EvaluateState()
        {
            if (jackalBt.canMoveIn)
            {
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
    }
}
