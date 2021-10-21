// Parked car spawning behavior
//
// This is a temporary approach to generating randomly-placed instances of the
// provided prefab off the screen to serve as obstacles for the player. This will
// be superseded by a more structured procedural approach in the near future.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParkedCar : MonoBehaviour
{
    [Tooltip("Spawning enabled")]
    public bool spawnEnabled = true;

    [Tooltip("Parked car prefab to spawn")]
    [SerializeField]
    private GameObject _parkedCarPrefab;

    [Tooltip("Spawn interval in seconds")]
    [SerializeField]
    private float _spawnInterval = 1.0F;

    [Tooltip("Minimum number of cars to spawn at a time")]
    [SerializeField]
    private int _spawnMin = 1;

    [Tooltip("Maximum number of cars to spawn at a time")]
    [SerializeField]
    private int _spawnMax = 3;

    private const float _spawnPointX = 20.0F;
    private float _timeSinceSpawn = 0.0F;

    void OnEnable()
    {
       PlayerObstacleCollision.OnCollision += PauseSpawn; 
    }

    void OnDisable()
    {
       PlayerObstacleCollision.OnCollision -= PauseSpawn; 
    }

    void PauseSpawn()
    {
        spawnEnabled = false;
    }

    void Update()
    {
        _timeSinceSpawn += Time.deltaTime; 
        if (spawnEnabled && (_timeSinceSpawn > _spawnInterval)) {
            Debug.Log("Spawning parked car(s)");

            _timeSinceSpawn = 0.0F;

            // Determine number of cars to spawn
            int count = Random.Range(_spawnMin, _spawnMax + 1);

            // Determine y positions
            for (int car = 0; car < count; car++) {
                var spawnPointY = Random.Range(-8.0F, +8.0F);
                Instantiate(_parkedCarPrefab,
                            new Vector2(_spawnPointX, spawnPointY),
                            _parkedCarPrefab.transform.rotation);
            }
        }
    }
}
