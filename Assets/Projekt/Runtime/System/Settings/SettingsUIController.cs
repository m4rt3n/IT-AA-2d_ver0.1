/*
 * Datei: SettingsUIController.cs
 * Zweck: Verbindet optionale UI-Steuerelemente mit dem zentralen SettingsManager.
 * Verantwortung: Liest UI-Werte, schreibt sie in SettingsData und aktualisiert die UI beim Oeffnen oder Reset.
 * Abhaengigkeiten: SettingsManager, Unity UI, TextMeshPro.
 * Verwendung: Wird auf einem SettingsPanel oder Child-Objekt platziert und per Inspector mit Slidern, Toggles und Dropdowns verbunden.
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.System.Settings
{
    public class SettingsUIController : MonoBehaviour
    {
        private const int SlowTextSpeedIndex = 0;
        private const int NormalTextSpeedIndex = 1;
        private const int FastTextSpeedIndex = 2;

        [Header("Audio")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        [Header("Video")]
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle vSyncToggle;

        [Header("Input")]
        [SerializeField] private TMP_InputField interactKeyInput;
        [SerializeField] private TMP_InputField moveUpKeyInput;
        [SerializeField] private TMP_InputField moveDownKeyInput;
        [SerializeField] private TMP_InputField moveLeftKeyInput;
        [SerializeField] private TMP_InputField moveRightKeyInput;

        [Header("Gameplay")]
        [SerializeField] private TMP_Dropdown textSpeedDropdown;
        [SerializeField] private Toggle showTutorialsToggle;

        [Header("Actions")]
        [SerializeField] private Button applyButton;
        [SerializeField] private Button resetButton;

        [Header("Behaviour")]
        [SerializeField] private bool applyChangesImmediately = true;

        private SettingsManager settingsManager;
        private bool isRefreshingUi;

        private void Awake()
        {
            settingsManager = SettingsManager.GetOrCreate();
            EnsureDropdownOptions();
            WireControls();
        }

        private void OnEnable()
        {
            settingsManager = SettingsManager.GetOrCreate();

            if (settingsManager != null)
            {
                settingsManager.SettingsChanged += HandleSettingsChanged;
                RefreshUi(settingsManager.GetSettings());
            }
        }

        private void OnDisable()
        {
            if (settingsManager != null)
            {
                settingsManager.SettingsChanged -= HandleSettingsChanged;
            }
        }

        private void OnDestroy()
        {
            UnwireControls();
        }

        public void ApplyFromUi()
        {
            if (settingsManager == null)
            {
                settingsManager = SettingsManager.GetOrCreate();
            }

            if (settingsManager == null)
            {
                return;
            }

            SettingsData settings = settingsManager.GetSettings();

            settings.MasterVolume = GetSliderValue(masterVolumeSlider, settings.MasterVolume);
            settings.MusicVolume = GetSliderValue(musicVolumeSlider, settings.MusicVolume);
            settings.SfxVolume = GetSliderValue(sfxVolumeSlider, settings.SfxVolume);

            settings.Fullscreen = GetToggleValue(fullscreenToggle, settings.Fullscreen);
            settings.VSync = GetToggleValue(vSyncToggle, settings.VSync);
            ApplySelectedResolution(settings);

            settings.InteractKey = GetInputText(interactKeyInput, settings.InteractKey);
            settings.MoveUpKey = GetInputText(moveUpKeyInput, settings.MoveUpKey);
            settings.MoveDownKey = GetInputText(moveDownKeyInput, settings.MoveDownKey);
            settings.MoveLeftKey = GetInputText(moveLeftKeyInput, settings.MoveLeftKey);
            settings.MoveRightKey = GetInputText(moveRightKeyInput, settings.MoveRightKey);

            settings.TextSpeed = GetSelectedTextSpeed(settings.TextSpeed);
            settings.ShowTutorials = GetToggleValue(showTutorialsToggle, settings.ShowTutorials);

            settings.Sanitize();
            settingsManager.ApplySettings();
            settingsManager.SaveSettings();
        }

        public void ResetToDefaults()
        {
            settingsManager ??= SettingsManager.GetOrCreate();
            settingsManager?.ResetToDefaults();
        }

        private void WireControls()
        {
            WireSlider(masterVolumeSlider);
            WireSlider(musicVolumeSlider);
            WireSlider(sfxVolumeSlider);
            WireToggle(fullscreenToggle);
            WireToggle(vSyncToggle);
            WireToggle(showTutorialsToggle);
            WireDropdown(resolutionDropdown);
            WireDropdown(textSpeedDropdown);
            WireInput(interactKeyInput);
            WireInput(moveUpKeyInput);
            WireInput(moveDownKeyInput);
            WireInput(moveLeftKeyInput);
            WireInput(moveRightKeyInput);

            if (applyButton != null)
            {
                applyButton.onClick.RemoveListener(ApplyFromUi);
                applyButton.onClick.AddListener(ApplyFromUi);
            }

            if (resetButton != null)
            {
                resetButton.onClick.RemoveListener(ResetToDefaults);
                resetButton.onClick.AddListener(ResetToDefaults);
            }
        }

        private void UnwireControls()
        {
            UnwireSlider(masterVolumeSlider);
            UnwireSlider(musicVolumeSlider);
            UnwireSlider(sfxVolumeSlider);
            UnwireToggle(fullscreenToggle);
            UnwireToggle(vSyncToggle);
            UnwireToggle(showTutorialsToggle);
            UnwireDropdown(resolutionDropdown);
            UnwireDropdown(textSpeedDropdown);
            UnwireInput(interactKeyInput);
            UnwireInput(moveUpKeyInput);
            UnwireInput(moveDownKeyInput);
            UnwireInput(moveLeftKeyInput);
            UnwireInput(moveRightKeyInput);

            if (applyButton != null)
            {
                applyButton.onClick.RemoveListener(ApplyFromUi);
            }

            if (resetButton != null)
            {
                resetButton.onClick.RemoveListener(ResetToDefaults);
            }
        }

        private void HandleSettingsChanged(SettingsData settings)
        {
            RefreshUi(settings);
        }

        private void RefreshUi(SettingsData settings)
        {
            if (settings == null)
            {
                return;
            }

            isRefreshingUi = true;

            SetSliderValue(masterVolumeSlider, settings.MasterVolume);
            SetSliderValue(musicVolumeSlider, settings.MusicVolume);
            SetSliderValue(sfxVolumeSlider, settings.SfxVolume);

            SetToggleValue(fullscreenToggle, settings.Fullscreen);
            SetToggleValue(vSyncToggle, settings.VSync);
            SetResolutionValue(settings.ResolutionWidth, settings.ResolutionHeight);

            SetInputText(interactKeyInput, settings.InteractKey);
            SetInputText(moveUpKeyInput, settings.MoveUpKey);
            SetInputText(moveDownKeyInput, settings.MoveDownKey);
            SetInputText(moveLeftKeyInput, settings.MoveLeftKey);
            SetInputText(moveRightKeyInput, settings.MoveRightKey);

            SetTextSpeedValue(settings.TextSpeed);
            SetToggleValue(showTutorialsToggle, settings.ShowTutorials);

            isRefreshingUi = false;
        }

        private void HandleImmediateChange(float value)
        {
            TryApplyImmediate();
        }

        private void HandleImmediateChange(bool value)
        {
            TryApplyImmediate();
        }

        private void HandleImmediateChange(int value)
        {
            TryApplyImmediate();
        }

        private void HandleImmediateChange(string value)
        {
            TryApplyImmediate();
        }

        private void TryApplyImmediate()
        {
            if (isRefreshingUi || !applyChangesImmediately)
            {
                return;
            }

            ApplyFromUi();
        }

        private void EnsureDropdownOptions()
        {
            if (textSpeedDropdown != null && textSpeedDropdown.options.Count == 0)
            {
                textSpeedDropdown.options.Add(new TMP_Dropdown.OptionData("slow"));
                textSpeedDropdown.options.Add(new TMP_Dropdown.OptionData("normal"));
                textSpeedDropdown.options.Add(new TMP_Dropdown.OptionData("fast"));
            }

            if (resolutionDropdown != null && resolutionDropdown.options.Count == 0)
            {
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1280 x 720"));
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1920 x 1080"));
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("2560 x 1440"));
            }
        }

        private void ApplySelectedResolution(SettingsData settings)
        {
            if (resolutionDropdown == null || resolutionDropdown.options.Count == 0)
            {
                return;
            }

            string selected = resolutionDropdown.options[resolutionDropdown.value].text;
            string[] parts = selected.ToLowerInvariant().Split('x');

            if (parts.Length != 2)
            {
                return;
            }

            if (int.TryParse(parts[0].Trim(), out int width) &&
                int.TryParse(parts[1].Trim(), out int height))
            {
                settings.ResolutionWidth = width;
                settings.ResolutionHeight = height;
            }
        }

        private string GetSelectedTextSpeed(string fallback)
        {
            if (textSpeedDropdown == null || textSpeedDropdown.options.Count == 0)
            {
                return fallback;
            }

            return textSpeedDropdown.options[textSpeedDropdown.value].text;
        }

        private void SetResolutionValue(int width, int height)
        {
            if (resolutionDropdown == null || resolutionDropdown.options.Count == 0)
            {
                return;
            }

            string targetText = $"{width} x {height}";

            for (int i = 0; i < resolutionDropdown.options.Count; i++)
            {
                if (resolutionDropdown.options[i].text == targetText)
                {
                    resolutionDropdown.SetValueWithoutNotify(i);
                    return;
                }
            }
        }

        private void SetTextSpeedValue(string textSpeed)
        {
            if (textSpeedDropdown == null)
            {
                return;
            }

            int index = textSpeed switch
            {
                "slow" => SlowTextSpeedIndex,
                "fast" => FastTextSpeedIndex,
                _ => NormalTextSpeedIndex
            };

            if (textSpeedDropdown.options.Count > index)
            {
                textSpeedDropdown.SetValueWithoutNotify(index);
            }
        }

        private static void WireSlider(Slider slider)
        {
            if (slider == null)
            {
                return;
            }

            SettingsUIController controller = slider.GetComponentInParent<SettingsUIController>(true);
            if (controller != null)
            {
                slider.onValueChanged.RemoveListener(controller.HandleImmediateChange);
                slider.onValueChanged.AddListener(controller.HandleImmediateChange);
            }
        }

        private static void UnwireSlider(Slider slider)
        {
            if (slider == null)
            {
                return;
            }

            SettingsUIController controller = slider.GetComponentInParent<SettingsUIController>(true);
            if (controller != null)
            {
                slider.onValueChanged.RemoveListener(controller.HandleImmediateChange);
            }
        }

        private static void WireToggle(Toggle toggle)
        {
            if (toggle == null)
            {
                return;
            }

            SettingsUIController controller = toggle.GetComponentInParent<SettingsUIController>(true);
            if (controller != null)
            {
                toggle.onValueChanged.RemoveListener(controller.HandleImmediateChange);
                toggle.onValueChanged.AddListener(controller.HandleImmediateChange);
            }
        }

        private static void UnwireToggle(Toggle toggle)
        {
            if (toggle == null)
            {
                return;
            }

            SettingsUIController controller = toggle.GetComponentInParent<SettingsUIController>(true);
            if (controller != null)
            {
                toggle.onValueChanged.RemoveListener(controller.HandleImmediateChange);
            }
        }

        private static void WireDropdown(TMP_Dropdown dropdown)
        {
            if (dropdown == null)
            {
                return;
            }

            SettingsUIController controller = dropdown.GetComponentInParent<SettingsUIController>(true);
            if (controller != null)
            {
                dropdown.onValueChanged.RemoveListener(controller.HandleImmediateChange);
                dropdown.onValueChanged.AddListener(controller.HandleImmediateChange);
            }
        }

        private static void UnwireDropdown(TMP_Dropdown dropdown)
        {
            if (dropdown == null)
            {
                return;
            }

            SettingsUIController controller = dropdown.GetComponentInParent<SettingsUIController>(true);
            if (controller != null)
            {
                dropdown.onValueChanged.RemoveListener(controller.HandleImmediateChange);
            }
        }

        private static void WireInput(TMP_InputField inputField)
        {
            if (inputField == null)
            {
                return;
            }

            SettingsUIController controller = inputField.GetComponentInParent<SettingsUIController>(true);
            if (controller != null)
            {
                inputField.onEndEdit.RemoveListener(controller.HandleImmediateChange);
                inputField.onEndEdit.AddListener(controller.HandleImmediateChange);
            }
        }

        private static void UnwireInput(TMP_InputField inputField)
        {
            if (inputField == null)
            {
                return;
            }

            SettingsUIController controller = inputField.GetComponentInParent<SettingsUIController>(true);
            if (controller != null)
            {
                inputField.onEndEdit.RemoveListener(controller.HandleImmediateChange);
            }
        }

        private static float GetSliderValue(Slider slider, float fallback)
        {
            return slider != null ? slider.value : fallback;
        }

        private static bool GetToggleValue(Toggle toggle, bool fallback)
        {
            return toggle != null ? toggle.isOn : fallback;
        }

        private static string GetInputText(TMP_InputField inputField, string fallback)
        {
            return inputField != null && !string.IsNullOrWhiteSpace(inputField.text)
                ? inputField.text.Trim()
                : fallback;
        }

        private static void SetSliderValue(Slider slider, float value)
        {
            slider?.SetValueWithoutNotify(Mathf.Clamp01(value));
        }

        private static void SetToggleValue(Toggle toggle, bool value)
        {
            toggle?.SetIsOnWithoutNotify(value);
        }

        private static void SetInputText(TMP_InputField inputField, string value)
        {
            inputField?.SetTextWithoutNotify(value);
        }
    }
}
