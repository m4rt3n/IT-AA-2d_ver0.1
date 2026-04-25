/*
 * Datei: ProgressManager.cs
 * Zweck: Verwaltet Quest- und Lernfortschritt im Speicher.
 * Verantwortung: Startet Quests, erhoeht Fortschritt, schliesst Quests ab und protokolliert Quiz-Ergebnisse.
 * Abhaengigkeiten: QuestDefinition, QuestProgress, ProgressProfile, UnityEngine.
 * Verwendung: Wird als optionale Szenen-Komponente genutzt und spaeter an Quiz, NPCs und SaveSystem angebunden.
 */

using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Progress
{
    [DisallowMultipleComponent]
    public class ProgressManager : MonoBehaviour
    {
        #region Inspector

        [Header("Quest Definitions")]
        [SerializeField] private List<QuestDefinition> questDefinitions = new();
        [SerializeField] private bool createDemoQuestDefinitions = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        private readonly ProgressProfile profile = new();
        private readonly Dictionary<string, QuestDefinition> definitionsById = new();

        public ProgressProfile Profile => profile;

        #region Unity

        private void Awake()
        {
            RebuildDefinitionLookup();
        }

        #endregion

        #region Public API

        public void StartQuest(string questId)
        {
            QuestDefinition definition = GetQuestDefinition(questId);

            if (definition == null)
            {
                Debug.LogWarning($"[{nameof(ProgressManager)}] Quest nicht gefunden: {questId}", this);
                return;
            }

            QuestProgress progress = profile.GetOrCreateQuestProgress(definition);
            Log($"Quest gestartet: {definition.QuestId} ({progress.CurrentValue}/{progress.TargetValue})");
        }

        public void AddQuestProgress(string questId, int amount = 1)
        {
            QuestDefinition definition = GetQuestDefinition(questId);

            if (definition == null)
            {
                Debug.LogWarning($"[{nameof(ProgressManager)}] Quest nicht gefunden: {questId}", this);
                return;
            }

            QuestProgress progress = profile.GetOrCreateQuestProgress(definition);
            bool wasCompleted = progress.IsCompleted;
            progress.AddProgress(amount);

            Log($"Quest-Fortschritt: {definition.QuestId} ({progress.CurrentValue}/{progress.TargetValue})");

            if (!wasCompleted && progress.IsCompleted)
            {
                Log($"Quest abgeschlossen: {definition.QuestId}");
            }
        }

        public void CompleteQuest(string questId)
        {
            QuestDefinition definition = GetQuestDefinition(questId);

            if (definition == null)
            {
                Debug.LogWarning($"[{nameof(ProgressManager)}] Quest nicht gefunden: {questId}", this);
                return;
            }

            QuestProgress progress = profile.GetOrCreateQuestProgress(definition);
            progress.Complete();
            Log($"Quest abgeschlossen: {definition.QuestId}");
        }

        public void ReportQuizAnswer(string topic, bool isCorrect)
        {
            profile.RegisterQuizAnswer(topic, isCorrect);

            if (isCorrect)
            {
                AddQuestProgress("answer_3_dns_questions", 1);
            }

            Log(
                $"Quiz-Antwort gemeldet: topic={ResolveTopic(topic)}, correct={isCorrect}, " +
                $"gesamt={profile.TotalQuizAnswers}, richtig={profile.CorrectQuizAnswers}");
        }

        public void ReportQuizCompleted(string quizId, string difficulty, int correctAnswers, int totalAnswers)
        {
            profile.MarkQuizCompleted(quizId);

            if (string.Equals(difficulty, "Easy", global::System.StringComparison.OrdinalIgnoreCase) && totalAnswers > 0 && correctAnswers >= totalAnswers)
            {
                CompleteQuest("complete_easy_quiz");
            }

            Log($"Quiz abgeschlossen: quizId={quizId}, difficulty={difficulty}, score={correctAnswers}/{totalAnswers}");
        }

        public QuestProgress GetQuestProgress(string questId)
        {
            return profile.GetQuestProgress(questId);
        }

        public void PrintProgress()
        {
            Log(
                $"ProgressProfile: quests={profile.Quests.Count}, quizAnswers={profile.TotalQuizAnswers}, " +
                $"correct={profile.CorrectQuizAnswers}, accuracy={Mathf.RoundToInt(profile.GetQuizAccuracy01() * 100f)}%");
        }

        #endregion

        #region Private

        private QuestDefinition GetQuestDefinition(string questId)
        {
            if (string.IsNullOrWhiteSpace(questId))
            {
                return null;
            }

            if (definitionsById.Count == 0)
            {
                RebuildDefinitionLookup();
            }

            definitionsById.TryGetValue(questId, out QuestDefinition definition);
            return definition;
        }

        private void RebuildDefinitionLookup()
        {
            definitionsById.Clear();

            if (questDefinitions != null)
            {
                for (int i = 0; i < questDefinitions.Count; i++)
                {
                    RegisterDefinition(questDefinitions[i]);
                }
            }

            if (createDemoQuestDefinitions)
            {
                RegisterDemoQuest("talk_to_bernd", "Sprich mit Bernd", "Starte ein Gespraech oder eine Interaktion mit Bernd.", "NPC", 1);
                RegisterDemoQuest("answer_3_dns_questions", "Beantworte 3 DNS-Fragen", "Beantworte drei Fragen zum Thema DNS korrekt.", "DNS", 3);
                RegisterDemoQuest("complete_easy_quiz", "Bestehe Easy Quiz", "Schliesse ein leichtes Quiz fehlerfrei ab.", "Quiz", 1);
            }
        }

        private void RegisterDemoQuest(string questId, string title, string description, string topic, int targetValue)
        {
            if (definitionsById.ContainsKey(questId))
            {
                return;
            }

            QuestDefinition definition = ScriptableObject.CreateInstance<QuestDefinition>();
            definition.name = $"DemoQuest_{questId}";
            definition.QuestId = questId;
            definition.Title = title;
            definition.Description = description;
            definition.Topic = topic;
            definition.TargetValue = Mathf.Max(1, targetValue);

            RegisterDefinition(definition);
        }

        private void RegisterDefinition(QuestDefinition definition)
        {
            if (definition == null || !definition.HasValidId())
            {
                return;
            }

            definitionsById[definition.QuestId] = definition;
        }

        private static string ResolveTopic(string topic)
        {
            return string.IsNullOrWhiteSpace(topic) ? "General" : topic;
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(ProgressManager)}] {message}", this);
        }

        #endregion
    }
}
