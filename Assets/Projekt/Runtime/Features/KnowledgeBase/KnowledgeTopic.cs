/*
 * Datei: KnowledgeTopic.cs
 * Zweck: Beschreibt die grobe fachliche Kategorie eines Lexikonartikels.
 * Verantwortung: Stellt stabile Themenwerte fuer KnowledgeArticle, Filter und spaetere Quiz-/Szenario-Verknuepfungen bereit.
 * Abhaengigkeiten: Keine.
 * Verwendung: Wird vom KnowledgeBaseRepository und KnowledgeBasePanel zur Einordnung von Artikeln genutzt.
 */

namespace ITAA.Features.KnowledgeBase
{
    public enum KnowledgeTopic
    {
        Networking,
        Security,
        OperatingSystems,
        Support,
        Hardware,
        Model,
        Other
    }
}
