/*
 * Datei: ScenarioManager.cs
 * Zweck: Steuert das Starten, Aktualisieren und Abschliessen eines IT-Lernszenarios.
 * Verantwortung: Haelt das aktive ScenarioDefinition-Asset, aktualisiert ScenarioProgress und meldet Statuswechsel per Debug.Log.
 * Abhaengigkeiten: ScenarioDefinition, ScenarioProgress, ScenarioStep, UnityEngine.
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

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        #endregion

        private ScenarioDefinition activeScenario;
        private readonly ScenarioProgress progress = new();

        public event Action<ScenarioStatus> StatusChanged;
        public event Action<ScenarioStep> CurrentStepChanged;

        public ScenarioDefinition ActiveScenario => activeScenario;
        public ScenarioProgress Progress => progress;
        public ScenarioStatus Status => progress.Status;

        #region Unity

        private void OnEnable()
        {
            if (startOnEnable)
            {
                StartScenario();
            }
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
            progress.Start(activeScenario.ScenarioId);

            NotifyStatusChanged(progress.Status);
            NotifyCurrentStepChanged();

            return true;
        }

        public ScenarioStep GetCurrentStep()
        {
            return progress.GetCurrentStep(activeScenario);
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

            if (progress.Status != previousStatus)
            {
                NotifyStatusChanged(progress.Status);
            }
        }

        public void ResetScenario()
        {
            activeScenario = null;
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
                    new("capture_problem", "Problem aufnehmen", "Sammle die Fehlerbeschreibung des Nutzers."),
                    new("check_dns_dhcp", "DNS/DHCP pruefen", "Pruefe IP-Konfiguration, Gateway, DNS und DHCP."),
                    new("confirm_solution", "Loesung bestaetigen", "Bestaetige, dass die Verbindung wieder funktioniert.")
                });

            return scenario;
        }

        #endregion
    }
}
