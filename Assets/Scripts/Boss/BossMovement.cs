using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // yatay hareket hizi
    public float moveDistance = 5f; // yatay hareket mesafesi
    public float minDescentTime = 20f; // asagi inmesi icin beklemesi gereken minimum sure
    public float maxDescentTime = 30f; // asagi inmesi icin gecmesi gereken maksimum sure
    public float stayTime = 5f; // asagida kalma suresi
    public float delayBeforeDescent = 2f; // asagi inmeden once sabit bekleme suresi

    // Titreme parametreleri
    public float trembleAmount = 10f; // sallanma miktari
    public float trembleSpeed = 5f; // sallanma hizi

    private Vector2 initialPosition;
    private Vector2 descentStartPosition; // asagi inmeye basladigi pozisyon
    private bool isDescending = false;  //asagi iniyor mu
    private bool isAscending = false;  //yukari cikiyor mu
    private bool isMovingHorizontally = true;  //yatay harelet ediyor mu
    private bool isTrembling = false; // sallaniyor mu
    private float horizontalMoveTimer = 0f;  //yatay hareket ettigi sure
    private float descentTimer = 0f;  //asagi inme suresi
    private float ascentTimer = 0f;  //yukari cikma suresi
    private float descentTargetY; // dikey asagý inme hedefi
    private float ascentTargetY;  //dikey yukari cikma hedefi
    private float horizontalTargetX;  //yatay hedef

    private float nextDescentTime; // sanraki asagi inme suresi
    private float descentDelayTimer = 0f; //  asagi inme bekleme zamani

    private void Start()
    {
        initialPosition = transform.position;  //baslangic pozisyonunu al
        descentTargetY = initialPosition.y - 7f; // asagi inme hedefini platforma denk gelecek sekilde 7f asagi indirme
        ascentTargetY = initialPosition.y; // yukari cýkma hedefini ilk pozisyona atama
        horizontalTargetX = initialPosition.x; // yatay harekete basladigi noktayi baslangic pozisyonuna atama

        nextDescentTime = Random.Range(minDescentTime, maxDescentTime);//20 ile 30 saniye arasinda rastgele bir degerde asagi iner
    }

    private void Update()
    {
        if (isMovingHorizontally)
        {
            HorizontalMovement();
        }

        if (isDescending)
        {
            DescentMovement();
        }

        if (isAscending)
        {
            AscentMovement();
        }
    }

    private void HorizontalMovement() //yatay pingpong hareketini yumusak bir sekilde yapma
    {
        horizontalMoveTimer += Time.deltaTime;
        float movement = Mathf.PingPong(horizontalMoveTimer * moveSpeed, moveDistance);
        transform.position = new Vector2(Mathf.Lerp(transform.position.x, initialPosition.x + movement, Time.deltaTime * moveSpeed), transform.position.y);


        if (horizontalMoveTimer >= nextDescentTime)
        {
            isMovingHorizontally = false; // yatay hareketi durdur
            horizontalMoveTimer = 0f;
            descentDelayTimer = 0f; // bekleme zamanlayicisini sifirla
            isDescending = true; // asagi inme hareketine gec
            descentStartPosition = transform.position; //asagi inmeye basladigi pozisyonu al


            nextDescentTime = Random.Range(minDescentTime, maxDescentTime);//20 ile 30 saniye arasinda rastgele bir degerde asagi iner

            isTrembling = true;  // sallanma hareketini baslat
        }
    }

    private void DescentMovement()  //asagi inme hareketi
    {
        descentDelayTimer += Time.deltaTime;

        if (descentDelayTimer < delayBeforeDescent)//bir asniye bekle
        {
            if (isTrembling)//sallanma basladiysa asagi inme pozisyonunu al
            {
                float trembleRotation = Mathf.Sin(Time.time * trembleSpeed) * trembleAmount;
                transform.rotation = Quaternion.Euler(0, 0, trembleRotation);
            }
            return; // Eðer bekleme süresi bitmediyse, hareket etme
        }

        descentTimer += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, descentTargetY, Time.deltaTime * moveSpeed));//asagi inme islemini yumusak bir sekilde yap

        if (Mathf.Abs(transform.position.y - descentTargetY) < 0.1f) // tolerans degeriyle karsilastirma
        {
            descentTimer = 0f;
            isDescending = false;
            StartCoroutine(StayAtBottom());
        }
    }

    private System.Collections.IEnumerator StayAtBottom()
    {
        yield return new WaitForSeconds(stayTime); // bes saniye bekle
        isAscending = true;
        ascentTargetY = descentStartPosition.y; // alcalmaya basladigi pozisyona geri don
        isTrembling = false;
        transform.rotation = Quaternion.identity; // harekete bastan basla
    }

    private void AscentMovement()
    {
        ascentTimer += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, ascentTargetY, Time.deltaTime * moveSpeed)); //yukari cikma islemini yumusak bir sekilde yap

        if (Mathf.Abs(transform.position.y - ascentTargetY) < 0.1f) //tolerans degeriyle karsilastir
        {
            ascentTimer = 0f;
            isAscending = false;
            isMovingHorizontally = true;
        }
    }
}
