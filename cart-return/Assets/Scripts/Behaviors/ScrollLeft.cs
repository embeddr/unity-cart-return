// Scroll left behavior
//
// Simple logic to move the associated object to the left through the world. This creates
// the illusion of the player moving to the right.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ScrollLeft : MonoBehaviour
{
    [Tooltip("Sets whether or not scrolling is enabled")]
    public bool scrollEnabled = true;

    [Tooltip("Despawn at this X coordinate")]
    [SerializeField]
    private float _despawnX = -25.0F;

    private Rigidbody2D _rb2d;

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
       CartObstacleCollision.OnCollision += PauseScroll; 
    }

    void OnDisable()
    {
       CartObstacleCollision.OnCollision -= PauseScroll; 
    }

    void PauseScroll()
    {
        scrollEnabled = false;
    }

    void FixedUpdate()
    {
        if (scrollEnabled) {
            // Update rigidbody position to move at specified velocity
            var new_pos = transform.position;
            new_pos.x += -1.0F * GameData.ScrollSpeed * Time.fixedDeltaTime;
            new_pos.y += _rb2d.velocity.y * Time.fixedDeltaTime;
            _rb2d.MovePosition(new_pos);

            // Despawn when sufficiently off-screen
            if (gameObject.transform.position.x < _despawnX) {
                Destroy(gameObject);
            }
        }
    }
}
