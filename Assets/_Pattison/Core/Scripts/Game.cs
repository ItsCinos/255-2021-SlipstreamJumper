﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlipstreamJumper {
    public class Game : MonoBehaviour {

        /// <summary>
        /// A singleton reference.
        /// </summary>
        public static Game main { get; private set; }
        /// <summary>
        /// Whether or not the game is paused.
        /// </summary>
        public static bool isPaused { get; private set; }
        /// <summary>
        /// The next zone to load. This is only used when sele
        /// </summary>
        static public ZoneInfo queuedZone { get; private set; }

        static public void GameOver() {
            if (Game.main == null) return; // the game isn't running...
            Time.timeScale = 1;
            Destroy(Game.main.gameObject);
            SceneManager.LoadScene("GameOver");
        }

        float prePauseTimescale = 1;

        public float timePerZone = 30;
        public float timerUntilWarp { get; private set; }
        private static List<ZoneInfo> _zones = new List<ZoneInfo>() {
            /*
             * TODO: Add student's zone info objects here...
             * Namespace.Zone.info
             */
            //Pattison.Zone.info,
            //Davis.Zone.info,
            Foster.Zone.info,
            Geib.Zone.info,
            Hodgkins.Zone.info,
            Hopkins.Zone.info,
            Howley.Zone.info,
            Jelsomeno.Zone.info,
            Kortge.Zone.info,
            Miller.Zone.info,
            ASmith.Zone.info,
            JSmith.Zone.info,
            Szczesniak.Zone.info,
            Velting.ZoneScript.info,
        };
        public static List<ZoneInfo> zones { get { return _zones; } }
        List<ZoneInfo> zonesUnplayed = new List<ZoneInfo>();
        public ZoneInfo currentZone { get; private set; }


        /// <summary>
        /// This queues a zone to play, and then
        /// it loads the SceneSwitcher scene if it hasn't already been loaded.
        /// </summary>
        /// <param name="zone">The zone to load</param>
        static public void Play(ZoneInfo zone) {
            queuedZone = zone;
            if (main == null) {
                SceneManager.LoadScene("SceneSwitcher"); // this is the scene that uses the Game script ;)
            }
        }

        void Awake() {
            // singleton implementation:
            if (main != null) {
                Destroy(gameObject);
                return;
            }
            main = this;
            DontDestroyOnLoad(gameObject);
            
            // un-pause, if necessary:
            Time.timeScale = 1;
            SetPause(false);
        }
        void Update() {

            if (Input.GetButtonDown("Pause")) TogglePause();
            if (isPaused) return;

            WarpToQueuedZone();

            timerUntilWarp -= Time.unscaledDeltaTime;

            if (timerUntilWarp < 0) WarpRandom();
        }

        private void WarpToQueuedZone() {
            if (queuedZone.sceneFile != null) {
                print($"loading queued level:\"{queuedZone.sceneFile}\"");
                WarpTo(queuedZone);
                queuedZone = new ZoneInfo(); // clear the queue
            }
        }

        public void WarpRandom() {
            if (zonesUnplayed.Count == 0) zonesUnplayed = new List<ZoneInfo>(zones);
            if (zonesUnplayed.Count == 0) return;
            int index = Random.Range(0, zonesUnplayed.Count);
            WarpTo(zonesUnplayed[index]);
        }
        public void WarpTo(ZoneInfo zone) {
            timerUntilWarp = timePerZone;
            SceneManager.LoadScene(zone.sceneFile, LoadSceneMode.Single);
            currentZone = zone;
            RemoveCurrentFromZoneList();
            prePauseTimescale = Time.timeScale = 1;
            print($"warped to \"{currentZone.sceneFile}\" ({zonesUnplayed.Count} left)");
        }
        private void RemoveCurrentFromZoneList() {
            if (zonesUnplayed.Count == 0) zonesUnplayed = new List<ZoneInfo>(zones);
            if (zonesUnplayed.Count == 0) return;
            int index = zonesUnplayed.IndexOf(currentZone);
            zonesUnplayed.RemoveAt(index);

        }
        public void Skip() {
            WarpRandom();
            SetPause(false);
        }
        public void TogglePause() {
            SetPause(null);
        }
        private void SetPause(bool? setTo = null) {

            bool pauseValue = !isPaused;
            if (setTo != null) {
                if (setTo == true) pauseValue = true;
                if (setTo == false) pauseValue = false;
            }
            isPaused = pauseValue;
            if (isPaused) prePauseTimescale = Time.timeScale;
            Time.timeScale = isPaused ? 0 : prePauseTimescale;
        }
        public void BackToMainMenu() {
            prePauseTimescale = Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
            Destroy(gameObject);
        }
    }
}