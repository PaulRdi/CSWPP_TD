using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creep enthält das Verhalten und die Stats eines Creeps!
/// </summary>
public class Creep : MonoBehaviour
{
    //Die events OnDeath und OnGoalReached notifizieren andere Systeme (z.B. das Targeting des Turms), dass wir nicht mehr existieren.
    //Als Parameter wird der sterbende / ziel erreichende Creep mitgegeben.
    public event Action<Creep> OnDeath;
    public event Action<Creep> OnGoalReached;

    List<Transform> path;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float minDistToGoal = .1f;
    [SerializeField] float _maxHP;
    float hp;
    public void Init(List<Transform> path)
    {
        //Da die Variable "path" im Parameter genau gleich wie die Variable "path" in der Klasse heißt, ist der "this"-operator hier zwingend nötig,
        //damit der compiler weiß, um welche Variable es geht!
        this.path = path;
        transform.position = path[0].position;
        hp = _maxHP;
        StartCoroutine(MoveAlongPathRoutine());
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            OnDeath?.Invoke(this);
            Destroy(this.gameObject);
        }
    }

    IEnumerator MoveAlongPathRoutine()
    {
        //Hier ist es wichtig, bis path.Count - 1 zu iterieren, da wir immer bis zum nächsten Pfad element gehen wollen.
        for (int i = 0; i < path.Count - 1; i++)
        {
            yield return StartCoroutine(MoveTowardsPoint(path[i + 1]));
        }
        //Wenn wir das Ziel erreichen das entsprechende Event ausführen!
        //Die Syntax mit dem "?" macht einen Nullcheck vor dem Event!
        OnGoalReached?.Invoke(this);
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
