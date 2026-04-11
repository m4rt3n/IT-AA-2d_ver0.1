# 📦 ScriptableObjects

## Zweck
Der Ordner enthält ScriptableObjects zur datengetriebenen Steuerung des Spiels.

## Vorteile
- Trennung von Daten und Logik
- Wiederverwendbarkeit
- einfache Anpassung im Editor

## Verwendung
ScriptableObjects werden genutzt für:
- Konfigurationen
- Balancing-Werte
- Spielparameter

## Beispiele
- PlayerStats
- NPC-Daten
- Item-Definitionen

## Architektur
ScriptableObjects werden von Runtime-Systemen genutzt:

Runtime → lädt Daten → verwendet Werte im Spiel

## Hinweis
ScriptableObjects enthalten keine Logik, sondern nur Daten.