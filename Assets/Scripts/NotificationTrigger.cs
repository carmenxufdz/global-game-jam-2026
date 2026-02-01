using UnityEngine;

public class NotificationTrigger : MonoBehaviour
{
    [TextArea]
    public string message;
    public float duration;

    AudioSource audioManager;
    [SerializeField] AudioClip roarClip;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("SoundM").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.PlayOneShot(roarClip);
            NotificationManager.Instance.Show(message, duration);
        }
    }
}
