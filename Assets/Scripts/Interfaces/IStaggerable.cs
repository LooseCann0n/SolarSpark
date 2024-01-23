using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaggerable
{
    float Stance { get; set; }

    public void Stagger(int staggerAmount, float stunTime);
}
