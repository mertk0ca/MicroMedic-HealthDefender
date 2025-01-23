using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class germMovement : MonoBehaviour
{
    [SerializeField] private float amplitude = 17f; // yukari asagi mesafe
    [SerializeField] private float frequency = 1f; // hareket hizi

    public bool vertical = true;

    private Vector3 startPosition; // baslangic pozisyonu

    void Start()
    {
        // baslangic pozisyonunu su anki yer olarak ayarla
        startPosition = transform.localPosition;
    }

    void Update()
    {
        HorizontalOrVertical();
    }
    
    void verticalMovement()
    {
        // sin dalgasi olusturur
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // objeyi sadece y ekseninde hareket ettir
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);

    }

    void horizontalMovement()
    {
        // sin dalgasi olusturur
        float newX = startPosition.x + Mathf.Sin(Time.time * frequency) * amplitude;

        // objeyi sadece y ekseninde hareket ettir
        transform.localPosition = new Vector3(newX, startPosition.y, startPosition.z);
    }

    void HorizontalOrVertical() //objenin yatay veya dikey hareket edecegine karar ver
    {
        if(vertical == true) 
        {
            verticalMovement();
        }
        else if (vertical == false)
        { 
            horizontalMovement();
        }
    }
}
