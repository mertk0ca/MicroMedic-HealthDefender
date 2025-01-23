using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    // Duraklatma menüsünü tutacak deðiþken
    public GameObject duraklatMenu;

    // Butona týklandýðýnda çaðrýlacak fonksiyon
    public void PauseGame()
    {
        Debug.Log("Duraklatma Butonuna Týklandý"); // Duraklatma butonuna týklanýp týklanmadýðýný görmek için.

        // Duraklatma menüsünü aktif hale getiriyoruz
        duraklatMenu.SetActive(true);

        // Oyunu duraklatýyoruz
        Time.timeScale = 0f;
    }


    // Oyunu devam ettirme fonksiyonu
    public void ResumeGame()
    {
        // Duraklatma menüsünü devre dýþý býrakýyoruz
        duraklatMenu.SetActive(false);

        // Oyunu devam ettiriyoruz (timeScale 1 olduðunda oyun devam eder)
        Time.timeScale = 1f;
    }
}
