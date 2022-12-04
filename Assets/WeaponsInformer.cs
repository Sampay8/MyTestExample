using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WeaponsInformer : MonoBehaviour
{
    [SerializeField] private Weapons _weapons;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _ammo;

    private void Start()
    {
        _weapons.WeaponsStateChanged += OnVeaponsStateChange;
    }

    private void OnVeaponsStateChange(string weaponsTitle, int magazineAmmo, int bulletsCount)
    {
        _title.text = weaponsTitle;
        _ammo.text = magazineAmmo + " / " + bulletsCount;
    }
}