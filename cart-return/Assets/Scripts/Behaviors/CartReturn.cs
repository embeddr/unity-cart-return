using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartReturn : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.ReturnZone.ToString()) {
            Debug.Log("Returning tag " + tag);
            
            if (tag == Tags.Player.ToString()) {
                Debug.Log("Player cart is now the front!");
                GetComponent<CartCartCollision>().frontCart = true;
            } else {
                Debug.Log("Returning cart!");
                Destroy(gameObject);
            }
        }
    }
}
