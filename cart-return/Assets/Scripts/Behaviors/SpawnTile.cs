// Tile spawning behavior
//
// Spawns a randomly-selected tile at a configurable interval, then populates
// obstacles on the tile (also in a random fashion for now - need to add rules).

using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    [Tooltip("List of tile objects to spawn")]
    [SerializeField]
    private List<GameObject> _tileObjects;

    [Tooltip("List of obstacle objects to spawn")]
    [SerializeField]
    private List<GameObject> _obstacleObjects;

    [Tooltip("Spawn interval, in meters")]
    [SerializeField]
    private float _spawnInterval = 20.0F;

    [Tooltip("Spawning enabled")]
    private bool _spawnEnabled = true;

    [Tooltip("Spawn at start")]
    private bool _spawnAtStart = true;

    // X-axis offset for spawn point
    private const float _spawnPointXOffset = 30.0F;

    // Distance since last spawn
    private float _distanceSinceSpawn = 0.0F;

    void OnEnable()
    {
        // We need to stop spawning when a collision (game over) occurs.
        // There is no need to disable spawning when the game is paused,
        // the time scale is set to zero during a pause.
        CartObstacleCollision.OnCollision += DisableSpawn; 
    }

    void OnDisable()
    {
        CartObstacleCollision.OnCollision -= DisableSpawn; 
    }
    void DisableSpawn()
    {
        _spawnEnabled = false;
    }

    private void Start()
    {
        // Ensure that objects list is not empty
        Utils.Assert(_tileObjects.Count != 0, "No objects provided to SpawnTile behavior!");

        if (_spawnAtStart)
        {
            SpawnRandomTile(_spawnPointXOffset);
        }
    }

    void Update()
    {
        if (_spawnEnabled)
        {
            // Integrate distance since last spawn
            _distanceSinceSpawn += (Time.deltaTime * GameData.ScrollSpeed);
            if (_distanceSinceSpawn > _spawnInterval)
            {
                // Spawn at exact offset, factoring in overshoot
                var overshoot = _distanceSinceSpawn - _spawnInterval;
                SpawnRandomTile(_spawnPointXOffset - overshoot);
                _distanceSinceSpawn = overshoot;
            }
        }
    }

    void SpawnRandomTile(float spawnPointX)
    {
        var index = Random.Range(0, _tileObjects.Count);
        var spawnObject = _tileObjects[index];
        var tile = Instantiate(spawnObject,
                               new Vector2(spawnPointX, 0.0F),
                               spawnObject.transform.rotation);

        SpawnObstaclesOnTile(tile);
    }

    void SpawnObstaclesOnTile(GameObject tileObject)
    {
        // Get all spawn-point components from children, append to list
        var obstacleSpawnPoints = new List<ParkedCarSpawnPoint>();
        foreach (Transform child in tileObject.transform) {
            // Note: This is only traversing the first level of children
            var tmp = child.GetComponent<ParkedCarSpawnPoint>();
            if (tmp != null)
            {
                obstacleSpawnPoints.Add(tmp);
            }
        }

        // Spawn random number of obstacles
        var numObstacles = Random.Range(2, 5); // FIXME: magic numbers
        Utils.Assert(numObstacles <= obstacleSpawnPoints.Count,
            "Not enough obstacle spawn points!");

        for (int i = 0; i < numObstacles; i++)
        {
            // Repeat until open space is found
            var lookingForOpenSpace = true;
            while (lookingForOpenSpace)
            {
                // Select random spawn point
                var index = Random.Range(0, obstacleSpawnPoints.Count);
                var candidateSpawnPoint = obstacleSpawnPoints[index];

                // Spawn only if open space
                if (candidateSpawnPoint.openSpace)
                {
                    candidateSpawnPoint.openSpace = false;
                    lookingForOpenSpace = false;

                    SpawnRandomObstacle(candidateSpawnPoint.transform.position);
                }
            }
        }
    }

    void SpawnRandomObstacle(Vector2 spawnPoint)
    {
        // Randomly select obstacle
        var index = Random.Range(0, _obstacleObjects.Count);
        var obstacle = _obstacleObjects[index];

        // Randomly select rotation (0 or 180 degrees)
        var rotation = obstacle.transform.rotation;
        int flip = Random.Range(0, 2);
        if (flip == 1)
        {
            rotation *= Quaternion.Euler(new Vector3(0, 0, 180));
        }
        Instantiate(obstacle, spawnPoint, rotation);
    }

}
