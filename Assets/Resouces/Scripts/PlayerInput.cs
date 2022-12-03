using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player _player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            _player.PreviusWeapon();
        if (Input.GetKeyDown(KeyCode.E))
            _player.NextWeapon();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            _player.Attack();
    }
}