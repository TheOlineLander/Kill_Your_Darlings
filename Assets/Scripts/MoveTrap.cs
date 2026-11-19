using System.Collections;
using UnityEngine;


public class MoveTrap : TrapAction
{
    [Header("Destination (use ONE)")]
    [Tooltip("Optional: move to this Transform's position. Overrides the offset.")]
    [SerializeField] private Transform destination;
    [Tooltip("Used when no destination Transform is set. Relative to the start position.")]
    [SerializeField] private Vector2 offset = new Vector2(0f, 3f);

    [Header("Alternative destinations (optional)")]
    [SerializeField] private Transform[] alternativeDestinations;

    [Header("Motion")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Return")]
    [SerializeField] private bool returnAfter = false;
    [SerializeField] private float holdTime = 1f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private readonly WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();

    private Vector2 Target => destination != null ? (Vector2)destination.position : startPosition + offset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    protected override IEnumerator Run()
    {
        Vector2 from = transform.position;
        Vector2 to = GetTarget();

        yield return MoveBetween(from, to);

        if (returnAfter)
        {
            if (holdTime > 0f) yield return new WaitForSeconds(holdTime);
            yield return MoveBetween(to, from);
        }
    }

    private Vector2 GetTarget()
    {
        if (variant >= 0
            && alternativeDestinations != null
            && variant < alternativeDestinations.Length
            && alternativeDestinations[variant] != null)
        {
            return alternativeDestinations[variant].position;
        }

        return Target; // your original destination / offset logic
    }

    private IEnumerator MoveBetween(Vector2 a, Vector2 b)
    {
        float t = 0f;
        float safeDuration = Mathf.Max(duration, 0.0001f);

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / safeDuration;
            SetPosition(Vector2.LerpUnclamped(a, b, ease.Evaluate(Mathf.Clamp01(t))));
            yield return waitFixed;
        }

        SetPosition(b);
    }

    private void SetPosition(Vector2 p)
    {
        if (rb != null) rb.MovePosition(p);
        else transform.position = p;
    }

    protected override void OnReset()
    {
        SetPosition(startPosition);
        if (rb != null) rb.position = startPosition;
        transform.position = startPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 start = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Vector2 end = destination != null ? (Vector2)destination.position : start + offset;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(end, 0.25f);
    }
}