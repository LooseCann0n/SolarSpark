using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
public class SimpleCombat : MonoBehaviour
{
    #region Debug Values
    [Header("Debug Values")]
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private BoxCollider swordHitbox;
    [SerializeField]
    private Collider blockHitbox;
    [SerializeField]
    private PlayerVoice voiceSource;
    [SerializeField]
    DialoguePair specialAttackLine;
    [SerializeField]
    Camera mainCam;
    Animator combatAnimator;
    Animator swordAnimator;


    ParticleSystem particles;
    GradientAlphaKey[] alphaGradients;

    [SerializeField]
    GameObject specialProjectile;

    PlayerMovement movementScript;
    
    [SerializeField]
    public bool canAttack;
    private bool canBlock;
    bool particlesEnabled;
    bool specialLinePlayed;

    public int attackPointer = 0;
    private Image powerBar;
    private float timeScale;
    [SerializeField]
    private PlayerAudio targetAudio;
    private SwordCollision swordCollision;
    #endregion
    #region Combat Variables
    [Header("Combat and Damage")]
    public int playerDamage;
    public int staggerDamage;
    public bool lightAttack;
    public bool isBlocking;
    public bool isParrying;
    [SerializeField]
    private float parryTime;
    [Header("Special Attack")]
    public int specialDamage;
    public int specialCost;
    public float specialSpeed;
    public float specialTime;
    public Transform specialOrigin;
    public Vector3 specialAttackCameraShake;

    [Header("Heat Bar")]
    [SerializeField]
    private int maxHeat;
    [SerializeField]
    private int noOfHits;
    [SerializeField]
    private int heatPerHit;
    public int currentHeat;
    [SerializeField]
    private Gradient trailColour;
    #endregion

    private void Awake()
    {
        mainCam = Camera.main;
        powerBar = GameObject.Find("PowerMeter").GetComponent<Image>();
        swordHitbox = sword.GetComponent<BoxCollider>();
        combatAnimator = GetComponentInChildren<Animator>();
        particles = sword.GetComponentInChildren<ParticleSystem>();
        movementScript = GetComponent<PlayerMovement>();
        swordCollision = sword.GetComponent<SwordCollision>();
        swordAnimator = sword.GetComponent<Animator>();

        var emissionComponent = particles.emission;
        emissionComponent.enabled = false;
        DisableHitbox();
        EnableAttacking();
        alphaGradients = new GradientAlphaKey[2] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) };
        trailColour.SetKeys(trailColour.colorKeys, alphaGradients);
        var col = particles.colorOverLifetime;
        col.color = trailColour;
        powerBar.fillAmount = 0f;
    }

    public void EnableMovement()
    {
        movementScript.canMove = true;
        canBlock = false;
    }

    public void DisableMovement()
    {
        movementScript.canMove = false;
        canBlock = true;
    }

    public void EnableAttacking()
    {
        canAttack = true;
        combatAnimator.ResetTrigger("Lattack");
        combatAnimator.ResetTrigger("Hattack");
    }

    public void DisableAttacking()
    {
        canAttack = false;
    }

    public void EnableHitbox()
    {
        swordCollision.ClearList();
        swordHitbox.enabled = true;
        var emissionComponent = particles.emission;
        emissionComponent.enabled = true;
        Actions.OnPlayerAttack.Invoke();
    }

    public void DisableHitbox()
    {
        swordHitbox.enabled = false;
        var emissionComponent = particles.emission;
        emissionComponent.enabled = false;
    }

    public void IncreaseHeat(int heatValue)
    {
        if (currentHeat + heatValue >= maxHeat)
        {
            StartCoroutine(AnimateSliderOverTime(0.5f, maxHeat));
        }
        else
            StartCoroutine(AnimateSliderOverTime(0.5f, currentHeat + heatValue));

        alphaGradients[0].alpha = (float)currentHeat / (float)maxHeat;
        trailColour.SetKeys(trailColour.colorKeys, alphaGradients);
        var col = particles.colorOverLifetime;
        col.color = trailColour;       
        currentHeat += heatValue;

        if(currentHeat >= maxHeat)
        {
            currentHeat = maxHeat;
        }
    }

    IEnumerator AnimateSliderOverTime(float seconds, float targetValue)
    {
        float startingHeat = currentHeat;

        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            powerBar.fillAmount = Mathf.Lerp(startingHeat, targetValue, lerpValue) / (float)maxHeat;
            yield return null;
        }
        
    }


    public void EnemyHit(/*GameObject enemy*/)
    {
        IncreaseHeat(heatPerHit);      
    }

    /// <summary>
    /// This function increments the current attack in the sequence and plays the animation
    /// </summary>
    /// <param name="context">this is the context from the control scheme</param>
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && canAttack)
        {
            lightAttack = true;
            //gameObject.transform.rotation = new Quaternion(transform.rotation.x, mainCam.transform.rotation.y, transform.rotation.z, mainCam.transform.rotation.w);
            //turns on the relevant parameter in the animation controller
            combatAnimator.SetTrigger("Lattack");
            swordAnimator.SetTrigger("Shorten");
            swordAnimator.ResetTrigger("Extend");
            //movementScript.canMove = false;
            //RotatePlayer();
            DisableAttacking();
        }

    }

    public void HeavyAttack(InputAction.CallbackContext context)
    {
        if (context.started && canAttack)
        {
            lightAttack = false;
            combatAnimator.SetTrigger("Hattack");
            swordAnimator.SetTrigger("Extend");
            swordAnimator.ResetTrigger("Shorten");
            DisableAttacking();
        }
    }
    public void SpecialAttack(InputAction.CallbackContext context)
    {
        if (context.started && canAttack)
        {
            if (currentHeat >= specialCost)
            {
                DisableAttacking();
                IncreaseHeat(-specialCost);
                combatAnimator.SetTrigger("special");
                movementScript.canMove = false;
                RotatePlayer();
                powerBar.fillAmount = (float)currentHeat / (float)maxHeat;
                if (!specialLinePlayed)
                {
                    voiceSource.PlayClip(specialAttackLine);
                    specialLinePlayed = true;
                }
            }
        }
    }
    
    public void SpawnSpecialAttack()
    {
        targetAudio.PlaySpecialAttackClip();
        Actions.OnSpecialAttack?.Invoke(specialAttackCameraShake);
        gameObject.transform.rotation = new Quaternion(transform.rotation.x, mainCam.transform.rotation.y, transform.rotation.z, mainCam.transform.rotation.w);
        GameObject projectile = Instantiate(specialProjectile, specialOrigin.transform.position, 
            new Quaternion(transform.rotation.x, mainCam.transform.rotation.y, transform.rotation.z, mainCam.transform.rotation.w));
        projectile.transform.parent = null;
        projectile.GetComponent<SpecialProjectile>().SetVariables(specialDamage, specialSpeed, specialTime);
    }

    public void clearAttacks()
    {
        swordAnimator.SetTrigger("Shorten");
        swordAnimator.ResetTrigger("Extend");
        EnableAttacking();
    }

    public void Parry(InputAction.CallbackContext context)
    {
        if(context.started && canBlock == true)
        {
            isBlocking = true;
            blockHitbox.enabled = true;
            swordAnimator.SetTrigger("Shorten");
            swordAnimator.ResetTrigger("Extend");
            combatAnimator.SetBool("parry", true);
            combatAnimator.SetTrigger("parryTrigger");
        }
        else if (context.canceled)
        {
            isBlocking = false;
            blockHitbox.enabled = false;
            combatAnimator.SetBool("parry", false);
            EnableAttacking();
            EnableMovement();
        }
    }

    public IEnumerator ParryTime()
    {
        isParrying = true;
        yield return new WaitForSeconds(parryTime);
        combatAnimator.ResetTrigger("parryTrigger");
        isParrying = false;
    }

    private void RotatePlayer()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, mainCam.transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
