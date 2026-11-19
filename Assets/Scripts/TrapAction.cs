using System.Collections;
using UnityEngine;
using static UnityEditor.UIElements.ToolbarMenu;

public abstract class TrapAction : MonoBehaviour
{
    [Header("Activation")]
    [Tooltip("Extra delay on this object, added on top of the zone's own delay.")]
    [SerializeField] private float startDelay = 0f;

    [Tooltip("If true, the trap only fires once until ResetTrap() is called.")]
    [SerializeField] private bool triggerOnce = true;

    protected int variant = -1; // -1 = default behaviour

   

    private bool hasTriggered;
    private bool isRunning;

    public void Activate(int variantIndex)
    {
        if (isRunning) return;
        if (triggerOnce && hasTriggered) return;

        variant = variantIndex;
        hasTriggered = true;
        StartCoroutine(RunWithDelay());
    }
    public void Activate() => Activate(-1);
    /*public void Activate()
    {
        if (isRunning) return;
        if (triggerOnce && hasTriggered) return;

        hasTriggered = true;
        StartCoroutine(RunWithDelay());
    }
    */
    private IEnumerator RunWithDelay()
    {
        isRunning = true;
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);
        yield return Run();
        isRunning = false;
    }

    /// <summary>Call this on player death / level restart.</summary>
    public void ResetTrap()
    {
        StopAllCoroutines();
        isRunning = false;
        hasTriggered = false;
        OnReset();
    }

    protected abstract IEnumerator Run();
    protected virtual void OnReset() { }
}

