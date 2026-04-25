/*
 * Datei: KnowledgeArticle.cs
 * Zweck: Beschreibt einen einzelnen Artikel im internen IT-Lexikon.
 * Verantwortung: Haelt ID, Titel, Thema, Kurzbeschreibung, Inhalt und optionale Integrations-IDs.
 * Abhaengigkeiten: KnowledgeTopic, System.Serializable.
 * Verwendung: Wird vom KnowledgeBaseRepository bereitgestellt und vom KnowledgeBasePanel angezeigt.
 */

using System;

namespace ITAA.Features.KnowledgeBase
{
    [Serializable]
    public class KnowledgeArticle
    {
        public string Id;
        public string Title;
        public KnowledgeTopic Topic;
        public string ShortDescription;
        public string Content;
        public string RelatedQuizTopic;
        public string RelatedScenarioId;

        public KnowledgeArticle()
        {
        }

        public KnowledgeArticle(
            string id,
            string title,
            KnowledgeTopic topic,
            string shortDescription,
            string content,
            string relatedQuizTopic = "",
            string relatedScenarioId = "")
        {
            Id = id;
            Title = title;
            Topic = topic;
            ShortDescription = shortDescription;
            Content = content;
            RelatedQuizTopic = relatedQuizTopic;
            RelatedScenarioId = relatedScenarioId;
        }

        public bool HasValidId()
        {
            return !string.IsNullOrWhiteSpace(Id);
        }

        public bool MatchesTitleSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return true;
            }

            return !string.IsNullOrWhiteSpace(Title) &&
                   Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
