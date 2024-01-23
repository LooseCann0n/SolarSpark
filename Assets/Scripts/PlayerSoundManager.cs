using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    /// <summary>
    /// Separate script from player manager for sound actions
    /// </summary>
    [Header("Combat Audio")]

    [SerializeField]
    private List<AudioClip> deathSounds;
    [SerializeField]
    private List<AudioClip> hurtSounds;
    [SerializeField]
    private List<AudioClip> attackSounds;

    [SerializeField]
    private PlayerVoice voice;

    private void OnEnable()
    {
        Actions.OnPlayerDeath += PlayDeathSound;
        Actions.OnPlayerHit += PlayHurtSound;
        Actions.OnPlayerAttack += PlayAttackSound;
    }

    private void OnDisable()
    {
        Actions.OnPlayerDeath -= PlayDeathSound;
        Actions.OnPlayerHit -= PlayHurtSound;
        Actions.OnPlayerAttack -= PlayAttackSound;
    }


    public void PlayHurtSound()
    {
        voice.PlaySecondaryClip(hurtSounds[Random.Range(0, hurtSounds.Count)]);
    }

    public void PlayAttackSound()
    {
        voice.PlaySecondaryClip(attackSounds[Random.Range(0, attackSounds.Count)]);
    }

    public void PlayDeathSound()
    {
        voice.PlayClip(deathSounds[Random.Range(0, deathSounds.Count)]);
    }
}
