using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [SerializeField] private List<WeaponsType> _weaponsStorrage;
    [SerializeField] private BulletsPool _pool;
    [SerializeField] private Transform _joinPoint;
    [SerializeField] private float _bulletSpeed = 10.0f;
    [SerializeField] private WeaponsType _type;
    [SerializeField] private GameObject _template;

    public event Action<string, int, int> WeaponsStateChanged;

    public Transform JoinPoint => _joinPoint;
    
    private Coroutine _doShot;
    private Coroutine _doReloading;
    private WaitForSeconds _reloadingDelay = new WaitForSeconds(3);
    private WaitForSeconds _shotDelay;

    private float _damage;
    private int _selected;
    private int _bulletInMagazine;
    
    public void SelectNext()
    {
        _selected--;

        if (_selected < 0)
            _selected = _weaponsStorrage.Count - 1;

        ChangeWeapon(_selected);
    }

    public void SelectPrevius()
    {
        _selected++;

        if (_selected > _weaponsStorrage.Count - 1)
            _selected = 0;

        ChangeWeapon(_selected);
    }

    public void Shoot(Transform target)
    {
        if ( _type.BulletInMagazine > 0 && _doShot == null && target != null && _doReloading == null)
            _doShot = StartCoroutine(DoShot(target));
        else
            Reload();
    }

    public void Reload()
    {
        if(_doReloading == null)
         _doReloading = StartCoroutine(DoReloading());
    }

    private void ChangeWeapon(int select)
    {
        if (_weaponsStorrage[_selected] != null)
        { 
            if (_template != null)
                Destroy(_template);

            _template = Instantiate(_weaponsStorrage[_selected].Template, _joinPoint);
            _type = _weaponsStorrage[_selected];
            _shotDelay = new WaitForSeconds(_type.ShootPause);
            _damage = _type.Damage;
            WeaponsStateChanged?.Invoke(_type.Title, _type.BulletInMagazine, _type.BulletCount);
        }
    }

    private void Start()
    {
        ChangeWeapon(_selected);
    }

    private IEnumerator DoShot(Transform target)
    {
        _type.TrySpendBullet();
        Bullet bullet = _pool.GetBullet();
        bullet.SetDamageValue(_type);
        bullet.transform.position = _template.transform.position;
        bullet.transform.LookAt(target);
        bullet.gameObject.SetActive(true);
        bullet.Rigidbody.velocity = (target.position - _template.transform.position).normalized * _bulletSpeed;
        WeaponsStateChanged?.Invoke(_type.Title, _type.BulletInMagazine, _type.BulletCount);
        yield return _shotDelay;

        StopCoroutine(_doShot);
        _doShot = null;
        
    }

    private IEnumerator DoReloading()
    {
        yield return _reloadingDelay;
        _bulletInMagazine = _type.GetReloading();
        WeaponsStateChanged?.Invoke(_type.Title, _type.BulletInMagazine, _type.BulletCount);
        
        StopCoroutine(_doReloading);
        _doReloading = null;
    }
}