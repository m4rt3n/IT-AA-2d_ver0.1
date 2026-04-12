/*
 * Datei: PlayerPositionSession.cs
 * Zweck: Speichert und setzt die Spielerposition.
 * Verantwortung:
 *   - Position speichern
 *   - Position wiederherstellen
 *
 * Verwendet von:
 *   - SaveGameManager
 */
using UnityEngine;

public class PlayerPositionSession : MonoBehaviour
{
    private Vector3 savedPosition;

    public void SavePosition(Transform player)
    {
        savedPosition = player.position;
    }

    public void LoadPosition(Transform player)
    {
        player.position = savedPosition;
    }
}