using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using Cinemachine;

public class PlayerManager : Character
{
    #region Variables
    public Image healthBar;
    public GameObject deathUI;
    [SerializeField]
    private ResetPlayer resetPlayer;
    public float respawnTime;
    private float respawnTimer;
    public bool alive = true;
    private bool hasToggled;
    private Animator playerAnimator;
    [SerializeField]
    PlayerVoice voice;
    [Header("Healing")]
    [SerializeField]
    float healingReceived;
    [SerializeField]
    float healingTime;
    [SerializeField]
    int powerCost;

    // Player components to toggle upon death

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineInputProvider cameraInput;
    private PlayerMovement playerMovement;
    private SimpleCombat playerCombat;
    private PlayerInput playerInput;
    private CameraLockOn cameraLock;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    private Noclip noclip;

    private Animator uiAnimator;

    public SwordCollision swordColl;
    public AudioClip parrySound;
    public AudioSource parrySource;
    #endregion


    private void OnEnable()
    {
        Actions.ApplyEnemyDamage += ChangeHealth;
    }

    private void OnDisable()
    {
        Actions.ApplyEnemyDamage -= ChangeHealth; 
    }

    protected override void Awake()
    {
        base.Awake();
        noclip = GetComponent<Noclip>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<SimpleCombat>();
        virtualCamera = GameObject.Find("Main CM Vcam").GetComponent<CinemachineVirtualCamera>();
        cameraInput = virtualCamera.GetComponent<CinemachineInputProvider>();
        playerAnimator = GetComponentInChildren<Animator>();
        uiAnimator = deathUI.GetComponent<Animator>();
        cameraLock = Camera.main.GetComponent<CameraLockOn>();
    }

    private void Start()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    private void Update()
    {
        if (!alive)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer > 2.9f)
                uiAnimator.SetTrigger("Reset");
            if (respawnTimer >= respawnTime)
                RespawnPlayer();
        }
    }

    public void ChangeHealth(float amount)
    {
        playerAnimator.SetTrigger("hurt");
        StartCoroutine(AnimateSliderOverTime(0.5f, amount));
        //healthBar.fillAmount = (float)Health / (float)maxHealth;
    }

    IEnumerator AnimateSliderOverTime(float seconds, float targetValue)
    {
        float currentHealth = (float)Health;
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            healthBar.color = Color.red;
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;

            healthBar.fillAmount = Mathf.Lerp(currentHealth, currentHealth - targetValue, lerpValue) / (float)maxHealth;
            yield return null;
        }
        healthBar.color = new Color32(255, 164, 0, 255);
    }

    public override void Die()
    {
        Actions.OnPlayerDeath?.Invoke();
        playerAnimator.SetTrigger("dead");
        playerAnimator.ResetTrigger("respawn");
        gameObject.layer = LayerMask.NameToLayer("Corpse");
        alive = false;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SobekBossRoom")
        {
            GameObject.Find("BossDoorBlock").SetActive(false);
        }
        cameraLock.RemoveLockOn();
        uiAnimator.SetTrigger("Fade");
        rb.constraints = RigidbodyConstraints.FreezeAll;
        InvertPlayerComponents();
        hasToggled = true;
    }

    private void RespawnPlayer()
    {
        uiAnimator.ResetTrigger("Reset");
        hasToggled = false;
        transform.position = resetPlayer.FindClosestCheckpoint().position;
        gameObject.layer = LayerMask.NameToLayer("Player");
        playerAnimator.SetTrigger("respawn");
        playerAnimator.ResetTrigger("dead");
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        InvertPlayerComponents();
        hasToggled = false;       
        Health = maxHealth;
        healthBar.fillAmount = (float)Health / (float)maxHealth;


        respawnTimer = 0;
        alive = true;
    }

    public void HealButton(InputAction.CallbackContext context)
    {
        if (context.performed && playerCombat.currentHeat >= powerCost)
        {
            playerAnimator.SetBool("isHealing", true);
        }
    }

    public void Heal()
    {
        MusicController.musicInstance.EndCombat();
        playerAnimator.SetBool("isHealing", false);
        Actions.OnHealAction?.Invoke(0f);
        StartCoroutine(HealTime());
    }

    public void HealShake()
    {
        playerCombat.IncreaseHeat(-powerCost);
        Actions.OnHealAction?.Invoke(1f);
    }

    IEnumerator HealTime()
    {
        float healingBudget = healingReceived;
        float healingIncrement = Mathf.Lerp(0, healingBudget, Time.deltaTime/healingTime);
        while(healingBudget > 0)
        {
            healthBar.color = Color.green;
            health += healingIncrement;
            healingBudget -= healingIncrement;
            healthBar.fillAmount = health / maxHealth;
            if (health > maxHealth)
                break;
            yield return null;
        }
        healthBar.color = new Color32(255, 164, 0, 255);

    }

    private void InvertPlayerComponents()
    {
        if (hasToggled == true)
            return;
        capsuleCollider.enabled = !capsuleCollider.enabled;
        noclip.enabled = !noclip.enabled;
        playerInput.enabled = !playerInput.enabled;
        playerMovement.enabled = !playerMovement.enabled;
        cameraInput.enabled = !cameraInput.enabled;
        cameraLock.enabled = !cameraLock.enabled;
    }

    public void HitParried()
    {
        playerAnimator.SetTrigger("blockTrigger");
        parrySource.PlayOneShot(parrySound);
        StartCoroutine(swordColl.SparkImpact(0.2f));
    }

    public override void Damage(int amount)
    {
        if (MusicController.musicInstance != null)
            MusicController.musicInstance.StartCombat();
        base.Damage(amount);
        Actions.OnPlayerHit.Invoke();
        Actions.ApplyEnemyDamage.Invoke(amount);
    }

    public void TrapDamage(int amount)
    {
        base.Damage(amount);
        Actions.OnPlayerHit.Invoke();
    }
}
