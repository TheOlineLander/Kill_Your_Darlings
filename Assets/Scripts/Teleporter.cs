using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform teleporterEnd;
    [SerializeField] private float teleportDuration = 2f;

    private bool isTeleporting;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTeleporting) return;
        if (!other.CompareTag("Player")) return;

        StartCoroutine(TeleportRoutine(other));
    }

    private IEnumerator TeleportRoutine(Collider2D player)
    {
        isTeleporting = true;

        SpriteRenderer sr = player.GetComponentInChildren<SpriteRenderer>();
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // Dölj och frys
        if (sr != null) sr.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false; // stänger av fysik och collider tillfälligt
        }
        player.transform.position = teleporterEnd.position;

        yield return new WaitForSeconds(teleportDuration);

        // Teleportera, visa och släpp
        if (rb != null) rb.simulated = true;
        if (sr != null) sr.enabled = true;

        isTeleporting = false;
    }
}