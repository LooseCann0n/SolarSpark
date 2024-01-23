using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrail : MonoBehaviour
{
    public float maxTrailTime;
    float activeTime = 2f;

    bool isTrailActive;
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform meshSpawn;

    public Material trailMat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefresh = 0.05f;

    [SerializeField]
    private SkinnedMeshRenderer[] meshRenderers;

    private void Start()
    {
        activeTime = maxTrailTime;
    }
    public void StartTrail()
    {
        isTrailActive = true;
        StartCoroutine(ActivateTrail());
    }
    IEnumerator ActivateTrail ()
    {
        while (activeTime > 0 && isTrailActive)
        {
            activeTime -= meshRefreshRate;

            for(int i = 0; i < meshRenderers.Length; i++)
            {
                GameObject tempMeshObject = new GameObject();
                tempMeshObject.transform.SetPositionAndRotation(meshSpawn.position, meshSpawn.rotation);

                MeshRenderer renderer = tempMeshObject.AddComponent<MeshRenderer>();
                MeshFilter filter = tempMeshObject.AddComponent<MeshFilter>();

                Mesh newMesh = new Mesh();
                meshRenderers[i].BakeMesh(newMesh);

                filter.mesh = newMesh;
                renderer.material = trailMat;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                StartCoroutine(AnimateMaterialFade(renderer.material, 0, shaderVarRate, shaderVarRefresh));
                Destroy(tempMeshObject, meshDestroyDelay);
            }
            yield return new WaitForSeconds(meshRefreshRate);
        }
        isTrailActive = false;
        activeTime = maxTrailTime;
    }

    IEnumerator AnimateMaterialFade(Material mat, float end, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);
        while(valueToAnimate > end)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
