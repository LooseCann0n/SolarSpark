using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Subtitle Audio Pair", menuName = "Dialogue/Create new dialogue", order = 1)]
public class DialoguePair : ScriptableObject
{
    public AudioClip clip;
    public string subtitle;
}
