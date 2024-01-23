using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonBlocker : MonoBehaviour
{
    public CapsuleCollider characterCollider;
    public CapsuleCollider collisionColliderBlocker;

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, collisionColliderBlocker, true);
    }
}
