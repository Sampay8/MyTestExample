using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class Enemy : MonoBehaviour
{
    public Action<Enemy> OnEnemyDied;
    public Action OnEnemyDamaged;

    [SerializeField] private Player _player;
    [SerializeField] private float _speed = .5f;
    [SerializeField] private float _damage = 3.0f;
    [SerializeField] private float _startHealth = 100.0f;

    private EnemyPool _pool;
    private EnemyStateMachine _stateMacine;
    private CapsuleCollider _collider;
    private Rigidbody _rigidbody;
    private float _health;

    private WaitForSeconds _findDelay = new WaitForSeconds(.02f);
    private WaitForSeconds _attackDelay = new WaitForSeconds(.5f);
    private WaitForSeconds _activateDelay = new WaitForSeconds(3f);

    private Coroutine _doWalk;
    private Coroutine _doAttack;
    private Vector2 _targetPosition;
    private Vector2 _curPosition;
    private float _attackDistance = 1f;

    public Animator Animator { get; private set; }
    public bool IsCanAttack => Vector3.Distance(transform.position, _player.transform.position) <= _attackDistance;

    private void Awake()
    {
        if (_collider == null)
            _collider = gameObject.GetComponent<CapsuleCollider>();
        if (_rigidbody == null)
            _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _health = _startHealth;
        _collider.enabled = true;

        StartCoroutine(DoActivate());
        _rigidbody.useGravity = true;
    }

    private IEnumerator DoActivate()
    {
        yield return _activateDelay;
        if (Animator == null)
            Animator = GetComponentInChildren<Animator>();

        _stateMacine = new EnemyStateMachine(this, _player.transform);
    }

    private void LateUpdate()
    { 
        if(_stateMacine != null)
            _stateMacine.Update();
    }

    private IEnumerator DoDeath()
    {
        _collider.enabled = false;
        _rigidbody.useGravity = false;
        Animator.Play(EnemyMotion.death.ToString());
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

    private void Walk()
    {
        _targetPosition = new Vector2(_player.transform.position.x, _player.transform.position.z);

        if (Vector3.Distance(transform.position, _player.transform.position) > _attackDistance)
            _doWalk = StartCoroutine(DoWalk(_player.transform));
    }

    private IEnumerator DoWalk(Transform target)
    {
        if (_doWalk != null)
            StopCoroutine(_doWalk);

        while (Vector3.Distance(_curPosition, _targetPosition) > _attackDistance)
        {
            _curPosition = new Vector2(transform.position.x, transform.position.z);
            _curPosition.x = Mathf.MoveTowards(_curPosition.x, _targetPosition.x, _speed * Time.fixedDeltaTime);
            _curPosition.y = Mathf.MoveTowards(_curPosition.y, _targetPosition.y, _speed * Time.fixedDeltaTime);

            transform.position = new Vector3(Mathf.MoveTowards(_curPosition.x, _targetPosition.x, _speed * Time.fixedDeltaTime),
                                             transform.position.y,
                                             Mathf.MoveTowards(_curPosition.y, _targetPosition.y, _speed * Time.fixedDeltaTime));

            transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
            yield return _findDelay;
        }

        WaitForSeconds delay = new WaitForSeconds(1);
        _doWalk = null;
    }

    private IEnumerator DoAttack()
    {
        if (IsCanAttack)
            _player.TakeDamage(_damage);

        yield return _attackDelay;
        StopCoroutine(_doAttack);
        _doAttack = null;
    }

    public void Init(Player target) => _player = target;

    public void ComeCloser() => Walk();

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            OnEnemyDied?.Invoke(this);
        else
            OnEnemyDamaged?.Invoke();

    }
    public void Attack()
    {
        transform.LookAt(_player.transform.position);

        if (_doAttack == null)
            _doAttack = StartCoroutine(DoAttack());
    }

    public void Death() => StartCoroutine(DoDeath());
}