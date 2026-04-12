/*
 * Datei: NotificationWidget.cs
 * Zweck: Zeigt kurzzeitig eine Meldung im UI an.
 * Verantwortung:
 *   - Text anzeigen
 *   - automatisch ausblenden
 *
 * Abhängigkeiten:
 *   - TMPro
 */
using TMPro;
using UnityEngine;
using System.Collections;

public class NotificationWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float duration = 2f;

    public void Show(string message)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);

        if (messageText != null)
        {
            messageText.text = message;
        }

        StartCoroutine(HideAfterTime());
    }

    private IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}