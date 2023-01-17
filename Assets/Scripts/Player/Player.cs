using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public Rigidbody2D Rigidbody { get => rb; }
    public Collider2D Collider { get => col; }
    public Animator Anim { get => anim; }
    public LayerMask GroundLayer { get => groundLayer; }

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundLayer;

    private PlayerActions playerActions;
    private PlayerLadderClimbing ladderClimbing;
    private PlayerMovement playerMovement;
    private PlayerAnimationController playerAnimation;
    private Damageable damageable;
    private bool hitPlaying = false;
    private bool isDead = false;

    public PlayerActions Actions() => playerActions;
    public PlayerLadderClimbing LadderClimbing() => ladderClimbing;
    public PlayerMovement Movement() => playerMovement;
    public PlayerAnimationController AnimationController() => playerAnimation;
    public Damageable GetDamageable() => damageable;

    public bool HitPlaying() => hitPlaying;
    public bool IsDead() => isDead;


    private void Awake()
    {
        Instance = this;
        playerActions = GetComponentInChildren<PlayerActions>();
        playerAnimation = GetComponentInChildren<PlayerAnimationController>();
        playerMovement = GetComponent<PlayerMovement>();
        ladderClimbing = GetComponent<PlayerLadderClimbing>();
        damageable = GetComponent<Damageable>();
    }

    public void Start()
    {
        damageable.OnHitEvent().AddListener(DamageTaken);
        damageable.OnDeadEvent().AddListener(Died);
        damageable.OnHealEvent().AddListener(Heal);
    }

    public void Heal()
    {
        UIManager.Instance.SetHealthBar(damageable.HealthPercentage());
    }

    public void DamageTaken()
    {
        UIManager.Instance.SetHealthBar(damageable.HealthPercentage());

        if (!playerMovement.IsCrouching() && !ladderClimbing.IsClimbing())
        {
            playerAnimation.ChangeAnimationState(playerAnimation.HurtAnimation);
            StartCoroutine(nameof(DamageCooldown));
        }
    }

    public void KockBack()
    {
        if (transform.localScale.x == 1) rb.MovePosition(rb.position + new Vector2(-1f, 0));
        else rb.MovePosition(rb.position + new Vector2(1f, 0));
    }

    public IEnumerator DamageCooldown()
    {
        hitPlaying = true;
        yield return new WaitForSeconds(.5f);
        hitPlaying = false;
    }
    public void Died()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        playerAnimation.ChangeAnimationState(playerAnimation.DeadAnimation);

        LevelManager.Instance.GameOver();
    }
}
