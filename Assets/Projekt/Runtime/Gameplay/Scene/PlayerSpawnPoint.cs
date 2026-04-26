/*
 * Datei: PlayerSpawnPoint.cs
 * Zweck: Markiert einen sicheren Einstiegspunkt fuer den Player in Gameplay-Szenen.
 * Verantwortung: Speichert eine SpawnId und stellt Position/Rotation fuer Bootstrapper oder Editor-Tools bereit.
 * Abhaengigkeiten: Unity Transform, Gizmos.
 * Verwendung: Wird in GameScene auf Spawn-GameObjects gelegt; Default SpawnId ist "Default".
 */

using UnityEngine;

namespace ITAA.Gameplay.Scene
{
    public sealed class PlayerSpawnPoint : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private string spawnId = "Default";

        [Header("Gizmo")]
        [SerializeField] private Color gizmoColor = new Color(0.2f, 0.85f, 1f, 0.9f);
        [SerializeField] private float gizmoRadius = 0.35f;

        public string SpawnId => string.IsNullOrWhiteSpace(spawnId) ? "Default" : spawnId;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, gizmoRadius);
            Gizmos.DrawLine(transform.position + Vector3.left * gizmoRadius, transform.position + Vector3.right * gizmoRadius);
            Gizmos.DrawLine(transform.position + Vector3.down * gizmoRadius, transform.position + Vector3.up * gizmoRadius);
        }
    }
}
