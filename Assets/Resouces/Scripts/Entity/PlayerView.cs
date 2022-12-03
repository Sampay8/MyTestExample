using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    public void SetMotion(PlayerMotion motion) => _animator.Play(motion.ToString());
}

public enum PlayerMotion
{
    RifleIdle,
    WalkForward,
    WalkLeft,
    WalkRight,
    WalkBackward,
    RifleRun,
    RunLeft,
    RunRight,
    Reloading,
    Reload,
    HitReaction,
    Death
}