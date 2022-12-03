using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _bullet;

    private void LateUpdate()
    {
        transform.LookAt(_enemy.transform);

        if (Input.GetKeyDown(KeyCode.Space))
        { 
            GameObject bullet = Instantiate(_bullet);
            bullet.transform.position = transform.position;
            Rigidbody rigidbody = bullet.AddComponent<Rigidbody>();
            rigidbody.AddForce(_enemy.transform.position - transform.position, ForceMode.Impulse);
            
        }
    }
}
