using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class FatScript : MonoBehaviour
{
    private int liquidCount = 0; // fat objesine carpan liquid sayisi
    public int maxLiquidCount = 15; // fat objesine carpabilecek maksimum liquid sayisi
    private SpriteRenderer spriteRenderer;
    public float shrinkDuration = 1f; // kuculme suresi
    private bool isShrinking = false; // kuculuyor mu

    public ShakeData FatExplosionShakeData;

    public GameObject FatDeathEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        liquidCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColor();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Liquid"))
        {
            Destroy(collision.gameObject); //liquid objesi temas ederse liquid objesini yok et

            liquidCount++; //carpan liquid obje sayisini arttir

            // fat objesi 20 liquid objesiyle temas ederse fat objesini yavasca kucultup yok et
            if (liquidCount >= maxLiquidCount && !isShrinking)
            {
                StartCoroutine(ShrinkAndDestroy());
            }
        }
    }

    //fat objesi liquid objesi temas ettikce rengi yavas yavas kirmiziya doner
    private void UpdateColor()
    {
        float colorValue = Mathf.Clamp01((float)liquidCount / maxLiquidCount);

        spriteRenderer.color = Color.Lerp(Color.white, Color.red, colorValue);
    }

    // kucult ve yok et
    private IEnumerator ShrinkAndDestroy()
    {
        isShrinking = true;

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        CameraShakerHandler.Shake(FatExplosionShakeData);

        if (FatDeathEffectPrefab != null)
        {
            Instantiate(FatDeathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // kuculme islemi bitince fat objesini yok et
    }
}
