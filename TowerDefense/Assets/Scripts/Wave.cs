using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Objects sind die beste Methode, um in Unity schnell eigene Daten zu verpacken.
/// Da eine Welle an Gegnern nichts anderes als Daten ist, ist dies hier ein perfekter Usecase!
/// </summary>
[CreateAssetMenu(fileName = "creep_wave.asset")]
public class Wave : ScriptableObject
{
    //Statt GameObject kann man hier auch ein beliebiges MonoBehaviour als referenz angeben,
    //sodass der Nutzer nur richtige Referenzen zuweisen kann!
    //In diesem Fall wollen wir nur Prefabs mit dem Creep-Script
    public Creep creepPrefab => _creepPrefab;
    [SerializeField] Creep _creepPrefab;

    public int numberOfSpawns => _numberOfSpawns;
    [SerializeField] int _numberOfSpawns;

    public float spawnInterval => _spawnInterval;
    [SerializeField] float _spawnInterval = 1.0f;
}
