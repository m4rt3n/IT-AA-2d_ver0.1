/*
 * Datei: PlayerInventorySession.cs
 * Zweck: Verwaltet das Inventar des Spielers.
 * Verantwortung:
 *   - Items speichern
 *   - Items hinzufügen/entfernen
 *
 * Verwendet von:
 *   - Inventory UI
 */
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySession : MonoBehaviour
{
    private List<string> items = new List<string>();

    public void AddItem(string item)
    {
        items.Add(item);
        Debug.Log($"Item hinzugefügt: {item}");
    }

    public void RemoveItem(string item)
    {
        items.Remove(item);
    }

    public List<string> GetItems()
    {
        return items;
    }
}