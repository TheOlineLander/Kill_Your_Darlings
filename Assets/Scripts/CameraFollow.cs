using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private float lookOffsetX = 2f;

    private float facing = 1f; // +1 = höger, -1 = vänster
    private float lastTargetX;

    private void Start()
    {
        lastTargetX = target.position.x;
    }

    private void LateUpdate()
    {
        // Uppdatera riktning utifrån hur spelaren rört sig sedan förra framen
        float dx = target.position.x - lastTargetX;
        if (Mathf.Abs(dx) > 0.0001f)
            facing = Mathf.Sign(dx);
        lastTargetX = target.position.x;

        Vector3 lookOffset = new Vector3(facing * lookOffsetX, 0f, 0f);
        Vector3 targetPosition = target.position + offset + lookOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}