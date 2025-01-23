using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public GameObject meatballPrefab;
    public GameObject dropPrefab;
    public float spawnInterval = 10f;  // meatball firlatma araligi
    public float dropInterval = 5f;    // damlatma araligi
    public float lifetime = 5f;        // meatball yasam suresi
    public float throwStrength = 10f;  // meatball firlatma gucu
    public float spawnOffsetX = 1f;   // yatay firlatma offseti
    public float spawnOffsetY = 1f;   // dikey firlatma offseti

    private void Start()
    {
        InvokeRepeating("SpawnMeatballs", 0f, spawnInterval);  // 10 saniyede bir meatball firlat
        InvokeRepeating("SpawnDrop", 0f, dropInterval); // bes saniyede bir damla damlat
    }

    private void SpawnMeatballs()//meatball olustur
    {
        Vector3 spawnPositionLeft = new Vector3(transform.position.x - spawnOffsetX, transform.position.y + spawnOffsetY, transform.position.z); //sola meatball fýrlatmak icin gereken offset
        Vector3 spawnPositionRight = new Vector3(transform.position.x + spawnOffsetX, transform.position.y + spawnOffsetY, transform.position.z);  //saga meatball firlatmak icin gereken offset

        GameObject meatballLeft = Instantiate(meatballPrefab, spawnPositionLeft, Quaternion.identity); //solda olusut
        GameObject meatballRight = Instantiate(meatballPrefab, spawnPositionRight, Quaternion.identity);  //sagda olustur

        meatballLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(-throwStrength, throwStrength);  // sol yukariya dogru firlat
        meatballRight.GetComponent<Rigidbody2D>().velocity = new Vector2(throwStrength, throwStrength);  // sag yukariya dogru firlat

        StartCoroutine(ShrinkAndDestroy(meatballLeft));
        StartCoroutine(ShrinkAndDestroy(meatballRight));
    }

    private IEnumerator ShrinkAndDestroy(GameObject meatball)
    {
        float elapsedTime = 0f;
        Vector3 originalScale = meatball.transform.localScale;

        while (elapsedTime < lifetime)
        {
            if (meatball == null)
            {
                // obje yok edilmisse islemden cýk
                yield break;
            }

            if (elapsedTime >= 4f)
            {
                // dorduncu saniyeden sonra kuculme islemini baslat
                meatball.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, (elapsedTime - 4f) / 1f);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // kuculme islemi bittikten sonra objeyi yok et
        Destroy(meatball);
    }

    private void SpawnDrop()
    {
        // boss un biraz altýndaki pozisyonu alma
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - spawnOffsetY, transform.position.z);

        // damla olusturma
        GameObject drop = Instantiate(dropPrefab, spawnPosition, Quaternion.identity);

        // bir saniye sonra damlayý yok et
        StartCoroutine(DestroyDropAfterTime(drop, 1f));
    }

    private IEnumerator DestroyDropAfterTime(GameObject drop, float delay)
    {
        // bir saniye bekle
        yield return new WaitForSeconds(delay);

        if (drop == null)
        {
            // obje yok edilmisse islemden cýk
            yield break;
        }

        // damlayý yok et
        Destroy(drop);
    }
}