using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep : MonoBehaviour
{
    List<Transform> path;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float minDistToGoal = .1f;

    public void Init(List<Transform> path)
    {
        //Da die Variable "path" im Parameter genau gleich wie die Variable "path" in der Klasse heißt, ist der "this"-operator hier zwingend nötig,
        //damit der compiler weiß, um welche Variable es geht!
        this.path = path;
        transform.position = path[0].position;

        StartCoroutine(MoveAlongPathRoutine());
    }

    IEnumerator MoveAlongPathRoutine()
    {
        //Hier ist es wichtig, bis path.Count - 1 zu iterieren, da wir immer bis zum nächsten Pfad element gehen wollen.
        for (int i = 0; i < path.Count - 1; i++)
        {
            yield return StartCoroutine(MoveTowardsPoint(path[i + 1]));
        }
        Destroy(this.gameObject);
    }

    IEnumerator MoveTowardsPoint(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        //Achtung! Wenn minDistToGoal zu klein ist oder speed zu groß ist, kann es sein, dass der Creep sein Ziel nie erreicht.
        while (Vector3.Distance(target.position, transform.position) > minDistToGoal)
        {
            transform.Translate(dir * Time.deltaTime * speed);
            yield return null;
        }
    }
}
