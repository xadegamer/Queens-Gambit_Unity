using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Basic Animation")]
    public AnimationClip IdleAnimation;
    public AnimationClip RunAnimation;

    [Header("LadderClimbing Animation")]
    public AnimationClip ClimbAnimation;
    public AnimationClip Climb_IdleAnimation;

    [Header("Ledge Climbing Animation")]
    public AnimationClip LedgeGrabAnimation;
    public AnimationClip LedgeIdleAnimation;

    [Header("Jumping Animation")]
    public AnimationClip Jump_StartAnimation;
    public AnimationClip Jump_FallAnimation;
    public AnimationClip Jump_EndAnimation;

    [Header("Attacking Animation")]
    public AnimationClip AttackAnimation;
    public AnimationClip DashAttackAnimation;

    [Header("Other Animation")]
    public AnimationClip CroushAnimation;
    public AnimationClip DashAnimation;
    public AnimationClip HurtAnimation;
    public AnimationClip DeadAnimation;

    string currentAnimationName;
    Animator animator;

    private void Awake() => animator = GetComponent<Animator>();
    public void ChangeAnimationState(AnimationClip newAnimation)
    {
        if (currentAnimationName == newAnimation.name) return; //Stop the same animation from interupting itself
        animator.Play(newAnimation.name); //Play new animation
        currentAnimationName = newAnimation.name; // Reassign the current state
    }

    public float CurrentAnimationDuration() => animator.GetCurrentAnimatorStateInfo(0).length;
}
