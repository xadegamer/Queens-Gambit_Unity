using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderClimbing : MonoBehaviour
{
    [SerializeField] private float climbSpeed;
    [SerializeField] float jumpGrace;

    [Header("Detect Ladder")]
    [SerializeField] private float ladderDetectRange;
    [SerializeField] private Transform ladderDetectorPos;
    [SerializeField] private float ladderCheckDetectRange;
    [SerializeField] private Transform ladderCheckDetectorPos;
    [SerializeField] private LayerMask ladderLayer;

    private bool ladderDetected;
    private bool isClimbing;
    private bool canClimb;

    private float inputVertical;
    private Player player;
    private float timer;

    private void Awake() => player = GetComponent<Player>();

    private void Update()
    {
        HandleInput();

        AllowJump();

        if (inputVertical == -1 && player.Movement().IsGrounded()) return; // Return if the player is pressing down when on the ground

        if (Mathf.Abs(inputVertical) > 0f) // Check if the player is presing the up or down button
        {
            CheckForLadderContact();
            CheckForLadderInRange();

            if (ladderDetected) isClimbing = true; //Let the player move Up and Down the ladder
            else
            {
                isClimbing = canClimb = false; // Disables climbing of no ladder is detected
            }
        }

        if(isClimbing) CheckForLadderContact();
    }

    private void FixedUpdate()
    {
        Climb();
    }

    public void HandleInput()
    {
        inputVertical = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space)) JumpFromLaddder();
    }

    public void Climb()
    {
        if (isClimbing) Climbing(); else StopClimbing();
    }

    public void Climbing()
    {
        if (!canClimb) if (inputVertical == 1) inputVertical = 0; // Stop the player from going above the ladder

        if (Mathf.Abs(inputVertical) == 0f)
            player.AnimationController().ChangeAnimationState(player.AnimationController().Climb_IdleAnimation);
        else
            player.AnimationController().ChangeAnimationState(player.AnimationController().ClimbAnimation);

        player.Rigidbody.velocity = new Vector2(player.Rigidbody.velocity.x, inputVertical * climbSpeed * Time.deltaTime);
        player.Rigidbody.gravityScale = 0;

        //The player is at the base of the ladder
        if (inputVertical < 0 && player.Movement().IsGrounded())
        {
            isClimbing = ladderDetected = canClimb = false;
        }
    }

    public void StopClimbing()
    {
        player.Rigidbody.gravityScale = 1;
    }

    public void JumpFromLaddder()
    {
        if (isClimbing || timer > 0 && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
        {
            isClimbing = ladderDetected = canClimb = false;
            player.Movement().Jump();
        }
    }

    public void CheckForLadderContact()
    {
        Collider2D ladderInContact = Physics2D.OverlapCircle(ladderDetectorPos.position, ladderDetectRange, ladderLayer);
        if (ladderInContact == null)
        {
            if (isClimbing) timer = jumpGrace; //Allow the player to jump for some sec 

            ladderDetected = isClimbing = false;

        }  else ladderDetected = true;
    }

    public void CheckForLadderInRange()
    {
        Collider2D ladderInRange = Physics2D.OverlapCircle(ladderCheckDetectorPos.position, ladderCheckDetectRange, ladderLayer);
        if (ladderInRange == null) canClimb = false; else canClimb = true;
    }

    public void AllowJump()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }

    public bool CanClimb() => ladderDetected;
    public bool IsClimbing() => isClimbing;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ladderDetectorPos.position, ladderDetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ladderCheckDetectorPos.position, ladderCheckDetectRange);
    }
}
