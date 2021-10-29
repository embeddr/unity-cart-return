// Basic object spawning behavior
//
// This is a temporary approach to generating randomly-placed instances of the
// provided prefab off the screen to serve as obstacles for the player. This will
// be superseded by a more structured procedural approach in the near future.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [Tooltip("Spawning enabled")]
    public bool spawnEnabled = true;

    [Tooltip("Object/Prefab to spawn")]
    [SerializeField]
    private GameObject _object;

    [Tooltip("Spawn interval, in seconds")]
    [SerializeField]
    private float _spawnInterval = 1.0F;

    [Tooltip("Delay before starting spawning, in seconds")]
    [SerializeField]
    private float _spawnDelay = 0.0F;

    [Tooltip("Minimum number of objects to spawn at a time")]
    [SerializeField]
    private int _spawnMin = 1;

    [Tooltip("Maximum number of objects to spawn at a time")]
    [SerializeField]
    private int _spawnMax = 3;

    private const float _spawnPointX = 20.0F;
    private float timer = 0.0F;
    private bool _delayExpired = false;

    private int _maxOverlapChecks = 5;

    void OnEnable()
    {
       CartObstacleCollision.OnCollision += PauseSpawn; 
    }

    void OnDisable()
    {
       CartObstacleCollision.OnCollision -= PauseSpawn; 
    }

    void PauseSpawn()
    {
        spawnEnabled = false;
    }

    void Spawn() {
        Debug.Log("Spawning " + _object.name + "(s)");

        // Determine number of instances to spawn
        int count = Random.Range(_spawnMin, _spawnMax + 1);

        // Determine y positions
        for (int obj = 0; obj < count; obj++) {
            // Attempt to find good spawn point
            bool goodPosition = false;
            for (int attempt = 0; attempt < _maxOverlapChecks; attempt++) {
                var spawnPointY = Random.Range(-7.0F, 7.0F);
                var collider = Physics2D.OverlapBox(new Vector2(_spawnPointX, spawnPointY),
                                                    _object.transform.localScale,
                                                    0,
                                                    LayerMask.GetMask("Obstacle"));
                if (!collider) {
                    Instantiate(_object,
                                new Vector2(_spawnPointX, spawnPointY),
                                _object.transform.rotation);
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (spawnEnabled) {
            timer += Time.deltaTime; 
            if (!_delayExpired) {
                // Hande initial spawning delay
                if (timer > _spawnDelay) {
                    timer = 0.0F;
                    _delayExpired = true;
                    Spawn();
                }
            } else {
                // Handle periodic spawning
                if (timer > _spawnInterval) {
                    timer = 0.0F;
                    Spawn();
                }
            }
        }
    }
}
