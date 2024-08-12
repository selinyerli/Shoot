using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform[] respawnPoints; // Respawn noktalarýný saklamak için dizi

    public Transform GetRandomRespawnPoint()
    {
        if (respawnPoints.Length == 0)
        {
            Debug.LogError("No respawn points set!");
            return null;
        }

        int randomIndex = Random.Range(0, respawnPoints.Length); // Rastgele bir indeks seç
        return respawnPoints[randomIndex]; // Rastgele respawn noktasý döndür
    }
}