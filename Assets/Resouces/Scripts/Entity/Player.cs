using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Weapons))]
public class Player : MonoBehaviour
{
    public Action<float> OnHealthChanged;
    public Action OnDie;

    private const float _maxHealth = 100f;

    [SerializeField] private Transform _targetRotate;
    [SerializeField] private Transform _targetShot;
    [SerializeField] private float _health;

    private Weapons _weapon;
    private Coroutine _doMoveToShotPoint;
    private float _rayDistance = 100f;

    public void PreviusWeapon() => _weapon.SelectPrevius();

    public void NextWeapon()  => _weapon.SelectNext();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<HealthBox>(out HealthBox box))
            Heal(box.Value);
    }

    private void Start()
    {
        _weapon = gameObject.GetComponent<Weapons>();
        _health = _maxHealth;
    }

    public void Attack()
    {
        if(_health > 0)
            _weapon.Shoot(target: _targetShot);
    }

    private void Heal(float value)
    {
        _health += value;

        if (_health > _maxHealth)
            _health = _maxHealth;

        OnHealthChanged?.Invoke(_health);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health < 0)
        {
            _health = 0;
            OnDie?.Invoke();          
        }

        OnHealthChanged?.Invoke(_health);
    }

    private void FixedUpdate()
    {
        _doMoveToShotPoint = StartCoroutine(DoMoveToShotPoint());
    }

    private IEnumerator DoMoveToShotPoint()
    {
        if (_doMoveToShotPoint != null)
            StopCoroutine(_doMoveToShotPoint);

        Ray ray = new Ray(_weapon.JoinPoint.position, _targetRotate.position - _weapon.JoinPoint.position);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: _rayDistance))
        {
            _targetShot.position = hit.point;
            Debug.DrawRay(_weapon.JoinPoint.position, ray.direction, Color.red);
        }

        yield return null;
    }
}
