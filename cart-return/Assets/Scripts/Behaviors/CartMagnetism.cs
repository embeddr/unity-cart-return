// Cart magnetism behavior
//
// Implements a simple magnetism behavior that attracts any free carts within the specified
// radius toward this objects y coordinate. Does not impact horizontal position or model any
// sort of realistic magnetic effect.

using UnityEngine;

public class CartMagnetism : MonoBehaviour
{
    public bool magnetismEnabled = true;

    private CartStacking _cartStacking;

    [Tooltip("Radius of magnetic effect")]
    private float _radius = 3.0F;

    private Collider2D[] _colliders = new Collider2D[10];


    void Awake()
    {
        _cartStacking = GetComponent<CartStacking>();
    }

    void FixedUpdate()
    {
        bool frontCart = (!_cartStacking || _cartStacking.isFrontCart);
        if (magnetismEnabled && frontCart) {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Obstacle"));

            Vector2 pos = new Vector2(transform.position.x + _radius,
                                      transform.position.y);
            int numColliders = Physics2D.OverlapCircle(pos,
                                                       _radius,
                                                       filter,
                                                       _colliders);
            for (int i = 0; i < numColliders; i++) {
                if (_colliders[i].tag == "FreeCart") {
                    // TODO: cache gameobjects/rigidbodies rather than fetching every tick?
                    // TODO: check for clear LOS from object to target y coord?
                    Rigidbody2D rb2d = _colliders[i].gameObject.GetComponent<Rigidbody2D>();
                    // TODO: This is a great place for a PID!
                    float error = transform.position.y - _colliders[i].transform.position.y;
                    float force = error * 50.0F; // TODO: tune coefficient
                    // TODO: share single Vector2?
                    Debug.Log("Magnetism force: " + force);
                    rb2d.AddForce(Vector2.up * force);
                }
            }
        }
    }
}
