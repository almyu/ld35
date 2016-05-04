using JamSuite.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LD35 {

    public class UIManager : MonoSingleton<UIManager> {

        public RectTransform stomachFill;
        public bool stomachVertical;
        public Image timer;
        public GameObject portrait;
        public Graphic sheepIconPrefab;
        public LayoutGroup eatenSheepIcons, lostSheepIcons;

        public Image wolfPortrait;

        public GameObject gameOverWindow;
        public Button restartButton;

        public Color baseSheepColor = Color.white, eatenSheepColor = Color.red, lostSheepColor = Color.grey;

        public Vector2 blinkIntervalRange = new Vector2(0.1f, 2f);

        private float blinkTimer, blinkInterval;

        public GameObject challengeMessage;
        public GameObject failedChallengeIcon;
        public GameObject completedChallengeIcon;
        public float delayBetweenMessages = 1f;
        public float messageLifetime = 1f;
        public AnimationCurve messageAlphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        private CanvasGroup messagesCachedRenderer;

        private void Awake() {
            gameOverWindow.SetActive(false);
            restartButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

            wolfPortrait.canvasRenderer.SetAlpha(0f);
            messagesCachedRenderer = challengeMessage.GetComponent<CanvasGroup>();
            messagesCachedRenderer.alpha = 0f;

            failedChallengeIcon.SetActive(false);
            completedChallengeIcon.SetActive(false);
        }

        protected void Update() {
            if (!messageInProgress && messagesQueue.Count > 0)
                StartCoroutine(DoShowMessage(messagesQueue.Dequeue()));
            
            if (Shepherd.instance.isWolf) {
                return;
            }

            var gmgr = GameManager.instance;
            if (gmgr.canShapeshift) {
                if (blinkTimer < 0f) {
                    blinkTimer = blinkInterval = Mathf.Lerp(blinkIntervalRange.x, blinkIntervalRange.y,
                        gmgr.stomach / gmgr.manualShapeshiftThreshold);
                }
            }
            else blinkTimer = 0f;

            wolfPortrait.canvasRenderer.SetAlpha(blinkTimer / blinkInterval);
            blinkTimer -= Time.unscaledDeltaTime;
        }

        private Action unsub;

        private void OnEnable() {
            unsub = Counters.Subscribe("Eaten", _ => EatSheep());
            unsub += Counters.Subscribe("Lost", _ => LoseSheep());
        }

        private void OnDisable() {
            unsub();
        }

        public void RefreshPortrait() {
            var alpha = Shepherd.instance.isWolf ? 1f : 0f;
            wolfPortrait.canvasRenderer.SetAlpha(alpha);
        }
        
        public static void SetStomach(float stomach) {
            if (instance && instance.stomachFill)
                instance.stomachFill.anchorMax = instance.stomachVertical
                    ? instance.stomachFill.anchorMax.WithY(stomach)
                    : instance.stomachFill.anchorMax.WithX(stomach);
        }

        public static void SetNormalizedTime(float t) {
            if (instance && instance.timer)
                instance.timer.fillAmount = Mathf.Clamp01(1f - t);
        }

        public static void SetShapeshiftAvailable(bool really) {
            if (instance && instance.portrait)
                instance.portrait.SetActive(really);
        }

        public static void SetupSheep(int numSheep) {
            if (!instance || !instance.eatenSheepIcons || !instance.lostSheepIcons) return;

            var eatenXf = instance.eatenSheepIcons.transform;

            for (int i = numSheep; i-- > 0; ) {
                var child = Instantiate(instance.sheepIconPrefab);
                child.color = instance.baseSheepColor;

                var xf = child.transform;
                xf.SetParent(eatenXf, false);
            }
        }

        public static void EatSheep() {
            if (!instance || !instance.eatenSheepIcons) return;

            foreach (Transform child in instance.eatenSheepIcons.transform) {
                var graphic = child.GetComponent<Graphic>();
                if (graphic && graphic.color == instance.baseSheepColor) {
                    graphic.color = instance.eatenSheepColor;
                    return;
                }
            }
        }

        public static void LoseSheep() {
            if (!instance || !instance.eatenSheepIcons || !instance.lostSheepIcons) return;

            var srcGroupXf = instance.eatenSheepIcons.transform;
            var dstGroupXf = instance.lostSheepIcons.transform;

            if (srcGroupXf.childCount > 0) {
                var child = srcGroupXf.GetChild(srcGroupXf.childCount - 1);
                child.SetParent(dstGroupXf, false);

                var graphic = child.GetComponent<Graphic>();
                if (graphic) graphic.color = instance.lostSheepColor;
            }
        }

        public void ShowGameOverWindow() {
            gameOverWindow.SetActive(true);
        }

        private Queue<string> messagesQueue = new Queue<string>();
        private bool messageInProgress = false;

        public void SpawnMessage(string message) {
            messagesQueue.Enqueue(message);
        }

        private IEnumerator DoShowMessage(string message) {
            messageInProgress = true;

            var failed = message.Contains("Failed");
            failedChallengeIcon.SetActive(failed);
            completedChallengeIcon.SetActive(!failed);

            var text = challengeMessage.GetComponentInChildren<Text>();
            text.color = failed ? Color.red : Color.green;
            text.text = message;

            for (var t = 0f; t <= messageLifetime; t += Time.unscaledDeltaTime) {
                messagesCachedRenderer.alpha = messageAlphaCurve.Evaluate(t / messageLifetime);
                yield return null;
            }
            messagesCachedRenderer.alpha = 0f;

            yield return new WaitForSeconds(delayBetweenMessages);
            messageInProgress = false;
        }
    }
}
