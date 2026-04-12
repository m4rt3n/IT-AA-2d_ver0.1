/*
 * Datei: TimerWidget.cs
 * Zweck: Zeigt eine laufende Zeit oder Countdown an.
 * Verantwortung:
 *   - Aktualisiert Zeitanzeige
 *
 * Abhängigkeiten:
 *   - TMPro
 */
using TMPro;
using UnityEngine;

public class TimerWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    private float time;

    public void SetTime(float newTime)
    {
        time = newTime;
        UpdateText();
    }

    public void AddTime(float delta)
    {
        time += delta;
        UpdateText();
    }

    private void UpdateText()
    {
        if (timeText == null) return;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}