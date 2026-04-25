/*
 * Datei: KnowledgeBaseRepository.cs
 * Zweck: Stellt Lexikonartikel ohne externe Datenbank bereit.
 * Verantwortung: Verwaltet Demo-Artikel, Suche nach Titel und Zugriff per Artikel-ID.
 * Abhaengigkeiten: KnowledgeArticle, KnowledgeTopic, System.Collections.Generic.
 * Verwendung: Wird vom KnowledgeBasePanel genutzt und kann spaeter durch JSON- oder ScriptableObject-Daten ersetzt werden.
 */

using System.Collections.Generic;

namespace ITAA.Features.KnowledgeBase
{
    public class KnowledgeBaseRepository
    {
        private readonly List<KnowledgeArticle> articles = new();

        public KnowledgeBaseRepository()
        {
            articles.AddRange(CreateDemoArticles());
        }

        public KnowledgeBaseRepository(IEnumerable<KnowledgeArticle> initialArticles)
        {
            if (initialArticles == null)
            {
                articles.AddRange(CreateDemoArticles());
                return;
            }

            foreach (KnowledgeArticle article in initialArticles)
            {
                if (article != null)
                {
                    articles.Add(article);
                }
            }
        }

        public IReadOnlyList<KnowledgeArticle> GetAllArticles()
        {
            return articles;
        }

        public KnowledgeArticle GetById(string articleId)
        {
            if (string.IsNullOrWhiteSpace(articleId))
            {
                return null;
            }

            for (int i = 0; i < articles.Count; i++)
            {
                KnowledgeArticle article = articles[i];

                if (article != null && article.Id == articleId)
                {
                    return article;
                }
            }

            return null;
        }

        public IReadOnlyList<KnowledgeArticle> SearchByTitle(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetAllArticles();
            }

            List<KnowledgeArticle> results = new();

            for (int i = 0; i < articles.Count; i++)
            {
                KnowledgeArticle article = articles[i];

                if (article != null && article.MatchesTitleSearch(searchTerm))
                {
                    results.Add(article);
                }
            }

            return results;
        }

        public static IReadOnlyList<KnowledgeArticle> CreateDemoArticles()
        {
            return new List<KnowledgeArticle>
            {
                new(
                    "dns",
                    "DNS",
                    KnowledgeTopic.Networking,
                    "Uebersetzt Namen wie example.com in IP-Adressen.",
                    "DNS steht fuer Domain Name System. Es ist der Namensdienst des Netzwerks und beantwortet die Frage, welche IP-Adresse zu einem Hostnamen gehoert. Bei Problemen mit Webseiten ist DNS ein typischer erster Pruefpunkt.",
                    "networking",
                    "no_internet_basic"),
                new(
                    "dhcp",
                    "DHCP",
                    KnowledgeTopic.Networking,
                    "Vergibt IP-Konfiguration automatisch an Clients.",
                    "DHCP steht fuer Dynamic Host Configuration Protocol. Es liefert Clients automatisch IP-Adresse, Subnetzmaske, Gateway und DNS-Server. Ohne funktionierendes DHCP erhalten Clients oft keine gueltige Netzwerkkonfiguration.",
                    "networking",
                    "no_internet_basic"),
                new(
                    "gateway",
                    "Gateway",
                    KnowledgeTopic.Networking,
                    "Leitet Verkehr aus dem lokalen Netzwerk weiter.",
                    "Ein Gateway ist meist der Router im lokalen Netzwerk. Wenn ein Ziel nicht im eigenen Subnetz liegt, sendet der Client Pakete an das Standardgateway. Ein falsches Gateway kann Internetzugriff verhindern.",
                    "networking",
                    "no_internet_basic"),
                new(
                    "vpn",
                    "VPN",
                    KnowledgeTopic.Security,
                    "Stellt eine geschuetzte Verbindung in ein anderes Netzwerk her.",
                    "VPN steht fuer Virtual Private Network. Es verbindet ein Geraet verschluesselt mit einem entfernten Netzwerk. Typische Fehlerquellen sind falsche Zugangsdaten, abgelaufene Zertifikate oder blockierte Ports.",
                    "security"),
                new(
                    "osi_model",
                    "OSI-Modell",
                    KnowledgeTopic.Model,
                    "Ordnet Netzwerkkommunikation in sieben Schichten.",
                    "Das OSI-Modell beschreibt Netzwerkkommunikation in sieben Schichten: Physical, Data Link, Network, Transport, Session, Presentation und Application. Es hilft, Fehler systematisch einzugrenzen.",
                    "networking")
            };
        }
    }
}
