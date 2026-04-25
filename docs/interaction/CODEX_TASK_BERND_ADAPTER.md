# Codex Task: Bernd Interaction Adapter

## Ziel
Bernd soll an das neue World Interaction System angebunden werden, ohne bestehende Logik zu brechen.

## Kontext
Bitte vorher lesen:
- docs/core/FEATURE_REGISTRY.md
- docs/interaction/WORLD_INTERACTION_FEATURE.md
- docs/interaction/CODEX_TASK_WORLD_INTERACTION.md

## Problem
Aktuell:
- Bernd nutzt eigene Interaktionslogik (BerndAutoInteraction / QuizStarter)
- Kein einheitliches System mit anderen Interaktionen

Ziel:
- Bernd kompatibel mit IInteractable machen
- Bestehende Logik weiter nutzbar lassen

---

## Anforderungen

- Namespace: ITAA.NPC.Bernd
- KEIN Entfernen bestehender Bernd-Logik
- KEINE Breaking Changes
- Adapter-Pattern verwenden
- Kompatibel mit geplantem InteractionSystem

---

## Zu erstellen

### 1. BerndInteractableAdapter.cs

Funktion:
- Implementiert IInteractable
- Delegiert an bestehende Bernd-Logik

Erwartetes Verhalten:
- Player sieht "E drücken"
- Bei Interaktion:
  → Quiz starten (bestehender BerndQuizStarter)

---

## Beispielverhalten

```text
Player → nähert sich Bernd
→ InteractionPrompt erscheint

Player drückt E
→ BerndQuizStarter wird ausgelöst
→ Quiz startet