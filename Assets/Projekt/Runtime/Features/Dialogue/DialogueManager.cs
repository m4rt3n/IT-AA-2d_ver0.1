/*
 * Datei: DialogueManager.cs
 * Zweck: Startet, steuert und beendet Dialogsequenzen.
 * Verantwortung: Verwaltet aktuellen Dialogzustand, naechste Zeile und Abschluss-Callback.
 * Abhaengigkeiten: DialogueSequence, DialoguePanel, UnityEngine.
 * Verwendung: Wird als Szenen-Komponente genutzt und kann spaeter von NPCs, Quiz oder Szenario-Systemen angesprochen werden.
 */

using UnityEngine;

namespace ITAA.Features.Dialogue
{
    [DisallowMultipleComponent]
    public class DialogueManager : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private DialoguePanel dialoguePanel;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        #endregion

        private DialogueSequence activeSequence;
        private int currentLineIndex;
        private global::System.Action dialogueFinishedCallback;

        public bool IsDialogueActive => activeSequence != null;
        public DialogueSequence ActiveSequence => activeSequence;

        #region Unity

        private void Awake()
        {
            ResolveReferences();
        }

        #endregion

        #region Public API

        public void StartDialogue(DialogueSequence sequence)
        {
            StartDialogue(sequence, null);
        }

        public void StartDialogue(DialogueSequence sequence, global::System.Action finishedCallback)
        {
            ResolveReferences();

            if (sequence == null || !sequence.HasLines())
            {
                Debug.LogWarning($"[{nameof(DialogueManager)}] Dialogsequenz fehlt oder ist leer.", this);
                finishedCallback?.Invoke();
                return;
            }

            if (dialoguePanel == null)
            {
                Debug.LogWarning($"[{nameof(DialogueManager)}] DialoguePanel fehlt.", this);
                finishedCallback?.Invoke();
                return;
            }

            activeSequence = sequence;
            currentLineIndex = 0;
            dialogueFinishedCallback = finishedCallback;

            dialoguePanel.Show(this);
            ShowCurrentLine();

            Log($"Dialog gestartet: {sequence.DisplayName}");
        }

        public void ShowNextLine()
        {
            if (activeSequence == null)
            {
                EndDialogue();
                return;
            }

            currentLineIndex++;

            if (currentLineIndex >= activeSequence.Lines.Count)
            {
                EndDialogue();
                return;
            }

            ShowCurrentLine();
        }

        public void EndDialogue()
        {
            DialogueSequence finishedSequence = activeSequence;
            global::System.Action finishedCallback = dialogueFinishedCallback;

            activeSequence = null;
            currentLineIndex = 0;
            dialogueFinishedCallback = null;

            if (dialoguePanel != null)
            {
                dialoguePanel.Close();
            }

            Log(finishedSequence != null ? $"Dialog beendet: {finishedSequence.DisplayName}" : "Dialog beendet.");
            finishedCallback?.Invoke();
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (dialoguePanel == null)
            {
                dialoguePanel = FindAnyObjectByType<DialoguePanel>(FindObjectsInactive.Include);
            }
        }

        private void ShowCurrentLine()
        {
            if (dialoguePanel == null || activeSequence == null)
            {
                return;
            }

            DialogueLine line = activeSequence.GetLineAt(currentLineIndex);

            if (line == null || !line.HasText())
            {
                ShowNextLine();
                return;
            }

            bool isLastLine = currentLineIndex >= activeSequence.Lines.Count - 1;
            dialoguePanel.SetLine(line, isLastLine);
        }

        private void Log(string message)
        {
            if (!enableDebugLogs)
            {
                return;
            }

            Debug.Log($"[{nameof(DialogueManager)}] {message}", this);
        }

        #endregion
    }
}
