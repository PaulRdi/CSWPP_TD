using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "game_settings.asset")]
public class GameSettings : ScriptableObject
{
    public int maxLives => _maxLives;
    [SerializeField] int _maxLives;

    public List<Wave> waves;
}
