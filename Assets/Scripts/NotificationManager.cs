using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField] private float defaultDuration = 2f;

    private TMP_Text notificationText;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        // Singleton clásico
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Suscribirse a cambios de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Buscar el TMP_Text en la nueva escena
        GameObject textGO = GameObject.FindGameObjectWithTag("Text");
        if (textGO != null)
        {
            notificationText = textGO.GetComponent<TMP_Text>();
            notificationText.gameObject.SetActive(false);
        }
        else
        {
            notificationText = null;
        }
    }

    public void Show(string message, float duration = -1.0f)
    {
        if (notificationText == null)
        {
            Debug.LogWarning("NotificationManager: No hay TextMeshProUGUI en la escena actual.");
            return;
        }

        if (duration < 0) duration = defaultDuration;

        // Si ya hay una notificación mostrando, detenerla
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowRoutine(message, duration));
    }

    private IEnumerator ShowRoutine(string message, float duration)
    {
        if (notificationText == null) yield break;

        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (notificationText != null)
            notificationText.gameObject.SetActive(false);

        currentCoroutine = null;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
