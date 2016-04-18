using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace LD35 {

    public class UIManager : MonoSingleton<UIManager> {

        public RectTransform stomachFill;
        public bool stomachVertical;
        public UICircle timer;
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

        private CanvasGroup messagesCachedRenderer;
        private AnimationCurve alphaCurve;
        private float duration;

        private void Awake() {
            gameOverWindow.SetActive(false);
            restartButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

            wolfPortrait.canvasRenderer.SetAlpha(0f);
            messagesCachedRenderer = challengeMessage.GetComponent<CanvasGroup>();
            messagesCachedRenderer.alpha = 0f;
            alphaCurve = UIFlash.instance.alphaCurve;
            duration = UIFlash.instance.duration;

            failedChallengeIcon.SetActive(false);
            completedChallengeIcon.SetActive(false);
        }

        protected void Update() {
            if (!messageInProgress && messagesQueue.Count > 0) {
                messagesQueue.Dequeue()();
            }
            
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
            if (instance && instance.timer) {
                instance.timer.fillPercent = Mathf.RoundToInt((1f - t) * 100f);
                instance.timer.SetVerticesDirty();
            }
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

        private Queue<Action> messagesQueue = new Queue<Action>();
        private bool messageInProgress = false;
        public void SpawnMessage(string text) {
            var msgText = challengeMessage.GetComponentInChildren<Text>();
            var txt = text;

            messagesQueue.Enqueue(() => StartCoroutine(ShowMessage(msgText, txt)));            
        }

        private IEnumerator ShowMessage(Text messageText, string msgText) {
            messageInProgress = true;
            messageText.text = msgText;

            var failed = msgText.Contains("Failed");
            failedChallengeIcon.SetActive(failed);
            completedChallengeIcon.SetActive(!failed);
            
            var elapsed = duration * delayBetweenMessages;
            messagesCachedRenderer.alpha = 0f;

            while (elapsed >= 0f) {

                messagesCachedRenderer.alpha = alphaCurve.Evaluate(1f - Mathf.Clamp01(elapsed / duration));

                elapsed -= Time.unscaledDeltaTime;

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(delayBetweenMessages);

            messagesCachedRenderer.alpha = 0f;
            

            messageInProgress = false;
        }
    }
}
