/*
 * Datei: SaveGameData.cs
 * Zweck:
 * - Speichert die serialisierbaren Daten eines Spielstands.
 * - Wird vom SaveSystem als JSON gespeichert.
 * - Dient als Grundlage für Restore (Player, Scene, Progress).
 */

using System;
using UnityEngine;

namespace ITAA.System.Savegame
{
    [Serializable]
    public class SaveGameData
    {
        #region Core Data

        public int SlotId;
        public string DisplayName;
        public string PlayerName;
        public string SceneName;
        public string SavedAtText;

        #endregion

        #region Player State

        public float PlayerPosX;
        public float PlayerPosY;
        public float PlayerPosZ;

        #endregion

        #region Progress

        public int Level;
        public int Score;
        public bool HasData = true;

        #endregion

        #region Public API

        public Vector3 GetPlayerPosition()
        {
            return new Vector3(PlayerPosX, PlayerPosY, PlayerPosZ);
        }

        public void SetPlayerPosition(Vector3 position)
        {
            PlayerPosX = position.x;
            PlayerPosY = position.y;
            PlayerPosZ = position.z;
        }

        #endregion

        #region Factory (optional, aber sehr praktisch)

        public static SaveGameData CreateDummy()
        {
            SaveGameData dummy = new SaveGameData
            {
                SlotId = 1,
                DisplayName = "Testslot Arthur",
                PlayerName = "Martin",
                SceneName = "GameScene",
                SavedAtText = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                Level = 3,
                Score = 1200,
                HasData = true
            };

            dummy.SetPlayerPosition(new Vector3(2f, 1f, 0f));

            return dummy;
        }

        #endregion
    }
}