using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Weapons))]
public class Player : MonoBehaviour
{
    public event Action<float> HealthChanged;
    public event Action Die;

    private const float _maxHealth = 100f;

    [SerializeField] private Transform _targetRotate;
    [SerializeField] private Transform _targetShot;
    [SerializeField] private float _health;

    private Weapons _weapon;

    internal void ReloadWeapon() => _weapon.Reload();

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
        _health = Mathf.Clamp(_health + value, 0, _maxHealth);
        HealthChanged?.Invoke(_health);
    }

    public void TakeDamage(float damage)
    {
         _health = Mathf.Clamp(_health - damage, 0, _maxHealth);

        if (_health <= 0)
            Die?.Invoke();          

        HealthChanged?.Invoke(_health);
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
