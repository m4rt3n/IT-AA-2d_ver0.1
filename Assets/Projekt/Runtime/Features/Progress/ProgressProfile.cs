/*
 * Datei: ProgressProfile.cs
 * Zweck: Speichert den gesamten Lernfortschritt einer Spielersitzung.
 * Verantwortung: Haelt Quest-Fortschritt, Quiz-Statistik und Themenwerte im Speicher.
 * Abhaengigkeiten: QuestProgress, System.Serializable, System.Collections.Generic.
 * Verwendung: Wird vom ProgressManager verwaltet und kann spaeter vom SaveSystem serialisiert werden.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Progress
{
    [Serializable]
    public class ProgressProfile
    {
        public List<QuestProgress> Quests = new();
        public int TotalQuizAnswers;
        public int CorrectQuizAnswers;
        public List<string> CompletedQuizIds = new();
        public List<TopicProgress> Topics = new();

        public QuestProgress GetQuestProgress(string questId)
        {
            if (string.IsNullOrWhiteSpace(questId))
            {
                return null;
            }

            for (int i = 0; i < Quests.Count; i++)
            {
                QuestProgress quest = Quests[i];

                if (quest != null && quest.QuestId == questId)
                {
                    return quest;
                }
            }

            return null;
        }

        public QuestProgress GetOrCreateQuestProgress(QuestDefinition definition)
        {
            if (definition == null || !definition.HasValidId())
            {
                return null;
            }

            QuestProgress existing = GetQuestProgress(definition.QuestId);

            if (existing != null)
            {
                return existing;
            }

            QuestProgress created = new QuestProgress(definition);
            Quests.Add(created);
            return created;
        }

        public void RegisterQuizAnswer(string topic, bool isCorrect)
        {
            TotalQuizAnswers++;

            if (isCorrect)
            {
                CorrectQuizAnswers++;
            }

            TopicProgress topicProgress = GetOrCreateTopicProgress(topic);
            topicProgress.Answers++;

            if (isCorrect)
            {
                topicProgress.CorrectAnswers++;
            }
        }

        public void MarkQuizCompleted(string quizId)
        {
            if (string.IsNullOrWhiteSpace(quizId) || CompletedQuizIds.Contains(quizId))
            {
                return;
            }

            CompletedQuizIds.Add(quizId);
        }

        public TopicProgress GetOrCreateTopicProgress(string topic)
        {
            string resolvedTopic = string.IsNullOrWhiteSpace(topic) ? "General" : topic;

            for (int i = 0; i < Topics.Count; i++)
            {
                TopicProgress item = Topics[i];

                if (item != null && item.Topic == resolvedTopic)
                {
                    return item;
                }
            }

            TopicProgress created = new TopicProgress(resolvedTopic);
            Topics.Add(created);
            return created;
        }

        public float GetQuizAccuracy01()
        {
            return TotalQuizAnswers <= 0 ? 0f : Mathf.Clamp01((float)CorrectQuizAnswers / TotalQuizAnswers);
        }
    }

    [Serializable]
    public class TopicProgress
    {
        public string Topic;
        public int Answers;
        public int CorrectAnswers;

        public TopicProgress()
        {
        }

        public TopicProgress(string topic)
        {
            Topic = topic;
        }

        public float GetAccuracy01()
        {
            return Answers <= 0 ? 0f : Mathf.Clamp01((float)CorrectAnswers / Answers);
        }
    }
}
