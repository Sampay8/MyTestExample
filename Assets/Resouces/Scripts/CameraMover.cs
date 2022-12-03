using System.Collections;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private float _camDeltaHorizontal = 5.0f;
    [SerializeField] private float _camDeltaVertical = 5.0f;

    private const float _speedMultiply = 0.7f;

    private Coroutine _runCoroutine;
    private Vector3 _targetValue = new Vector3();
    private Vector3 _curValue = new Vector3();
    private float _speed;

    private void Start()
    {
        _curValue = _camera.transform.position;
        _targetValue = _player.transform.position;
        SetPosition(_targetValue);
    }

    private void SetPosition(Vector3 pos) => _camera.transform.position = new Vector3(pos.x, _camera.transform.position.y, pos.y - _camDeltaHorizontal);

    private void LateUpdate()
    {
        _targetValue = new Vector3(_player.transform.position.x,_player.transform.position.y + _camDeltaVertical ,_player.transform.position.z - _camDeltaHorizontal);

        if (_curValue != _targetValue)
            MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (_runCoroutine != null)
            StopCoroutine(_runCoroutine);

        _runCoroutine = StartCoroutine(DoMoveCamera());
    }

    private IEnumerator DoMoveCamera()
    {
        while (_curValue != _targetValue)
        {
            _speed = Vector3.Distance(_curValue, _targetValue);
            _curValue = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z);

            _curValue.x = Mathf.MoveTowards(_curValue.x, _targetValue.x, _speed * _speedMultiply * Time.deltaTime);
            _curValue.y = Mathf.MoveTowards(_curValue.y, _targetValue.y, _speed * _speedMultiply * Time.deltaTime);
            _curValue.z = Mathf.MoveTowards(_curValue.z, _targetValue.z, _speed * _speedMultiply * Time.deltaTime);

            _camera.transform.position = new Vector3(Mathf.MoveTowards(_curValue.x, _targetValue.x, _speed * Time.deltaTime),
                                                     Mathf.MoveTowards(_curValue.y, _targetValue.y, _speed * Time.deltaTime),
                                                     Mathf.MoveTowards(_curValue.z, _targetValue.z, _speed * Time.deltaTime));
            yield return null;
        }
    }
}
