/*
 * Datei: PlayerMovementConfig.cs
 * Zweck: Enthält konfigurierbare Bewegungswerte für den Spieler.
 * Verantwortung:
 *   - Bereitstellen von Lauf- und Sprintwerten
 *   - Zentrale Konfigurationsstelle für Bewegung
 *
 * Abhängigkeiten:
 *   - ScriptableObject
 *
 * Verwendet von:
 *   - PlayerController
 *   - PlayerMotor2D
 *   - PlayerSprintController
 */
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Projekt/Player/Movement Config")]
public class PlayerMovementConfig : ScriptableObject
{
    public float walkSpeed = 5f;
    public float sprintMultiplier = 1.5f;
}