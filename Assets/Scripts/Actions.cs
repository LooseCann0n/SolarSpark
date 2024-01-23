using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BehaviourTree;

public static class Actions
{
    #region Enemy Actions
    /// <summary>
    /// Event called when enemy dies
    /// </summary>
    public static Action<SimpleEnemy> OnEnemyKilled;

    /// <summary>
    /// Event called after enemy attacks
    /// </summary>
    public static Action<float> EnemyCooldown;

    /// <summary>
    /// Event called when enemy connects an attack
    /// </summary>
    public static Action<float> ApplyEnemyDamage;

    /// <summary>
    /// Event called when player hits an enemy
    /// </summary>
    public static Action OnEnemyHit;


    #endregion

    #region Player Actions

    /// <summary>
    /// Event called when player dies
    /// </summary>
    public static Action OnPlayerDeath;

    /// <summary>
    /// Event called when player attacks
    /// </summary>
    public static Action OnPlayerAttack;
    /// <summary>
    /// Event called when player is hit
    /// </summary>
    public static Action OnPlayerHit;

    public static Action<Vector3> OnSpecialAttack;

    // Movement Actions

    public static Action<PlayerMovement> OnPlayerMove;
    /// <summary>
    /// Event called when player dashes
    /// </summary>
    public static Action OnPlayerDash;

    public static Action<float> OnHealAction;

    public static Action OnPlayerScreenshot;

    #endregion

}
