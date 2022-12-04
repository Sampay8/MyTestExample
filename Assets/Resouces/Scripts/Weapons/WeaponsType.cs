using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponType", menuName = "ScriptableObjects/CreateNewWeapon", order = 51)]
public class WeaponsType : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private GameObject _template;
    [SerializeField] private float _shootPause;
    [SerializeField] private float _damage;
    [SerializeField] private int _bulletCount;
    [SerializeField] private int _magazineSize;

    private int _bulletInMagazine;
   
    public int BulletCount => _bulletCount;
    public int BulletInMagazine => _bulletInMagazine;

    public int MagazineSize => _magazineSize;
    public string Title => _title;
    public GameObject Template => _template;
    public float ShootPause => _shootPause;
    public float Damage => _damage;

    public void AddBullet(int value) => _bulletCount += value;

    public bool TrySpendBullet()
    {
        bool isSucses = _bulletInMagazine > 0;
        
        if (isSucses)
            _bulletInMagazine--;

        return isSucses;
    }

    public int GetReloading()
    {
        if (_bulletCount > _magazineSize)
        {
            _bulletCount -= _magazineSize;
            _bulletInMagazine = _magazineSize;
            return _magazineSize;
        }
        else
        {
            _bulletCount = 0;
            _bulletInMagazine = _bulletCount;
            return _bulletCount;
        }
    }
}