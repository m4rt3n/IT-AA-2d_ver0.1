/*
 * Datei: ScenarioManager.cs
 * Zweck: Steuert das Starten, Aktualisieren und Abschliessen eines IT-Lernszenarios.
 * Verantwortung: Haelt das aktive ScenarioDefinition-Asset, waehlt optionale Fehlerursachen, aktualisiert Timer und ScenarioProgress und meldet Statuswechsel per Debug.Log.
 * Abhaengigkeiten: ScenarioDefinition, ScenarioProgress, ScenarioStep, ScenarioFailureCause, ScenarioTimeLimit, UnityEngine.
 * Verwendung: Wird als MonoBehaviour in einer Szene platziert und kann spaeter von HUD, Quiz oder Dialogen angesprochen werden.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Scenarios
{
    [DisallowMultipleComponent]
    public class ScenarioManager : MonoBehaviour
    {
        #region Inspector

        [Header("Scenario")]
        [SerializeField] private ScenarioDefinition defaultScenario;
        [SerializeField] private bool useDemoScenarioIfMissing = true;
        [SerializeField] private bool startOnEnable;

        [Header("Failure Causes")]
        [SerializeField] private bool selectRandomFailureCauseOnStart = true;

        [Header("Timing")]
        [SerializeField] private bool updateTimer = true;
        [SerializeField] private bool failScenarioOnTimeout;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        private ScenarioDefinition activeScenario;
        private ScenarioFailureCause activeFailureCause;
        private readonly ScenarioProgress progress = new();

        public event Action<ScenarioStatus> StatusChanged;
        public event Action<ScenarioStep> CurrentStepChanged;
        public event Action<ScenarioTimerState> TimerChanged;
        public event Action<ScenarioTimerState> TimerTimedOut;

        public ScenarioDefinition ActiveScenario => activeScenario;
        public ScenarioFailureCause ActiveFailureCause => activeFailureCause;
        public ScenarioProgress Progress => progress;
        public ScenarioStatus Status => progress.Status;
        public float Progress01 => progress.GetProgress01(activeScenario);
        public ScenarioTimerState Timer => progress.Timer;

        #region Unity

        private void OnEnable()
        {
            if (startOnEnable)
            {
                StartScenario();
            }
        }

        private void Update()
        {
            if (!updateTimer || progress.Status != ScenarioStatus.Running)
            {
                return;
            }

            if (!progress.TickTimer(Time.deltaTime))
            {
                if (progress.Timer != null && progress.Timer.IsActive)
                {
                    TimerChanged?.Invoke(progress.Timer);
                }

                return;
            }

            NotifyTimerTimedOut();
        }

        #endregion

        #region Public API

        public bool StartScenario()
        {
            ScenarioDefinition scenario = defaultScenario;

            if (scenario == null && useDemoScenarioIfMissing)
            {
                scenario = CreateDemoScenario();
            }

            return StartScenario(scenario);
        }

        public bool StartScenario(ScenarioDefinition scenario)
        {
            return StartScenario(scenario, selectRandomFailureCauseOnStart
                ? ScenarioFailureCauseSelector.SelectRandom(scenario)
                : null);
        }

        public bool StartScenarioWithFailureCauseIndex(ScenarioDefinition scenario, int failureCauseIndex)
        {
            ScenarioFailureCause selectedCause = ScenarioFailureCauseSelector.SelectByIndex(scenario, failureCauseIndex);
            return StartScenario(scenario, selectedCause);
        }

        public bool StartScenarioWithFailureCauseId(ScenarioDefinition scenario, string failureCauseId)
        {
            ScenarioFailureCause selectedCause = ScenarioFailureCauseSelector.SelectById(scenario, failureCauseId);
            return StartScenario(scenario, selectedCause);
        }

        public bool StartScenario(ScenarioDefinition scenario, ScenarioFailureCause selectedFailureCause)
        {
            if (scenario == null)
            {
                Debug.LogWarning($"[{nameof(ScenarioManager)}] Kein Szenario gesetzt.", this);
                return false;
            }

            if (!scenario.HasValidId())
            {
                Debug.LogWarning($"[{nameof(ScenarioManager)}] Szenario hat keine ScenarioId: {scenario.name}", this);
                return false;
            }

            activeScenario = scenario;
            activeFailureCause = selectedFailureCause != null && selectedFailureCause.IsEnabled && selectedFailureCause.HasValidId()
                ? selectedFailureCause
                : null;
            progress.Start(activeScenario.ScenarioId, activeFailureCause);
            StartTimerForCurrentStep();

            NotifyStatusChanged(progress.Status);
            NotifyActiveFailureCauseChanged();
            NotifyCurrentStepChanged();

            return true;
        }

        public ScenarioStep GetCurrentStep()
        {
            return progress.GetCurrentStep(activeScenario);
        }

        public float GetRemainingTimeSeconds()
        {
            return progress.Timer != null ? progress.Timer.RemainingSeconds : 0f;
        }

        public bool CompleteCurrentStep()
        {
            if (activeScenario == null)
            {
                Debug.LogWarning($"[{nameof(ScenarioManager)}] Kein aktives Szenario vorhanden.", this);
                return false;
            }

            ScenarioStep completedStep = GetCurrentStep();
            ScenarioStatus previousStatus = progress.Status;

            if (!progress.CompleteCurrentStep(activeScenario))
            {
                return false;
            }

            if (enableDebugLogs && completedStep != null)
            {
                Debug.Log($"[{nameof(ScenarioManager)}] Schritt abgeschlossen: {completedStep.Title}", this);
            }

            if (progress.Status != previousStatus)
            {
                NotifyStatusChanged(progress.Status);
            }

            StartTimerForCurrentStep();
            NotifyCurrentStepChanged();
            return true;
        }

        public bool CompleteStepById(string stepId)
        {
            return CompleteSpecificStep(activeScenario != null ? activeScenario.FindStepById(stepId) : null);
        }

        public bool CompleteStepByCompletionKey(string completionKey)
        {
            return CompleteSpecificStep(activeScenario != null ? activeScenario.FindStepByCompletionKey(completionKey) : null);
        }

        public bool CompleteStepByLinkedQuiz(string quizId)
        {
            return CompleteSpecificStep(activeScenario != null ? activeScenario.FindStepByLinkedQuizId(quizId) : null);
        }

        public bool CompleteStepByLinkedDialogue(string dialogueId)
        {
            return CompleteSpecificStep(activeScenario != null ? activeScenario.FindStepByLinkedDialogueId(dialogueId) : null);
        }

        public bool SkipCurrentOptionalStep()
        {
            if (activeScenario == null)
            {
                Debug.LogWarning($"[{nameof(ScenarioManager)}] Kein aktives Szenario vorhanden.", this);
                return false;
            }

            ScenarioStatus previousStatus = progress.Status;

            if (!progress.SkipCurrentOptionalStep(activeScenario))
            {
                return false;
            }

            if (progress.Status != previousStatus)
            {
                NotifyStatusChanged(progress.Status);
            }

            StartTimerForCurrentStep();
            NotifyCurrentStepChanged();
            return true;
        }

        public void CompleteScenario()
        {
            if (activeScenario == null)
            {
                Debug.LogWarning($"[{nameof(ScenarioManager)}] Kein aktives Szenario vorhanden.", this);
                return;
            }

            ScenarioStatus previousStatus = progress.Status;
            progress.CompleteScenario(activeScenario);
            progress.ClearTimer();

            if (progress.Status != previousStatus)
            {
                NotifyStatusChanged(progress.Status);
            }

            NotifyCurrentStepChanged();
        }

        public void FailScenario()
        {
            ScenarioStatus previousStatus = progress.Status;
            progress.FailScenario();
            progress.ClearTimer();

            if (progress.Status != previousStatus)
            {
                NotifyStatusChanged(progress.Status);
            }
        }

        public void ResetScenario()
        {
            activeScenario = null;
            activeFailureCause = null;
            ScenarioStatus previousStatus = progress.Status;
            progress.Reset();

            if (progress.Status != previousStatus)
            {
                NotifyStatusChanged(progress.Status);
            }

            NotifyCurrentStepChanged();
        }

        #endregion

        #region Private

        private void NotifyStatusChanged(ScenarioStatus status)
        {
            if (enableDebugLogs)
            {
                string scenarioTitle = activeScenario != null ? activeScenario.Title : "Kein Szenario";
                Debug.Log($"[{nameof(ScenarioManager)}] Status: {status} ({scenarioTitle})", this);
            }

            StatusChanged?.Invoke(status);
        }

        private void NotifyActiveFailureCauseChanged()
        {
            if (!enableDebugLogs)
            {
                return;
            }

            if (activeFailureCause == null)
            {
                Debug.Log($"[{nameof(ScenarioManager)}] Keine aktive Fehlerursache.", this);
                return;
            }

            Debug.Log(
                $"[{nameof(ScenarioManager)}] Aktive Fehlerursache: " +
                $"{activeFailureCause.GetResolvedTitle()} ({activeFailureCause.CauseId})",
                this);
        }

        private void NotifyCurrentStepChanged()
        {
            ScenarioStep currentStep = GetCurrentStep();

            if (enableDebugLogs)
            {
                string stepTitle = currentStep != null ? currentStep.Title : "Kein aktiver Schritt";
                Debug.Log($"[{nameof(ScenarioManager)}] Aktueller Schritt: {stepTitle}", this);
            }

            CurrentStepChanged?.Invoke(currentStep);
        }

        private void StartTimerForCurrentStep()
        {
            if (progress.Status != ScenarioStatus.Running)
            {
                progress.ClearTimer();
                TimerChanged?.Invoke(progress.Timer);
                return;
            }

            ScenarioTimeLimit timeLimit = ResolveActiveTimeLimit(out string fallbackTimerId);

            if (timeLimit == null)
            {
                progress.ClearTimer();
                TimerChanged?.Invoke(progress.Timer);
                return;
            }

            progress.StartTimer(timeLimit, fallbackTimerId);
            TimerChanged?.Invoke(progress.Timer);

            if (enableDebugLogs)
            {
                Debug.Log(
                    $"[{nameof(ScenarioManager)}] Timer gestartet: " +
                    $"{progress.Timer.TimerId}, {progress.Timer.DurationSeconds:0.##}s",
                    this);
            }
        }

        private ScenarioTimeLimit ResolveActiveTimeLimit(out string fallbackTimerId)
        {
            fallbackTimerId = activeScenario != null ? activeScenario.ScenarioId : "scenario_timer";

            ScenarioStep currentStep = GetCurrentStep();

            if (currentStep != null)
            {
                fallbackTimerId = currentStep.HasValidId() ? currentStep.StepId : fallbackTimerId;

                if (currentStep.HasTimeLimit())
                {
                    return currentStep.TimeLimit;
                }
            }

            if (activeScenario != null && activeScenario.HasScenarioTimeLimit())
            {
                return activeScenario.TimeLimit;
            }

            return null;
        }

        private void NotifyTimerTimedOut()
        {
            if (enableDebugLogs && progress.Timer != null)
            {
                Debug.LogWarning($"[{nameof(ScenarioManager)}] Timer abgelaufen: {progress.Timer.TimerId}", this);
            }

            TimerTimedOut?.Invoke(progress.Timer);
            TimerChanged?.Invoke(progress.Timer);

            if (failScenarioOnTimeout)
            {
                FailScenario();
            }
        }

        private bool CompleteSpecificStep(ScenarioStep step)
        {
            if (activeScenario == null)
            {
                Debug.LogWarning($"[{nameof(ScenarioManager)}] Kein aktives Szenario vorhanden.", this);
                return false;
            }

            if (step == null || string.IsNullOrWhiteSpace(step.StepId))
            {
                Debug.LogWarning($"[{nameof(ScenarioManager)}] Szenario-Schritt nicht gefunden.", this);
                return false;
            }

            ScenarioStatus previousStatus = progress.Status;
            bool completesCurrentStep = GetCurrentStep() == step;

            if (!progress.CompleteStep(activeScenario, step.StepId))
            {
                return false;
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(ScenarioManager)}] Schritt abgeschlossen: {step.Title}", this);
            }

            if (progress.Status != previousStatus)
            {
                NotifyStatusChanged(progress.Status);
            }

            if (completesCurrentStep)
            {
                StartTimerForCurrentStep();
            }

            NotifyCurrentStepChanged();
            return true;
        }

        private static ScenarioDefinition CreateDemoScenario()
        {
            ScenarioDefinition scenario = ScriptableObject.CreateInstance<ScenarioDefinition>();
            scenario.name = "DemoScenario_NoInternetBasic";
            scenario.Initialize(
                "no_internet_basic",
                "Kein Internet",
                "Ein einfaches Netzwerk-Szenario fuer den Einstieg.",
                "Netzwerk",
                "Easy",
                new List<ScenarioStep>
                {
                    new("capture_problem", "Problem aufnehmen", "Sammle die Fehlerbeschreibung des Nutzers.")
                    {
                        StepType = ScenarioStepType.Dialogue,
                        CompletionKey = "dialogue_capture_problem",
                        LinkedDialogueId = "scenario_no_internet_intro"
                    },
                    new("check_dns_dhcp", "DNS/DHCP pruefen", "Pruefe IP-Konfiguration, Gateway, DNS und DHCP.")
                    {
                        StepType = ScenarioStepType.Task,
                        CompletionKey = "task_check_dns_dhcp",
                        ProgressQuestId = "answer_3_dns_questions",
                        TimeLimit = new ScenarioTimeLimit("check_dns_dhcp_timer", 180f)
                        {
                            WarningThresholdSeconds = 30f
                        }
                    },
                    new("confirm_solution", "Loesung bestaetigen", "Bestaetige, dass die Verbindung wieder funktioniert.")
                    {
                        StepType = ScenarioStepType.Checkpoint,
                        CompletionKey = "checkpoint_confirm_solution"
                    }
                });

            scenario.FailureCauses = new List<ScenarioFailureCause>
            {
                new("dns_wrong_server", "Falscher DNS-Server", "Der Client nutzt einen DNS-Server, der keine externen Namen aufloesen kann.")
                {
                    Hint = "Vergleiche DNS-Eintrag und Gateway-Konfiguration.",
                    RelatedStepId = "check_dns_dhcp",
                    RelatedKnowledgeArticleId = "dns"
                },
                new("dhcp_missing_gateway", "Fehlendes Gateway per DHCP", "Der Client hat eine IP-Adresse, aber kein gueltiges Standardgateway erhalten.")
                {
                    Hint = "Pruefe DHCP-Lease, Gateway und Subnetz.",
                    RelatedStepId = "check_dns_dhcp",
                    RelatedKnowledgeArticleId = "dhcp"
                },
                new("router_gateway_down", "Gateway nicht erreichbar", "Der lokale Router oder das Standardgateway antwortet nicht.")
                {
                    Hint = "Ping auf das Gateway und Verkabelung/WLAN pruefen.",
                    RelatedStepId = "check_dns_dhcp",
                    RelatedKnowledgeArticleId = "gateway"
                }
            };

            return scenario;
        }

        #endregion
    }
}
