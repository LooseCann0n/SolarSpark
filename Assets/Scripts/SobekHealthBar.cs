using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SobekHealthBar : HealthBar
{
    private Image healthBar;

    protected override void Start()
    {
        healthBar = transform.Find("Bar").GetComponentInChildren<Image>();
        cam = Camera.main;
    }

    protected override void FixedUpdate()
    {
        
    }

    public override void SetSize(float sizeNormalised)
    {
        float currentScale = healthBar.fillAmount;
        float barValue = Mathf.Lerp(currentScale, sizeNormalised, 0.1f);
        healthBar.fillAmount = barValue;
    }
}
