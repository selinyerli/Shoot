using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform[] respawnPoints; // Respawn noktalar�n� saklamak i�in dizi

    public Transform GetRandomRespawnPoint()
    {
        if (respawnPoints.Length == 0)
        {
            Debug.LogError("No respawn points set!");
            return null;
        }

        int randomIndex = Random.Range(0, respawnPoints.Length); // Rastgele bir indeks se�
        return respawnPoints[randomIndex]; // Rastgele respawn noktas� d�nd�r
    }
}