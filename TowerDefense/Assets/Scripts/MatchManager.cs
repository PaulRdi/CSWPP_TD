using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Der Match Manager initialisiert den Start einer Runde.
/// Es ist grundsätzlich gut, in einer Szene eine "Reset"-Methode aufrufen zu können und das Spiel ohne Probleme neu starten zu können.
/// Dies kann u.A. dadurch umgesetzt werden, dass man MatchManager und GameManager trennt. In einer Init-Methode müssen wir dafür sorgen,
/// dass alle Elemente des Spiels in den Ausgangs-Zustand gesetzt werden.
/// </summary>
public class MatchManager : MonoBehaviour
{
    [SerializeField] Transform pathParent;
    List<Transform> path;
    [SerializeField] GameSettings settings;

    int currentWaveIndex = 0;
    Wave currentWave;
    int currentCreepInWave;

    void Init()
    {
        path = new List<Transform>();

        //Iteriert über jedes Child des Transforms pathParent
        foreach(Transform t in pathParent)
        {
            path.Add(t);
        }
        currentWaveIndex = 0;
        StartCoroutine(MatchCoroutine());
    }

    
    private void Awake()
    {
        //Vorerst führen wir Init einfach in Awake aus. Später ist es aber denkbar Init() von unterschiedlichen
        //Orten auszuführen!
        Init();
    }

    IEnumerator MatchCoroutine()
    {
        //Wir wollen (mit Zeitverzögerung über jede Wave iterieren, daher ist hier eine For-Schleife angebracht!
        for (currentWaveIndex = 0; currentWaveIndex < settings.waves.Count; currentWaveIndex++)
        {
            currentWave = settings.waves[currentWaveIndex];
            yield return StartCoroutine(WaveRoutine());
        }
    }

    IEnumerator WaveRoutine()
    {
        //Ansonsten gleiches Pattern wie im match, bloß dass wir hier über jeden Creep iterieren!
        //Sowohl die creeps in der wave als auch die Waves sind hier Member der klasse (und werden nicht wie üblich erst in der For-Schleife deklariert),
        //weil es gut sein könnte, dass wir diese Dinge später in der UI anzeigen wollen!
        for (currentCreepInWave = 0; currentCreepInWave < currentWave.numberOfSpawns; currentCreepInWave++)
        {
            Creep creep = Instantiate(currentWave.creepPrefab);
            creep.Init(path);
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
    }
}
