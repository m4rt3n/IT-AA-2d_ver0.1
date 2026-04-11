# Authentication

## Zweck
Verwaltet Login, Registrierung und Benutzerzugriffe.

## Inhalt
- AuthManager.cs: Hauptlogik für Login / Signup
- Legacy/: Alte UI-Implementierungen

## Verantwortlichkeit
Kontrolle des Benutzerzugriffs und Session-Start.

## Abhängigkeiten
- Data (User-Daten)
- UI (Login-Menü)

## Hinweise
Langfristig von UI trennen (Clean Architecture).