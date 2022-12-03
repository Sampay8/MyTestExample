using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWawe", menuName = "ScriptableObjects/CreateNewWawe", order = 51)]
public class Wave : ScriptableObject
{
    [SerializeField] private List<GameObject> _templates;
    [SerializeField] private int _count;
    [SerializeField] private int _countInScene;


    public int EnemyCount => _count;
    public int CountInScene => _countInScene;
    public List<GameObject> Templates => _templates;


}
