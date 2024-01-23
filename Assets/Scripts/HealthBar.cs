using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    protected Camera cam;

    protected virtual void Start()
    {
        bar = transform.Find("Bar");
        cam = Camera.main;
        bar.localScale = Vector3.one;
        Mathf.Clamp(bar.localScale.x, 0, 1);
    }

    protected virtual void FixedUpdate()
    {
         transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }

    public virtual void SetSize(float sizeNormalised)
    {
        float currentScale = bar.localScale.x;
        Vector3 barValue = new Vector3(Mathf.Lerp(currentScale, sizeNormalised, 0.1f), 1f);       
        bar.localScale = barValue;
    }
}
