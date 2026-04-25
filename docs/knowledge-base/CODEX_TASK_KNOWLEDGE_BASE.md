# Codex Task: Knowledge Base implementieren

## Ziel
Implementiere ein einfaches Lexikon für IT-Themen.

## Kontext
Bitte vorher lesen:
- docs/FEATURE_REGISTRY.md
- docs/knowledge-base/KNOWLEDGE_BASE_FEATURE.md

## Anforderungen
- Namespace: ITAA.Features.KnowledgeBase
- Keine Breaking Changes
- UI und Datenmodell trennen
- Keine externe Datenbank nötig

## Zu erstellen
- KnowledgeArticle.cs
- KnowledgeBaseRepository.cs
- KnowledgeBasePanel.cs
- KnowledgeArticleListItemUI.cs
- KnowledgeTopic.cs
- README.md im Feature-Ordner

## Demo-Artikel
- DNS
- DHCP
- Gateway
- VPN
- OSI-Modell

## Verhalten
- Artikel-Liste anzeigen
- Artikel öffnen
- nach Titel suchen
- bei fehlendem Artikel Debug.LogWarning

## Wichtig
- Vollständige Dateien liefern
- Header-Kommentare ergänzen
- Quiz-/Scenario-Integration nur vorbereiten