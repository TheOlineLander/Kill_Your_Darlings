using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    [SerializeField] private float timeToRespawn = 0.1f;

    private bool isRespawning;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (isRespawning) return;

        StartCoroutine(RespawnAfterDelay(other));
    }

    private IEnumerator RespawnAfterDelay(Collider2D player)
    {
        isRespawning = true;

        // Optional: freeze the player during the delay
        if (player.attachedRigidbody != null)
        {
            player.attachedRigidbody.linearVelocity = Vector2.zero;
            player.attachedRigidbody.simulated = false;
        }

        yield return new WaitForSeconds(timeToRespawn);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}