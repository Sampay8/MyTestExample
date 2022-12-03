using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private TerrainCollider _groundPlane;

    private bool _isTarget = true;
    private float _rayDistance = 1000f;
    private Coroutine _doMove;

    private void LateUpdate()
    {
        if (_isTarget)
            _doMove = StartCoroutine(DoMove());
    }

    private IEnumerator DoMove()
    {
        if (_doMove != null)
            StopCoroutine(_doMove);

        while (_isTarget)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out RaycastHit hit, maxDistance: _rayDistance))
                _target.transform.position = hit.point;

            yield return null;
        }
    }
}
