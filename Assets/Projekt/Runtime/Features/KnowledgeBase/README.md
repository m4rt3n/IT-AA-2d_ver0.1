# Knowledge Base

## Zweck
Die Knowledge Base ist ein internes IT-Lexikon fuer Support- und Netzwerkgrundlagen. Sie ist als MVP lokal und datengetrieben aufgebaut, ohne externe Datenbank.

## Dateien
- `KnowledgeArticle.cs` beschreibt einen Artikel.
- `KnowledgeTopic.cs` kategorisiert Artikel.
- `KnowledgeBaseRepository.cs` liefert Demo-Artikel und Suche.
- `KnowledgeBasePanel.cs` zeigt Liste, Suche und Artikeldetails.
- `KnowledgeArticleListItemUI.cs` stellt einen Listeneintrag dar.
- `KnowledgeBaseHotkeyController.cs` oeffnet und schliesst das Panel per Hotkey.

## Demo-Artikel
- DNS
- DHCP
- Gateway
- VPN
- OSI-Modell

## Unity Setup
1. In der `StartScene` kann ein `KnowledgeBaseHotkeyController` manuell oder ueber einen spaeteren Bootstrapper gesetzt werden.
2. Standard-Hotkey: `K`.
3. Wenn kein `KnowledgeBasePanel` vorhanden ist, erzeugt der Controller ein Runtime-Canvas mit Panel.
4. Optional kann ein eigenes UI-GameObject in einem Canvas angelegt und mit `KnowledgeBasePanel` versehen werden.
5. Fuer MVP kann `Create Missing Ui` aktiv bleiben; dann erzeugt das Panel die benoetigte UI selbst.

## Verhalten
- Beim Start werden Demo-Artikel aus `KnowledgeBaseRepository` geladen.
- Das Panel startet geschlossen.
- `K` oeffnet und schliesst die Knowledge Base.
- Der Close-Button schliesst ebenfalls.
- Bei offenem Panel wird die Player-Bewegung ueber `PlayerController.SetMovementEnabled(false)` gesperrt und beim Schliessen wieder freigegeben.
- Die Liste zeigt alle Artikel.
- Suche filtert nach Titel.
- Klick auf einen Artikel zeigt Details.
- `OpenArticleById(string articleId)` kann spaeter von Quiz- oder Szenario-Systemen genutzt werden.

## Erweiterung
Spaeter kann das Repository durch JSON-, ScriptableObject- oder lokalisierte Daten ersetzt werden, ohne das Panel grundlegend umzubauen.
