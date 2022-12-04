using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private EmemySpawner _spawner;
    [SerializeField] private float _recoveryRate =1.0f;

    private Coroutine _runCoroutine;
    private float _targetValue;
    private string _wave = "Волна: ";
    private int _curWave = -1;

    private void OnEnable() =>_spawner.EnemiesCountChange += OnValueChange;

    private void OnDisable() =>_spawner.EnemiesCountChange -= OnValueChange;

    private void OnValueChange(int wave, int value)
    {
        if (_curWave != wave)
        {
            _curWave = wave;
            float maxValue = _spawner.Waves[_curWave].EnemyCount;
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
            _waveText.text = _wave + (_curWave +1);
        }
        SetValue(value);
    }

    private void SetValue(int value)
    {
        _targetValue =(float) value;

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