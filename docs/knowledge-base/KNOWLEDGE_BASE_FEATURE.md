# Feature: Knowledge Base / Lexikon

## Ziel
Ein internes Nachschlagewerk für IT-Themen.

## Namespace
ITAA.Features.KnowledgeBase

## Themen
- DNS
- DHCP
- IP-Adresse
- Subnetzmaske
- Gateway
- VPN
- Windows
- Linux
- Drucker
- Ticketsystem
- OSI-Modell

## Funktionen
- Artikel anzeigen
- Artikel nach Thema filtern
- Suche nach Begriff
- Artikel aus Quiz-Erklärung öffnen
- Artikel aus Szenario öffnen

## Datenmodell

### KnowledgeArticle
- id
- title
- topic
- shortDescription
- content
- relatedQuizTopic
- relatedScenarioId

## MVP
- feste Demo-Artikel
- Liste anzeigen
- Artikel öffnen
- Suche nach Titel

## Spätere Erweiterungen
- Favoriten
- Fortschritt: gelesen/nicht gelesen
- Verlinkungen zwischen Artikeln
- DE/EN Lokalisierung