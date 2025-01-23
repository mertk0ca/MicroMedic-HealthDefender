using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public float lifetime = 5f; // Sývýnýn ne kadar süre sonra yok olacaðýný belirler

    void Start()
    {
        // Sývýyý belirli bir süre sonra yok et
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player'a çarptýðýnda sývýyý yok et
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (!collision.collider.CompareTag("Liquid") && collision.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
