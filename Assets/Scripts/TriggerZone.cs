using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TriggerZone : MonoBehaviour
{
    public enum Mode { OnEnter, OnExit }

    [Header("Detection")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private Mode mode = Mode.OnEnter;
    [SerializeField] private bool triggerOnce = true;

    [Header("Timing")]
    [Tooltip("Delay before this zone fires its traps.")]
    [SerializeField] private float delay = 0f;

    [Header("What to fire")]
    [SerializeField] private TrapAction[] traps;

    [Tooltip("-1 = the trap's default destination. 0, 1, 2... = pick from the trap's Alternative Destinations.")]
    [SerializeField] private int destinationIndex = -1;

    [Tooltip("Optional extras: sounds, camera shake, particles, etc.")]
    [SerializeField] private UnityEvent onTriggered;

    private bool hasFired;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mode == Mode.OnEnter) TryFire(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (mode == Mode.OnExit) TryFire(other);
    }

    private void TryFire(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;
        if (triggerOnce && hasFired) return;

        hasFired = true;
        StartCoroutine(FireAfterDelay());
    }

    private IEnumerator FireAfterDelay()
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        foreach (TrapAction trap in traps)
        {
            if (trap != null) trap.Activate(destinationIndex);
        }

        onTriggered?.Invoke();
    }

    /// <summary>Call on player death / level restart.</summary>
    public void ResetZone()
    {
        StopAllCoroutines();
        hasFired = false;
    }

    private void OnDrawGizmos()
    {
        if (traps == null) return;

        Gizmos.color = Color.yellow;
        foreach (TrapAction trap in traps)
        {
            if (trap != null) Gizmos.DrawLine(transform.position, trap.transform.position);
        }
    }
}