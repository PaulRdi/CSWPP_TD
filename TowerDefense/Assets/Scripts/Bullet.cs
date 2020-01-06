using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 20.0f;
    [SerializeField] float _damage = 1.0f;
    [SerializeField] float _minDistForHit = 0.05f;

    Creep target;

    public void Init(Creep target)
    {
        this.target = target;
        StartCoroutine(FlyTowardsTarget());
    }

    IEnumerator FlyTowardsTarget()
    {
        while(target != null &&
              Vector3.Distance(transform.position, target.transform.position) > _minDistForHit)
        {
            Vector3 dir = (target.transform.position - this.transform.position).normalized;
            transform.Translate(dir * _speed * Time.deltaTime);
            yield return null;
        }

        if (target != null)
        {
            target.TakeDamage(_damage);
        }
        Destroy(this.gameObject);
    }
}
