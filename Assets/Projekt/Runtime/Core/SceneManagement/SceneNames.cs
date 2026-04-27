/*
 * Datei: SceneNames.cs
 * Zweck: Stellt zentrale Szenennamen für Runtime-Systeme bereit.
 * Verantwortung: Verhindert abweichende harte Scene-Strings in Save-, Auth- und Load-Flows.
 * Abhängigkeiten: Keine.
 * Verwendung: Wird von SaveGameData, SaveSystem und AuthManager als gemeinsame Quelle für Start-/Fallback-Szenen genutzt.
 */

namespace ITAA.Core.SceneManagement
{
    public static class SceneNames
    {
        public const string StartScene = "StartScene";
        public const string GameScene = "GameScene";
        public const string DevelopmentLevel = "DevelopmentLevel";
        public const string LegacyGameScene = GameScene;
    }
}
