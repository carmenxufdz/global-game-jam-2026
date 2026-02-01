using UnityEngine;

public class NotificationTrigger : MonoBehaviour
{
    [TextArea]
    public string message;
    public float duration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationManager.Instance.Show(message, duration);
        }
    }
}
