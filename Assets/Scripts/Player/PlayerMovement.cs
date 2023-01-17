using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float runSpeed;
    [SerializeField] private KeyCode jumpButton;
    [SerializeField] private KeyCode crouchButton;
    [SerializeField] float extraDetectionCheck;

    [Header("Standing State")]
    [SerializeField] private Vector2 standingColliderOffset;
    [SerializeField] Vector2 standingColliderSize;

    [Header("Standing State")]
    [SerializeField] private Vector2 crouchColliderOffset;
    [SerializeField] private Vector2 crouchColliderSize;

    private float inputHorizontal;
    private float speed;
    private bool isCrouching;

    private Player player;

    private void Awake() => player = GetComponent<Player>();

    private void Start()
    {
        speed = runSpeed;
    }

    private void Update()
    {
        HandleInput();
        MoveAnimation();
        HandleAir();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void HandleInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded()) Jump();

        if (Input.GetButtonDown("Crouch") && IsGrounded()) EnableCrouch();

        if (Input.GetButtonUp("Crouch") && IsGrounded()) DisableCrouch();
    }

    public void Move()
    {
        if (player.IsDead() || isCrouching) return;
        player.Rigidbody.velocity = new Vector2(inputHorizontal * (!player.LadderClimbing().IsClimbing() ? speed : speed /2)  * Time.deltaTime, player.Rigidbody.velocity.y);
        if (inputHorizontal != 0) transform.localScale = new Vector3(inputHorizontal < 0 ? -1 : 1, 1, 1); //Flip player
    }

    public void Jump()
    {
        if (player.IsDead() || isCrouching) return;
        player.Rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        player.AnimationController().ChangeAnimationState(player.AnimationController().Jump_StartAnimation);
    }

    public void MoveAnimation()
    {
        if (player.IsDead()) return;
        if (IsGrounded() && !player.HitPlaying() && !player.Actions().IsAttacking() && !player.LadderClimbing().IsClimbing() && !isCrouching)
        {
            if (player.Rigidbody.velocity == Vector2.zero)
            {
                player.AnimationController().ChangeAnimationState(player.AnimationController().IdleAnimation);
            }
            else if (inputHorizontal != 0)
            {
                player.AnimationController().ChangeAnimationState(player.AnimationController().RunAnimation);
            }
        }
    }

    public void EnableCrouch()
    {
        isCrouching = true;
        player.Rigidbody.velocity = Vector2.zero;
        player.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        SetColliderSizeAndOffset(crouchColliderOffset, crouchColliderSize);
        player.AnimationController().ChangeAnimationState(player.AnimationController().CroushAnimation);
    }

    public void DisableCrouch()
    {
        SetColliderSizeAndOffset(standingColliderOffset, standingColliderSize);
        player.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        isCrouching = false;
    }

    public void SetColliderSizeAndOffset(Vector2 newOffset, Vector2 newSize)
    {
        player.Collider.offset = newOffset;
        (player.Collider as CapsuleCollider2D).size = newSize;
    }

    public bool IsGrounded()
    {
        //RaycastHit2D hit = Physics2D.BoxCast(player.Collider.bounds.center, player.Collider.bounds.size, 0, Vector2.down, 0.1f, player.GroundLayer);

        RaycastHit2D hit = Physics2D.CapsuleCast(player.Collider.bounds.center, player.Collider.bounds.size,CapsuleDirection2D.Vertical, 0, Vector2.down, extraDetectionCheck, player.GroundLayer);
        return hit.collider != null;
    }

    public void HandleAir()
    {
        if (IsFalling() && !player.LadderClimbing().IsClimbing())
            player.AnimationController().ChangeAnimationState(player.AnimationController().Jump_EndAnimation);
    }

    public bool IsFalling()
    {
        if (player.Rigidbody.velocity.y < 0 && !IsGrounded()) return true;
        else return false;
    }

    public bool IsClimbing() => player.LadderClimbing().IsClimbing();

    public void SetSpeed(float newValue) => speed = newValue;

    public float CurrentSpeed() => speed;

    public bool IsCrouching() => isCrouching;
}
