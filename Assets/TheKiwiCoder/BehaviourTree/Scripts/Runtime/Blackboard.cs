using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard {

        public Vector3 moveToPosition;
        public Transform attackPosition;
        public Transform playerTransform;
        public int circleCounter;
        public Vector3 targetCirclePoint;
        // 0 is clockwise, 1 is anticlockWise
        public int circleDirection;
    }
}