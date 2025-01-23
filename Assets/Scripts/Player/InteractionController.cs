using UnityEngine;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    public GameObject papyrusCanvas;// papirus gorseli iceren canvas
    public Animator playerAnimator;// oyuncunun animatoru
    public AudioSource papyrusAudio;// papirus acilma sesi
    public GameObject interactionChild;// child obje

    public float papyrusDisplayTime = 5f;// papirusun acik kalma suresi

    private bool isPlayerNearby = false;// oyuncunun yakinlik durumu
    private bool isInteracting = false;// oyuncu etkilesimde mi
    private bool isPapyrusOpen = false;// papirus acik mi

    private void Start()
    {
        papyrusCanvas.SetActive(false);// baslangicta papirus gorunmez
        interactionChild.SetActive(false);// child obje baslangicta gizli
        if (papyrusAudio != null)
        {
            papyrusAudio.Stop();// ses baslangicta calmiyor
        }
    }

    private void Update()
    {
        // eger oyuncu yakinsa ve E tusuna basiyorsa
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isInteracting)
        {
            if (!isPapyrusOpen)
            {
                StartCoroutine(PlayAnimationAndShowPapyrus());
            }
            else
            {
                ClosePapyrus();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;// oyuncu yakinsa true yap
            interactionChild.SetActive(true);// child objeyi aktif et
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;// oyuncu uzaklasinca false yap
            interactionChild.SetActive(false);// child objeyi deaktif et
        }
    }

    private IEnumerator PlayAnimationAndShowPapyrus()
    {
        isInteracting = true;// etkilesim basladi
        playerAnimator.SetTrigger("SpecialAnimation");// animasyonu tetikle

        yield return new WaitForSeconds(0.3f);

        papyrusCanvas.SetActive(true);// papirus gorunur hale gelsin
        isPapyrusOpen = true;// papirus acik olarak isaretle
        Time.timeScale = 0;// oyunu durdur

        if (papyrusAudio != null)
        {
            papyrusAudio.Play();// papirus acilma sesi calsýn
        }

        isInteracting = false;// etkilesim bitti

        // belirlenen sure kadar bekle
        yield return new WaitForSecondsRealtime(papyrusDisplayTime);

        // eger oyuncu E tusuna basarak kapatmamissa otomatik olarak kapat
        if (isPapyrusOpen)
        {
            ClosePapyrus();
        }
    }

    private void ClosePapyrus()
    {
        papyrusCanvas.SetActive(false);// papirusu gizle
        isPapyrusOpen = false;// papirusun kapali oldugunu isaretle
        Time.timeScale = 1;// oyunu devam ettir

        if (papyrusAudio != null)
        {
            papyrusAudio.Stop();// papirus sesi durdurulsun
        }

        // animasyonu durdurmak veya sifirlamak icin animator parametrelerini sifirla
        playerAnimator.ResetTrigger("SpecialAnimation");// tetikleyiciyi sifirla
        playerAnimator.Play("Idle");// idle animasyona gec
    }
}
