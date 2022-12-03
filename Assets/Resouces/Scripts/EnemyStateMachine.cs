using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStateMachine
{
    public Enemy Entity { get; private set; }
    public Transform Target { get; private set; }

    public State Idle;
    public State Run;
    public State Attack;
    public State Damaged;
    public State Death;

    private State _curState;

    public EnemyStateMachine(Enemy entity, Transform target)
    {
        Entity = entity;
        Target = target;

        Idle = new Idle(this);
        Run = new Run(this);
        Attack = new Attack(this);
        Damaged = new Damaged(this);
        Death = new Death(this);

        SetState(Idle);
        Enable();
    }

    private void Enable()
    { 
        Entity.OnEnemyDied += OnDie;
        Entity.OnEnemyDamaged += OnDamage;

    }

    private void OnDie(Enemy sender) 
    {
        Disable();
        SetState(Death);
    }

    private void OnDamage() =>  SetState(Damaged);

    private void Disable()
    { 
        Entity.OnEnemyDied -= OnDie;
        Entity.OnEnemyDamaged = OnDamage;
    }
        
    public void SetState(State state)
    {
        if (_curState != null)
            _curState.Exit();

        _curState = state;
        _curState.Enter();
    }

    public void Update()
    {
        if (_curState != null)
            _curState.Update();
    }

}

public abstract class State
{
    protected EnemyStateMachine SM;
    public State(EnemyStateMachine stateMachine)
    {
        SM = stateMachine;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();
}

public class Idle : State
{
    public Idle(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        SM.Entity.Animator.Play(EnemyMotion.idle.ToString());
    }

    public override void Exit()
    {
        SM.Entity.Animator.StopRecording();
    }

    public override void Update()
    {
        if (SM.Entity.IsCanAttack)
            SM.SetState(SM.Attack);
        else
            SM.SetState(SM.Run);
    }
}

public class Run : State
{
    public Run(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        SM.Entity.Animator.Play(EnemyMotion.run.ToString());
    }

    public override void Exit()
    {
        SM.Entity.Animator.StopRecording();
    }

    public override void Update()
    {
        SM.Entity.ComeCloser();

        if (SM.Entity.IsCanAttack)
            SM.SetState(SM.Attack);
        else
            SM.SetState(SM.Run);
    }
}

public class Attack : State
{
    public Attack(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        SM.Entity.Animator.Play(EnemyMotion.attack.ToString());
    }

    public override void Exit()
    {
        SM.Entity.Animator.StopRecording();
    }

    public override void Update()
    {
        SM.Entity.Attack();

        if (SM.Entity.IsCanAttack == false)
            SM.SetState(SM.Run);
    }
}

public class Damaged : State
{
    private float _waiting = 2.0f;
    private float _delay;
    public Damaged(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        SM.Entity.Animator.Play(EnemyMotion.damaged.ToString());
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        _delay += Time.deltaTime;
        if (_delay > _waiting) 
        {
            _delay = 0;
            SM.SetState(SM.Idle);
        }
    }
}

public class Death : State
{
    public Death(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        SM.Entity.Animator.Play("death");
        SM.Entity.Death();
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}

public enum EnemyMotion
{
    idle,
    run,
    attack,
    damaged,
    death
}