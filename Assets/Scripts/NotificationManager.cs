using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private float defaultDuration = 2f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        print("Notify");
        // Singleton clásico
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Opcional: mantener entre escenas
        DontDestroyOnLoad(gameObject);
        notificationText = GameObject.FindGameObjectWithTag("Text").GetComponent<TMP_Text>();
        notificationText.gameObject.SetActive(false);
    }

    public void Show(string message, float duration = -1f)
    {
        if (duration <= 0)
            duration = defaultDuration;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowRoutine(message, duration));
    }

    private IEnumerator ShowRoutine(string message, float duration)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        notificationText.gameObject.SetActive(false);
        currentCoroutine = null;
    }
}
