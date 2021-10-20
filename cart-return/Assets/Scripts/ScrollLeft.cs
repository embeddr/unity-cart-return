using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollLeft : MonoBehaviour
{
    // Sets whether object should currently be scrolling
    public bool _scrolling = true;

    // TODO: Where should this actually be defined?
    private const float ScrollVelocity = -10.0F;

    void FixedUpdate()
    {
        if (_scrolling) {
            // Update rigidbody position to move at specified velocity
            var new_pos = gameObject.transform.position;
            new_pos.x += ScrollVelocity * Time.fixedDeltaTime;
            GetComponent<Rigidbody2D>().MovePosition(new_pos);

            // Despawn when sufficiently off-screen
            if (gameObject.transform.position.x < -20.0F) {
                Destroy(gameObject);
            }
        }
    }
}
