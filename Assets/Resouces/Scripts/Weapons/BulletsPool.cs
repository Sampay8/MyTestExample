using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    [SerializeField] private GameObject _template;
    [SerializeField] private GameObject _pool;
    [SerializeField] private List<Bullet> _bullets = new List<Bullet>();
    [SerializeField] private int _count = 30;

    public Bullet GetBullet()
    {
        Bullet bullet = null;
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (_bullets[i].gameObject.activeInHierarchy == false)
            {
                bullet = _bullets[i];
                break;
            }
        }

        if (bullet == null)
        {
            bullet = CreateBullet();
        }

        return bullet;
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < _count; i++)
            CreateBullet();
    }
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(_template).GetComponent<Bullet>();
        bullet.transform.parent = _pool.transform;
        _bullets.Add(bullet);
        bullet.gameObject.SetActive(false);
        return bullet;
    }
}