using System.Collections;
using UnityEditor;
using UnityEngine;
using BehaviourTree;

[CustomEditor(typeof(JackalBt))]
public class JackalBtEditor : Editor
{
    protected void OnSceneGUI()
    {
        JackalBt fov = (JackalBt)target;
        Handles.color = Color.black;

        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, fov.transform.forward * -1, 180 - (fov.viewAngle / 2), fov.coneDistance);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, fov.transform.forward * -1, -180 + (fov.viewAngle / 2), fov.coneDistance);

        Handles.color = Color.white;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.coneDistance);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.coneDistance);

        Handles.DrawWireArc(fov.transform.position, Vector3.up, fov.transform.forward, fov.viewAngle / 2, fov.coneDistance);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, fov.transform.forward, -fov.viewAngle / 2, fov.coneDistance);

    }
}
