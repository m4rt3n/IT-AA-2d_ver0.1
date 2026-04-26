/*
 * Datei: StartSceneFeatureBootstrap.cs
 * Zweck: Bietet eine kompatible StartScene-Fassade fuer den zentralen GameSystemsBootstrap.
 * Verantwortung: Leitet manuelle StartScene-Initialisierung an GameSystemsBootstrap weiter, ohne eigene Runtime-Objekte zu erzeugen.
 * Abhaengigkeiten: GameSystemsBootstrap, Unity SceneManagement.
 * Verwendung: Bestehende Aufrufer koennen EnsureForScene weiter nutzen; automatische Initialisierung erfolgt ueber GameSystemsBootstrap.
 */

using UnityEngine.SceneManagement;

namespace ITAA.Core.SceneManagement
{
    public static class StartSceneFeatureBootstrap
    {
        public static void EnsureForScene(Scene scene)
        {
            GameSystemsBootstrap.EnsureForScene(scene);
        }
    }
}
