using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] float attackDelay;

    private float attacktimer;
    private bool attacking;
    private Player player;

    private void Awake() => player = GetComponentInParent<Player>();

    private void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        if (player.IsDead() || attacking) return;

        if (Input.GetMouseButtonDown(0) && attacktimer <= 0)
        {
            NormalAttack();
            attacktimer = attackDelay;
        }
        else attacktimer -= Time.deltaTime;
    }

    public void NormalAttack()
    {
        if(!player.Movement().IsGrounded() || player.Movement().IsCrouching()|| player.Movement().IsClimbing()) return;
        attacking = true;
        player.AnimationController().ChangeAnimationState(player.AnimationController().AttackAnimation);
        StartCoroutine(AttackEnd(player.AnimationController().AttackAnimation.length));
    }

    public void DashAttack()
    {
        if (!player.Movement().IsGrounded() || player.Movement().IsCrouching() || player.Movement().IsClimbing()) return;
        attacking = true;

        if (transform.parent.localScale.x == 1) player.Rigidbody.MovePosition((Vector2) transform.parent.position + new Vector2(1f, 0));
        else player.Rigidbody.MovePosition((Vector2)transform.parent.position + new Vector2(-1f, 0));

        player.AnimationController().ChangeAnimationState(player.AnimationController().DashAttackAnimation);
        StartCoroutine(AttackEnd(player.AnimationController().DashAttackAnimation.length));
    }

    IEnumerator AttackEnd(float delay)
    {
        player.Movement().SetSpeed(player.Movement().CurrentSpeed() / 3);
        yield return new WaitForSeconds(delay);
        attacking = false;
        player.Movement().SetSpeed(player.Movement().CurrentSpeed() * 3);
    }

    public bool IsAttacking() => attacking;
}
