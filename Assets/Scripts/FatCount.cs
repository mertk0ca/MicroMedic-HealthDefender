using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro namespace'ini ekledik

public class FatCount : MonoBehaviour
{
    public TextMeshProUGUI fatCountText; // UI'da Fat objeleri sayýsýný gösterecek TextMeshProUGUI component
    public int fatCount = 0; // Fat objelerinin sayýsýný tutan deðiþken
    public GameObject objectToDestroy; // Yok edilecek obje referansý

    void Start()
    {
        UpdateFatCount(); // Baþlangýçta sayýyý güncelle
    }

    void Update()
    {
        UpdateFatCount(); // Her frame'de Fat objesi sayýsýný güncelle
    }

    // Fat objelerinin sayýsýný güncelleyen fonksiyon
    private void UpdateFatCount()
    {
        // "Fat" tag'ýna sahip tüm aktif objeleri bul
        GameObject[] fatObjects = GameObject.FindGameObjectsWithTag("Fat");

        // Fat objelerinin sayýsýný al
        fatCount = fatObjects.Length;

        // UI'da sayýyý göster (eðer UI TextMeshProUGUI objesi atanmýþsa)
        if (fatCountText != null)
        {
            fatCountText.text = fatCount.ToString();
        }

        // Eðer Fat objesi kalmadýysa belirli bir objeyi yok et
        if (fatCount == 0 && objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
    }
}
