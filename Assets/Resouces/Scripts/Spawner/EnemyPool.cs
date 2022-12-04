using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    private List<Enemy> _enemies;
    private EmemySpawner _spawner;
    private System.Random _rand = new System.Random();

    public void Init(Wave wawe, Player player, EmemySpawner spawner)
    {
        _spawner = spawner;
        ReBoot(wawe, player);
    }

    public void ReBoot(Wave wawe, Player player)
    {
        if (_enemies != null)
        {
            foreach (Enemy enemy in _enemies)
            {
                enemy.Died -= _spawner.OnEnemyDie;
                Destroy(enemy.gameObject);
            }
        }

        _enemies = new List<Enemy>();

        for (int i = 0; i < wawe.EnemyCount; i++)
        {
            Enemy enemy = Instantiate(wawe.Templates[(_rand.Next(1, wawe.Templates.Count)) - 1].GetComponent<Enemy>());
            enemy.Init(target: player);
            enemy.gameObject.SetActive(false);
            enemy.transform.parent = this.transform;
            _enemies.Add(enemy);
            enemy.Died += _spawner.OnEnemyDie;
        }
    }

    public bool TryGetRandomEnemy(out Enemy enemy)
    {
        bool isSucses = false;
        int number = _rand.Next(0, _enemies.Count - 1);
        enemy = _enemies[number];
        isSucses = enemy.gameObject.activeInHierarchy == false;

        return isSucses;
    }
}