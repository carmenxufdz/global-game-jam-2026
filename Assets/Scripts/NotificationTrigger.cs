using UnityEngine;

public class NotificationTrigger : MonoBehaviour
{
    [TextArea]
    public string message;

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("hila");
        if (other.CompareTag("Player"))
        {
            NotificationManager.Instance.Show(message);
        }
    }
}
