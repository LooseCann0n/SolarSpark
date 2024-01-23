using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField]
    private float minShakeMultipler;
    [SerializeField]
    private float maxShakeMultipler;    
    public CinemachineVirtualCamera vCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    private CinemachineImpulseSource impulseSource; // Assign in inspector
    

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        perlinNoise = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlinNoise.m_AmplitudeGain = 0f;
    }

    private void OnEnable()
    {
        Actions.OnPlayerHit += Shake;
        Actions.OnEnemyHit += Shake;
        Actions.OnSpecialAttack += Shake;
        Actions.OnHealAction += HealShake;
    }

    private void OnDisable()
    {
        Actions.OnPlayerHit -= Shake;
        Actions.OnEnemyHit -= Shake;
        Actions.OnSpecialAttack -= Shake;
        Actions.OnHealAction -= HealShake;
    }


    protected virtual void Shake()
    {
        Vector3 velocity = new Vector3 (Random.Range (0.0f, -0.3f), Random.Range (0.0f, -0.5f), 0f);
        impulseSource.GenerateImpulse(velocity);
    }

    protected virtual void Shake(Vector3 velocity)
    {
        velocity = new Vector3(Random.Range(velocity.x / minShakeMultipler, velocity.x * maxShakeMultipler), Random.Range(velocity.y / minShakeMultipler, velocity.y * maxShakeMultipler), Random.Range(velocity.z / minShakeMultipler, velocity.z * maxShakeMultipler));
        impulseSource.GenerateImpulse(velocity);
    }

    private void HealShake(float ampGain)
    {
        perlinNoise.m_AmplitudeGain = ampGain;
    }

}
