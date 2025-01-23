using UnityEngine;

public class Player : MonoBehaviour  //yuklenen dosyalarý oyuna ekler
{
    private SaveManager saveManager;
    private playerInfo playerInfoScript;

    public Vector3 initialPosition;

    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        playerInfoScript = FindObjectOfType<playerInfo>();
        PlayerShooting playerShooting = GetComponent<PlayerShooting>();

        if (saveManager != null)
        {
            // saveManager scriptindeki kaydedilen degerleri al
            (Vector3 loadedPosition, float loadedHealth, bool loadedHasWeapon) = saveManager.LoadPlayerData();

            // kayitli bir pozisyon varsa
            if (loadedPosition != Vector3.zero)
            {
                transform.position = loadedPosition;
                playerInfoScript.currentHealth = loadedHealth; // saglýk verisini yukle
                if (playerShooting != null)
                {
                    playerShooting.hasWeapon = loadedHasWeapon; // silah verisini yukle
                }
            }
            else
            {
                // kayitli bir pozisyon yoksa default durumu yukle
                transform.position = initialPosition;
                playerInfoScript.currentHealth = playerInfoScript.maxHealth; // default saglýk degeri
                if (playerShooting != null)
                {
                    playerShooting.hasWeapon = false; // default silah durumu
                }
            }
        }
        else
        {
            // saveManager yoksa defaul durumlarý yukle
            transform.position = initialPosition;
            playerInfoScript.currentHealth = playerInfoScript.maxHealth;
            if (playerShooting != null)
            {
                playerShooting.hasWeapon = false;
            }
        }
    }

    public void SavePositionAndReturnToMainMenu() //ana menu butonuna týklandýgýnda oyunun kaydedilmesi fonksiyonu
    {
        PlayerShooting playerShooting = GetComponent<PlayerShooting>();

        if (saveManager != null && playerShooting != null)
        {
            saveManager.SavePlayerPosition(transform.position, playerInfoScript.currentHealth, playerShooting.hasWeapon);
        }

        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");  // ana menu sahnesini yukle
    }
}
