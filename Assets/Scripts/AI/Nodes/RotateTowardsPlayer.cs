using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class RotateTowardsPlayer : ActionNode
{
    public Transform target;
    public float rotationSpeed;

    private MonoBehaviour monoBehaviour;
    private Coroutine LookCoroutine;

    protected override void OnStart() 
    {
        monoBehaviour = context.gameObject.GetComponent<SimpleEnemy>().GetComponent<MonoBehaviour>();
        target = context.playerTransform;
    }
    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        StartRotating(monoBehaviour);
        return State.Running;
    }

    public void StartRotating(MonoBehaviour myMonoBehaviour)
    {
        if (LookCoroutine != null)
        {
            myMonoBehaviour.StopCoroutine(LookCoroutine);
        }
        LookCoroutine = myMonoBehaviour.StartCoroutine(LookAt());
    }
   

    private IEnumerator LookAt()
    {
        Debug.Log("Look At");
        Quaternion lookRotation = Quaternion.LookRotation(target.position, context.transform.position);

        float time = 0;
        
        while (time < 1)
        {
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, lookRotation, time);
            Debug.Log(time);
            time += Time.deltaTime * rotationSpeed;

            yield return null;
        }
    }
}
