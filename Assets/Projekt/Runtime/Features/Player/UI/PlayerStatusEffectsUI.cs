/*
 * Datei: PlayerStatusEffectsUI.cs
 * Zweck: Zeigt aktive Status-Effekte des Spielers an.
 * Verantwortung:
 *   - Anzeige von Buffs/Debuffs
 *   - Verwaltung von Icons
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ITAA.Player.UI
{
    public class PlayerStatusEffectsUI : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private GameObject effectIconPrefab;

        private List<GameObject> activeIcons = new List<GameObject>();

        public void AddEffect(Sprite icon)
        {
            GameObject obj = Instantiate(effectIconPrefab, container);
            obj.GetComponent<Image>().sprite = icon;
            activeIcons.Add(obj);
        }

        public void ClearEffects()
        {
            foreach (var icon in activeIcons)
            {
                Destroy(icon);
            }

            activeIcons.Clear();
        }
    }
}
