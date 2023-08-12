using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the "Walls" tag
        if (collision.CompareTag("Walls"))
        {
            // Call the ThrowRock() method from the GameManager instance
            GameManager.Instance.ThrowRock();

            // Destroy the current rock object
            Destroy(this.gameObject);
        }
    }
}
