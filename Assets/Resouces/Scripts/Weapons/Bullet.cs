using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _blood;

    private Rigidbody _rb;
    private SphereCollider _collider;
    private float _damage;
    private WaitForSeconds _delayForDisable = new WaitForSeconds(2.2f);

    public void SetDamageValue(WeaponsType weapon) => _damage = weapon.Damage;
    public Rigidbody Rigidbody => _rb;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        _collider.enabled = true;
        StartCoroutine(DoDisableWaiting());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _collider.enabled = false;
            _blood.Play();
            enemy.TakeDamage(_damage);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DoDisableWaiting()
    {
        yield return _delayForDisable;
        gameObject.SetActive(false);
    }
}