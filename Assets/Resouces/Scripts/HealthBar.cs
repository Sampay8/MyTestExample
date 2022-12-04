using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Player _player;
    [SerializeField] private float _recoveryRate;

    private Coroutine _runCoroutine;
    private float _targetValue;

    public void OnValueChange(float value) =>SetValue(value);

    private void OnEnable()
    {
        _player.HealthChanged += OnValueChange;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnValueChange;
    }

    private void SetValue(float value)
    {
        _targetValue = value / 100;

        if (_runCoroutine != null)
            StopCoroutine(_runCoroutine);

        _runCoroutine = StartCoroutine(DoUpdateSliderValue());
    }

    private IEnumerator DoUpdateSliderValue()
    {
        while (_slider.value != _targetValue)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _targetValue, _recoveryRate * Time.deltaTime);
            yield return null;
        }
    }
}