using System.Collections;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f; // mermi hizi
    public float bulletLifeTime = 2f; // mermi yasam suresi
    public GameObject GermDeathEffectPrefab; 
    public GameObject PlatformDeathEffectPrefab;
    public GameObject BossDeathEffectPrefab;

    public ShakeData EnemyDamageShakeData;
    public ShakeData BossDeathShakeData;

    private void Start()
    {
        Destroy(gameObject, bulletLifeTime);//iki saniye sonra mermiyi yok et
    }

    private void Update()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);//mermi hareketi
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("papirus") && !collision.CompareTag("BossArea") && !collision.CompareTag("Boss"))
        {
            Destroy(gameObject); // mermi bu taglar disinda bir nesneye carptiginda yok olur
        }

        if (collision.CompareTag("Enemy")) // dusman tagina carparsa
        {
            Destroy(collision.gameObject);  //carptigi objeyi yok et
            CameraShakerHandler.Shake(EnemyDamageShakeData);

            if (GermDeathEffectPrefab != null)
            {
                Instantiate(GermDeathEffectPrefab, collision.transform.position, Quaternion.identity);
            }

            Destroy(gameObject);//mermiyi yok et
        }

        if (collision.CompareTag("Boss")) // boss tagina carparsa
        {
            BossHealth bossHealth = collision.GetComponent<BossHealth>();

            if (bossHealth != null)
            {
                // boss'a hasar ver
                bossHealth.TakeDamage(20);
                CameraShakerHandler.Shake(EnemyDamageShakeData);
                Debug.Log(bossHealth.GetCurrentHealth());
            }

            if (bossHealth.currentHealth <= 0)
            {
                if (BossDeathEffectPrefab != null)
                {
                    Instantiate(BossDeathEffectPrefab, collision.transform.position, Quaternion.identity);
                    CameraShakerHandler.Shake(BossDeathShakeData);
                }
            }

            Destroy(gameObject);// mermiyi yok et
        }
        else if (collision.CompareTag("Breakable")) // breakable tagina carparsa
        {
            Destroy(collision.gameObject); // objeyi yok et
            CameraShakerHandler.Shake(EnemyDamageShakeData);

            if (PlatformDeathEffectPrefab != null)
            {
                Instantiate(PlatformDeathEffectPrefab, collision.transform.position, Quaternion.identity);
            }

            Destroy(gameObject); //mermiyi yok et
        }
    }
}