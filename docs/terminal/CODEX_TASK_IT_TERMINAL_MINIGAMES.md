# Codex Task: IT Terminal Minigames implementieren

## Ziel
Implementiere ein simuliertes Terminal für IT-Support-Minispiele.

## Kontext
Bitte vorher lesen:
- docs/FEATURE_REGISTRY.md
- docs/terminal/IT_TERMINAL_MINIGAMES_FEATURE.md

## Anforderungen
- Namespace: ITAA.Features.Terminal
- Keine echten OS-Befehle ausführen
- Keine echten Netzwerkzugriffe ausführen
- Keine Breaking Changes
- UI und Logik trennen

## Zu erstellen
- TerminalCommand.cs
- TerminalCommandResult.cs
- TerminalEmulator.cs
- TerminalPanel.cs
- TerminalCommandType.cs
- README.md im Feature-Ordner

## MVP-Kommandos
- help
- ipconfig
- ping
- nslookup
- clear
- exit

## Beispielausgaben
ipconfig:
- IPv4 Address: 192.168.1.42
- Subnet Mask: 255.255.255.0
- Default Gateway: 192.168.1.1

ping:
- Reply from 8.8.8.8
oder
- Request timed out

nslookup:
- Name: example.local
- Address: 192.168.1.10

## Wichtig
- Terminal ist rein simuliert
- Vollständige Dateien liefern
- Header-Kommentare ergänzen