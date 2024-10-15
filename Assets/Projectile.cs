using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask projectileHitsType;
    public float speed = 10.0f;
    public int damageAmount = 30;
    public float lifeTime = 20.0f;
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        var FoundActors = Physics.OverlapSphere(transform.position, 0.2f, projectileHitsType.value, QueryTriggerInteraction.Ignore);
        if (FoundActors.Length > 0) {
            if (FoundActors[0].TryGetComponent<Actor>(out Actor foundActor)) {
                foundActor.healthSystem.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
