using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public LayerMask walls;
    public int maximumAmountOfEnemies = 10;
    public Vector2 intervalBetweenEnemies = new Vector2(2.2f,5.5f);
    public Enemy enemyPrefab;

    // Start is called before the first frame update
    void Start() {
        Invoke("AttemptSpawnEnemy", Random.Range(intervalBetweenEnemies.x, intervalBetweenEnemies.y));
    }

    void AttemptSpawnEnemy()
    {
        if (Enemy.allEnemies.Count < maximumAmountOfEnemies)
        {
            var originPosition = Player.instance.transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(originPosition, (new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f))).normalized, out RaycastHit hit, 500f, walls, QueryTriggerInteraction.Ignore))
            {
                var spawnPosition = Vector3.Lerp(hit.point, originPosition, 0.08f);
                var newEnemy = GameObject.Instantiate(enemyPrefab);
                spawnPosition.y = Player.instance.transform.position.y;
                newEnemy.transform.position = spawnPosition;
            }
        }
        Invoke("AttemptSpawnEnemy", Random.Range(intervalBetweenEnemies.x, intervalBetweenEnemies.y));
    }
}
