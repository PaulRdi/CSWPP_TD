using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Der TowerController enthält verhalten und parameter eines Turms.
/// Parameter sind, wie schnell der Turm schießt und welche Bullet instantiiert wird, wenn der Turm schießt.
/// </summary>
public class TowerController : MonoBehaviour
{
    List<Creep> targets;

    public float shotInterval => _shotInterval;
    [SerializeField] float _shotInterval;
    [SerializeField] Transform _muzzle;
    [SerializeField] Bullet _bullet;

    //interner timer, der die schuss frequenz stoppt.
    float t;

    private void Awake()
    {
        targets = new List<Creep>();
        t = 0.0f;
    }

    private void Update()
    {
        //Wir wollen nichts machen, wenn wir kein target haben!
        if (targets == null ||
            targets.Count == 0) return;

        if (t >= shotInterval)
        {
            ShootAt(targets[0]);
            t = 0;
        }
        t += Time.deltaTime;
    }

    private void ShootAt(Creep creep)
    {
        Bullet bullet = Instantiate(_bullet);
        bullet.transform.position = _muzzle.position;
        bullet.Init(creep);
    }

    private void OnTriggerEnter(Collider other)
    {

        //wir halten uns eine liste an targets. Wir schießen immer auf Element 0 in der liste.
        //aber man könnte die Liste auch sortieren (nach distanz z.B.)
        if (other.TryGetComponent<Creep>(out Creep creep))
        {
            targets.Add(creep);
            creep.OnDeath += RemoveCreepFromTargeting;
            creep.OnGoalReached += RemoveCreepFromTargeting;

        }
    }

    private void RemoveCreepFromTargeting(Creep creep)
    {
        if (targets.Contains(creep))
        {
            //Da jeder Creep genau 1x zur liste hinzugefügt wird, machen wir das unsubscriben von den Events gleichzeitig mit dem Entfernen aus der Liste!
            //Das subscriben passiert analog beim hinzufügen!
            targets.Remove(creep);
            creep.OnDeath -= RemoveCreepFromTargeting;
            creep.OnGoalReached -= RemoveCreepFromTargeting;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Creep>(out Creep creep))
        {
            RemoveCreepFromTargeting(creep);
        }
    }
}
