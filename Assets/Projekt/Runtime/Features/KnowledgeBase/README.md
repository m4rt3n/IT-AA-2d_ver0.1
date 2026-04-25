# Knowledge Base

## Zweck
Die Knowledge Base ist ein internes IT-Lexikon fuer Support- und Netzwerkgrundlagen. Sie ist als MVP lokal und datengetrieben aufgebaut, ohne externe Datenbank.

## Dateien
- `KnowledgeArticle.cs` beschreibt einen Artikel.
- `KnowledgeTopic.cs` kategorisiert Artikel.
- `KnowledgeBaseRepository.cs` liefert Demo-Artikel und Suche.
- `KnowledgeBasePanel.cs` zeigt Liste, Suche und Artikeldetails.
- `KnowledgeArticleListItemUI.cs` stellt einen Listeneintrag dar.

## Demo-Artikel
- DNS
- DHCP
- Gateway
- VPN
- OSI-Modell

## Unity Setup
1. Ein UI-GameObject in einem Canvas anlegen.
2. `KnowledgeBasePanel` hinzufuegen.
3. Optional Inspector-Referenzen setzen.
4. Fuer MVP kann `Create Missing Ui` aktiv bleiben; dann erzeugt das Panel die benoetigte UI selbst.

## Verhalten
- Beim Start werden Demo-Artikel aus `KnowledgeBaseRepository` geladen.
- Die Liste zeigt alle Artikel.
- Suche filtert nach Titel.
- Klick auf einen Artikel zeigt Details.
- `OpenArticleById(string articleId)` kann spaeter von Quiz- oder Szenario-Systemen genutzt werden.

## Erweiterung
Spaeter kann das Repository durch JSON-, ScriptableObject- oder lokalisierte Daten ersetzt werden, ohne das Panel grundlegend umzubauen.
