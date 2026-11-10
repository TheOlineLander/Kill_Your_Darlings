using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform TeleporterEnd;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = TeleporterEnd.position;
            other.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
