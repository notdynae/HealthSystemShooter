using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {
    public static List<Enemy> allEnemies = new List<Enemy>();

    public List<GameObject> itemDrops = new List<GameObject>();
    float shootInterval = 1.5f;

    private void OnEnable() {
        allEnemies.Add(this);
    }

    public override void Start()
    {
        base.Start();
        healthSystem.lives = 0;
    }

    private void OnDisable() {
        allEnemies.Remove(this);
    }

    public override Vector3 GetMovementDirection()
    {
        Vector3 directionToPlayer = ((Player.instance.transform.position - transform.position));
        directionToPlayer.y = 0.0f;
        return directionToPlayer.normalized;
    }

    public override bool WantsToShoot()
    {
        if (healthSystem.health <= 0)
            return false;
        shootInterval -= Time.deltaTime;
        if (shootInterval < 0.0f) {
            shootInterval = UnityEngine.Random.Range(0.7f, 1.7f);
            return true;
        }
        return base.WantsToShoot();
    }

    public override void Die()
    {
        base.Die();
        if (Random.value < 0.45f && itemDrops.Count > 0)
        {
            var newCreated = GameObject.Instantiate(itemDrops[Random.Range(0, itemDrops.Count)]);
            newCreated.transform.position = transform.position + Vector3.up * 0.45f;
        }
        Destroy(gameObject);
    }
}
