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

    [Tooltip("Spawn interval in seconds")]
    [SerializeField]
    private float _spawnInterval = 1.0F;

    [Tooltip("Minimum number of objects to spawn at a time")]
    [SerializeField]
    private int _spawnMin = 1;

    [Tooltip("Maximum number of objects to spawn at a time")]
    [SerializeField]
    private int _spawnMax = 3;

    private const float _spawnPointX = 20.0F;
    private float _timeSinceSpawn = 0.0F;

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

    void Update()
    {
        _timeSinceSpawn += Time.deltaTime; 
        if (spawnEnabled && (_timeSinceSpawn > _spawnInterval)) {
            Debug.Log("Spawning object(s)");

            _timeSinceSpawn = 0.0F;

            // Determine number of instances to spawn
            int count = Random.Range(_spawnMin, _spawnMax + 1);

            // Determine y positions
            for (int obj = 0; obj < count; obj++) {
                var spawnPointY = Random.Range(-8.0F, +8.0F);
                Instantiate(_object,
                            new Vector2(_spawnPointX, spawnPointY),
                            _object.transform.rotation);
            }
        }
    }
}
