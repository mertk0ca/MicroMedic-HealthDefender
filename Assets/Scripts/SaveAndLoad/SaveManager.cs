using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour // dosyalarý kaydedip yukler
{
    private string saveFilePath; //dosya konumu

    void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/saveData.json"; //json formatýnda kaydedilen datayý kalýcý sekilde saklamak icin
    }

    public void SavePlayerPosition(Vector3 position, float currentHealth, bool hasWeapon)
    {
        SaveData data = new SaveData
        {
            position = position,
            CurrentHealth = currentHealth,
            HasWeapon = hasWeapon
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Kaydedilen: " + json);
    }

    public (Vector3, float, bool) LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Yüklenen: " + json);
            return (data.position, data.CurrentHealth, data.HasWeapon);
        }
        else
        {
            Debug.LogWarning("Kayýt dosyasý bulunamadý");
            return (Vector3.zero, 100f, false); // kayitli dosya yoksa default durumu yukle
        }
    }

    [System.Serializable]
    private class SaveData //kaydedilecek degiskenler
    {
        public Vector3 position;
        public float CurrentHealth;
        public bool HasWeapon;
    }
}
