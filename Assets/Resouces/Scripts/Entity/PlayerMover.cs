using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Transform _targretToRotate;
    [SerializeField] private float _speed =1.0f;
    [SerializeField] private Animator _anim;
    [SerializeField] private Player _player;

    private const string _horisontal = "Horizontal";
    private const string _vertical = "Vertical";
    
    private Vector3 _moveDirection;
    private Vector3 _moveInput;

    private float _forwardAmount;
    private float _turnAmount;
    private string _Walk = "Walk";
    private string _Alive = "Alive";
    private bool _isAlive = true;



    private void OnEnable()
    {
        _player.OnDie += Die;
    }

    private void OnDisable()
    {
        _player.OnDie -= Die;
    }

    private void Die()
    { 
        _isAlive = false;
        _anim.SetBool(_Alive,false);
    }

    private void Awake()
    {
        _anim = gameObject.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
       if(_isAlive)
            Move();
    }

    private void Move()
    {
        _moveDirection = Input.GetAxis(_vertical) * Vector3.forward + Input.GetAxis(_horisontal) * Vector3.right;
        bool isWalk;

        if (_moveDirection.x != 0 || _moveDirection.z != 0)
            isWalk = true;
        else
            isWalk = false;

        transform.LookAt(new Vector3(_targretToRotate.transform.position.x, transform.position.y, _targretToRotate.transform.position.z));
        transform.position += _moveDirection * _speed * Time.fixedDeltaTime;

        _anim.SetBool(_Walk,isWalk);

        if (_moveDirection.magnitude > 1)
            _moveDirection.Normalize();

        ConvertMoveInput();
        UpdateAimator();
    }

    private void UpdateAimator()
    {
        _anim.SetFloat(_horisontal,_turnAmount,0.1f,Time.deltaTime);
        _anim.SetFloat(_vertical,_forwardAmount,0.1f,Time.deltaTime);
    }

    private void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(_moveDirection);
        _turnAmount = localMove.x;
        _forwardAmount = localMove.z;
    }
}
