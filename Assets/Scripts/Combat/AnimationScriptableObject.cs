using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "animationLoadout", menuName = "ScriptableObjects/Create New Animation Loadout", order = 1)] 
public class AnimationScriptableObject : ScriptableObject
{
    public int numberOfAttacks;
    public AnimationClip idleAnimation;

    public List<AnimationClip> lightAttacks;
    public List<AnimationClip> heavyAttacks;

}
