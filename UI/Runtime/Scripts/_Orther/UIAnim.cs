using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NabaGame.UI
{
    public enum UIAnimType
    {
        // ===== OLD — KHÔNG ĐƯỢC ĐỔI THỨ TỰ =====
        Scale,          // 0
        Pulse,          // 1
        Bounce,         // 2
        PunchScale,     // 3
        Shake,          // 4
        ShakeRotation,  // 5
        Wobble,         // 6
        Swing,          // 7
        Float,          // 8
        Breathing,      // 9
        Heartbeat,      // 10
        Jelly,          // 11
        SquashStretch,  // 12
        Rubber,         // 13
        Squeeze,        // 14
        JellyBounce,    // 15
        Pop,            // 16
        Boing,          // 17
        Impact,         // 18
        Attention,      // 19

        // ===== NEW — CHỈ THÊM Ở CUỐI =====
        Rotate,         // 20
        Spin,           // 21
        Move,           // 22
        MoveX,          // 23
        MoveY,          // 24
        Flame,          // 25
        FlameFlicker    // 26
    }
    public enum UIAnimLoop
    {
        Once,
        Loop,
        PingPong
    }

    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class UIAnim : MonoBehaviour
    {
        [EnumToggleButtons] [SerializeField] private UIAnimType animType = UIAnimType.PunchScale;

        [SerializeField, MinValue(0.01f)] private float duration = 0.5f;
        [SerializeField, MinValue(0f)] private float strength = 1f;
        [SerializeField] private Ease ease = Ease.OutQuad;

        [Title("Playback")] [SerializeField] private bool playOnEnable;
        [SerializeField, MinValue(0f)] private float delay;
        [SerializeField] private UIAnimLoop loop = UIAnimLoop.Once;
        [SerializeField] private bool useUnscaledTime = true;

        [Title("Random")] [SerializeField] private bool randomize;

        [ShowIf(nameof(randomize))] [SerializeField, Range(0f, 1f)]
        private float randomStrength = 0.15f;

#if UNITY_EDITOR
        [Title("Editor Preview")] [SerializeField]
        private bool livePreview;

        [SerializeField] private bool autoResetAfterPreview = true;
#endif

        private RectTransform _rect;
        private Vector3 _initialScale;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private Tween _tween;
        private Sequence _sequence;

#if UNITY_EDITOR
        private bool _refreshQueued;
#endif

        private void Awake() => Init();

        private void OnEnable()
        {
            Init();

            if (!Application.isPlaying)
                return;

            if (playOnEnable)
                Play();
        }

        private void OnDisable()
        {
            Kill();

#if UNITY_EDITOR
            StopEditorPreview();
#endif
        }

        private void OnDestroy()
        {
            Kill();

#if UNITY_EDITOR
            StopEditorPreview();
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Init();

            if (Application.isPlaying)
            {
                QueuePlayModeRefresh();
                return;
            }

            if (!livePreview)
                return;

            EditorApplication.delayCall -= Preview;
            EditorApplication.delayCall += Preview;
        }

        private void QueuePlayModeRefresh()
        {
            if (_refreshQueued)
                return;

            _refreshQueued = true;

            EditorApplication.delayCall -= RefreshPlayMode;
            EditorApplication.delayCall += RefreshPlayMode;
        }

        private void RefreshPlayMode()
        {
            _refreshQueued = false;

            if (this == null || !Application.isPlaying)
                return;

            Kill();
            ResetAnimation();
            CreateAnimation();
            ApplyUpdate();
            RepaintEditor();
        }
#endif

        private void Init()
        {
            if (_rect != null)
                return;

            _rect = transform as RectTransform;

            if (_rect == null)
            {
                Debug.LogError(
                    $"{nameof(UIAnim)} requires RectTransform.",
                    this);

                enabled = false;
                return;
            }

            _initialScale = _rect.localScale;
            _initialPosition = _rect.localPosition;
            _initialRotation = _rect.localRotation;
        }

        // =========================================================
        // CONTROLS
        // =========================================================

        [Title("Controls")]
        [Button("▶ Preview", ButtonSizes.Large)]
        [GUIColor(0.25f, 0.85f, 0.35f)]
        private void Preview()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Play();
                return;
            }

            Init();
            Kill();
            ResetAnimation();
            CreateAnimation();
            ApplyUpdate();
            RepaintEditor();
#endif
        }

        [Button("■ Stop", ButtonSizes.Large)]
        [GUIColor(0.9f, 0.3f, 0.3f)]
        private void StopPreview()
        {
#if UNITY_EDITOR
            StopEditorPreview();
            Kill();

            if (autoResetAfterPreview)
                ResetAnimation();

            RepaintEditor();
#endif
        }

        [Button("↺ Reset", ButtonSizes.Large)]
        private void ResetPreview()
        {
#if UNITY_EDITOR
            StopEditorPreview();
            Kill();
            ResetAnimation();
            RepaintEditor();
#endif
        }

        // =========================================================
        // PUBLIC API
        // =========================================================

        public void Play()
        {
            Init();
            Kill();
            ResetAnimation();
            CreateAnimation();
            ApplyUpdate();
        }

        public void Stop() => Kill();

        public void Restart() => Play();

        public void ResetAnimation()
        {
            if (_rect == null)
                return;

            _rect.localScale = _initialScale;
            _rect.localPosition = _initialPosition;
            _rect.localRotation = _initialRotation;
        }

        public void SetStrength(float value) =>
            strength = Mathf.Max(0f, value);

        public void SetDuration(float value) =>
            duration = Mathf.Max(0.01f, value);

        public void SetLoop(UIAnimLoop value) =>
            loop = value;

        public void SetEase(Ease value) =>
            ease = value;

        // =========================================================
        // CREATE
        // =========================================================

        private void CreateAnimation()
        {
            float p = GetStrength();

            switch (animType)
            {
                case UIAnimType.Scale:
                    PlayScale(p);
                    break;

                case UIAnimType.Pulse:
                    PlayPulse(p);
                    break;

                case UIAnimType.PunchScale:
                    PlayPunchScale(p);
                    break;

                case UIAnimType.Rotate:
                    PlayRotate(p);
                    break;

                case UIAnimType.Spin:
                    PlaySpin(p);
                    break;

                case UIAnimType.Move:
                    PlayMove(p);
                    break;

                case UIAnimType.MoveX:
                    PlayMoveX(p);
                    break;

                case UIAnimType.MoveY:
                    PlayMoveY(p);
                    break;

                case UIAnimType.Bounce:
                    PlayBounce(p);
                    break;

                case UIAnimType.Float:
                    PlayFloat(p);
                    break;

                case UIAnimType.Shake:
                    PlayShake(p);
                    break;

                case UIAnimType.ShakeRotation:
                    PlayShakeRotation(p);
                    break;

                case UIAnimType.Wobble:
                    PlayWobble(p);
                    break;

                case UIAnimType.Swing:
                    PlaySwing(p);
                    break;

                case UIAnimType.Flame:
                    PlayFlame(p);
                    break;

                case UIAnimType.FlameFlicker:
                    PlayFlameFlicker(p);
                    break;

                case UIAnimType.Breathing:
                    PlayBreathing(p);
                    break;

                case UIAnimType.Heartbeat:
                    PlayHeartbeat(p);
                    break;

                case UIAnimType.Jelly:
                    PlayJelly(p);
                    break;

                case UIAnimType.SquashStretch:
                    PlaySquashStretch(p);
                    break;

                case UIAnimType.Rubber:
                    PlayRubber(p);
                    break;

                case UIAnimType.Squeeze:
                    PlaySqueeze(p);
                    break;

                case UIAnimType.JellyBounce:
                    PlayJellyBounce(p);
                    break;

                case UIAnimType.Pop:
                    PlayPop(p);
                    break;

                case UIAnimType.Boing:
                    PlayBoing(p);
                    break;

                case UIAnimType.Impact:
                    PlayImpact(p);
                    break;

                case UIAnimType.Attention:
                    PlayAttention(p);
                    break;
            }

            Tween tween = _sequence ?? _tween;

            if (tween != null)
                ApplyLoop(tween);
        }

        // =========================================================
        // BASIC
        // =========================================================

        private void PlayScale(float p) =>
            _tween = _rect
                .DOScale(_initialScale * (1f + 0.15f * p), duration)
                .SetEase(ease);

        private void PlayPulse(float p) =>
            _tween = _rect
                .DOScale(_initialScale * (1f + 0.08f * p), duration)
                .SetEase(Ease.InOutSine);

        private void PlayPunchScale(float p) =>
            _tween = _rect
                .DOPunchScale(
                    _initialScale * (0.2f * p),
                    duration,
                    6,
                    0.8f)
                .SetEase(ease);

        private void PlayRotate(float p) =>
            _tween = _rect
                .DOLocalRotate(
                    _initialRotation.eulerAngles +
                    Vector3.forward * (30f * p),
                    duration)
                .SetEase(ease);

        private void PlaySpin(float p) =>
            _tween = _rect
                .DOLocalRotate(
                    _initialRotation.eulerAngles +
                    Vector3.forward * (360f * p),
                    duration,
                    RotateMode.FastBeyond360)
                .SetEase(ease);

        private void PlayMove(float p) =>
            _tween = _rect
                .DOLocalMove(
                    _initialPosition +
                    new Vector3(20f, 20f, 0f) * p,
                    duration)
                .SetEase(ease);

        private void PlayMoveX(float p) =>
            _tween = _rect
                .DOLocalMoveX(
                    _initialPosition.x + 30f * p,
                    duration)
                .SetEase(ease);

        private void PlayMoveY(float p) =>
            _tween = _rect
                .DOLocalMoveY(
                    _initialPosition.y + 30f * p,
                    duration)
                .SetEase(ease);

        private void PlayBounce(float p)
        {
            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOLocalMoveY(
                            _initialPosition.y + 30f * p,
                            duration * 0.45f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOLocalMoveY(
                            _initialPosition.y,
                            duration * 0.55f)
                        .SetEase(Ease.OutBounce));
        }

        private void PlayFloat(float p) =>
            _tween = _rect
                .DOLocalMoveY(
                    _initialPosition.y + 15f * p,
                    duration)
                .SetEase(Ease.InOutSine);

        private void PlayShake(float p) =>
            _tween = _rect
                .DOShakePosition(
                    duration,
                    15f * p,
                    20,
                    90f,
                    false,
                    true);

        // =========================================================
        // ROTATION
        // =========================================================

        private void PlayShakeRotation(float p) =>
            _tween = _rect
                .DOShakeRotation(
                    duration,
                    15f * p,
                    20,
                    90f,
                    true);

        private void PlayWobble(float p)
        {
            float angle = 8f * p;

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles +
                            Vector3.forward * angle,
                            duration * 0.25f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles -
                            Vector3.forward * angle,
                            duration * 0.5f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles,
                            duration * 0.25f)
                        .SetEase(Ease.OutQuad));
        }

        private void PlaySwing(float p) =>
            _tween = _rect
                .DOLocalRotate(
                    _initialRotation.eulerAngles +
                    Vector3.forward * (12f * p),
                    duration)
                .SetEase(Ease.InOutSine);

        // =========================================================
        // FLAME
        // =========================================================

        private void PlayFlame(float p)
        {
            float s = 0.1f * p;
            float r = 7f * p;
            float y = 5f * p;

            Vector3 stretch = Scale(
                1f - s,
                1f + s * 1.6f);

            Vector3 squash = Scale(
                1f + s,
                1f - s);

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            stretch,
                            duration * 0.3f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles +
                            Vector3.forward * r,
                            duration * 0.3f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalMoveY(
                            _initialPosition.y + y,
                            duration * 0.3f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOScale(
                            squash,
                            duration * 0.35f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles -
                            Vector3.forward * r,
                            duration * 0.35f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalMoveY(
                            _initialPosition.y - y * 0.3f,
                            duration * 0.35f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.35f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles,
                            duration * 0.35f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalMoveY(
                            _initialPosition.y,
                            duration * 0.35f)
                        .SetEase(Ease.InOutSine));
        }

        private void PlayFlameFlicker(float p)
        {
            float s = 0.08f * p;
            float r = 5f * p;
            float y = 3f * p;

            Vector3 a = Scale(
                1f + s,
                1f - s * 0.5f);

            Vector3 b = Scale(
                1f - s * 0.6f,
                1f + s);

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            a,
                            duration * 0.2f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles +
                            Vector3.forward * r,
                            duration * 0.2f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalMoveY(
                            _initialPosition.y + y,
                            duration * 0.2f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOScale(
                            b,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles -
                            Vector3.forward * r,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalMoveY(
                            _initialPosition.y - y,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine))
                .Join(
                    _rect.DOLocalMoveY(
                            _initialPosition.y,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine));
        }

        // =========================================================
        // ORGANIC
        // =========================================================

        private void PlayBreathing(float p) =>
            _tween = _rect
                .DOScale(
                    Scale(
                        1f + 0.06f * p,
                        1f - 0.025f * p),
                    duration)
                .SetEase(Ease.InOutSine);

        private void PlayHeartbeat(float p)
        {
            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            _initialScale * (1f + 0.12f * p),
                            duration * 0.18f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.12f)
                        .SetEase(Ease.InQuad))
                .AppendInterval(duration * 0.1f)
                .Append(
                    _rect.DOScale(
                            _initialScale * (1f + 0.08f * p),
                            duration * 0.15f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.15f)
                        .SetEase(Ease.InQuad))
                .AppendInterval(duration * 0.3f);
        }

        private void PlayJelly(float p)
        {
            Vector3 squash = Scale(
                1f + 0.2f * p,
                1f - 0.15f * p);

            Vector3 stretch = Scale(
                1f - 0.12f * p,
                1f + 0.12f * p);

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            squash,
                            duration * 0.2f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            stretch,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOScale(
                            _initialScale * (1f + 0.04f * p),
                            duration * 0.2f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.35f)
                        .SetEase(Ease.OutElastic));
        }

        private void PlaySquashStretch(float p)
        {
            Vector3 squash = Scale(
                1f + 0.25f * p,
                1f - 0.2f * p);

            Vector3 stretch = Scale(
                1f - 0.15f * p,
                1f + 0.25f * p);

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            squash,
                            duration * 0.25f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            stretch,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.5f)
                        .SetEase(Ease.OutElastic));
        }

        private void PlayRubber(float p)
        {
            Vector3 stretchX = Scale(
                1f + 0.3f * p,
                1f - 0.15f * p);

            Vector3 stretchY = Scale(
                1f - 0.15f * p,
                1f + 0.3f * p);

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            stretchX,
                            duration * 0.25f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            stretchY,
                            duration * 0.25f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.5f)
                        .SetEase(Ease.OutElastic));
        }

        private void PlaySqueeze(float p) =>
            _tween = _rect
                .DOScale(
                    Scale(
                        1f - 0.25f * p,
                        1f + 0.25f * p),
                    duration)
                .SetEase(Ease.OutElastic);

        private void PlayJellyBounce(float p)
        {
            float jump = 30f * p;

            Vector3 squash = Scale(
                1f + 0.25f * p,
                1f - 0.2f * p);

            Vector3 stretch = Scale(
                1f - 0.12f * p,
                1f + 0.2f * p);

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            squash,
                            duration * 0.15f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOLocalMoveY(
                            _initialPosition.y + jump,
                            duration * 0.25f)
                        .SetEase(Ease.OutQuad))
                .Join(
                    _rect.DOScale(
                            stretch,
                            duration * 0.25f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOLocalMoveY(
                            _initialPosition.y,
                            duration * 0.3f)
                        .SetEase(Ease.InQuad))
                .Append(
                    _rect.DOScale(
                            squash,
                            duration * 0.1f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.2f)
                        .SetEase(Ease.OutElastic));
        }

        // =========================================================
        // IMPACT
        // =========================================================

        private void PlayPop(float p)
        {
            _rect.localScale = Vector3.zero;

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            _initialScale * (1.1f + 0.1f * p),
                            duration * 0.6f)
                        .SetEase(Ease.OutBack))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.4f)
                        .SetEase(Ease.OutElastic));
        }

        private void PlayBoing(float p) =>
            _tween = _rect
                .DOScale(
                    _initialScale * (1f + 0.15f * p),
                    duration)
                .SetEase(Ease.OutElastic);

        private void PlayImpact(float p)
        {
            Vector3 squash = Scale(
                1f + 0.3f * p,
                1f - 0.25f * p);

            Vector3 stretch = Scale(
                1f - 0.15f * p,
                1f + 0.2f * p);

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOScale(
                            squash,
                            duration * 0.2f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            stretch,
                            duration * 0.2f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOScale(
                            _initialScale,
                            duration * 0.6f)
                        .SetEase(Ease.OutElastic));
        }

        private void PlayAttention(float p)
        {
            float angle = 7f * p;

            _sequence = DOTween.Sequence()
                .Append(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles +
                            Vector3.forward * angle,
                            duration * 0.15f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles -
                            Vector3.forward * angle,
                            duration * 0.3f)
                        .SetEase(Ease.InOutSine))
                .Append(
                    _rect.DOLocalRotate(
                            _initialRotation.eulerAngles,
                            duration * 0.15f)
                        .SetEase(Ease.OutQuad))
                .Append(
                    _rect.DOPunchScale(
                        _initialScale * (0.12f * p),
                        duration * 0.4f,
                        5,
                        0.7f));
        }

        // =========================================================
        // HELPERS
        // =========================================================

        private Vector3 Scale(float x, float y) =>
            new Vector3(
                _initialScale.x * x,
                _initialScale.y * y,
                _initialScale.z);

        private float GetStrength() =>
            randomize
                ? strength * Random.Range(
                    1f - randomStrength,
                    1f + randomStrength)
                : strength;

        private void ApplyLoop(Tween tween)
        {
            if (tween == null)
                return;

            if (delay > 0f)
                tween.SetDelay(delay);

            switch (loop)
            {
                case UIAnimLoop.Loop:
                    tween.SetLoops(-1, LoopType.Restart);
                    break;

                case UIAnimLoop.PingPong:
                    tween.SetLoops(-1, LoopType.Yoyo);
                    break;
            }
        }

        private void ApplyUpdate()
        {
            if (!useUnscaledTime)
                return;

            if (_sequence != null)
                _sequence.SetUpdate(true);
            else
                _tween?.SetUpdate(true);
        }

        public void Kill()
        {
            _tween?.Kill();
            _sequence?.Kill();

            _tween = null;
            _sequence = null;
        }

#if UNITY_EDITOR
        private void RepaintEditor()
        {
            SceneView.RepaintAll();
            EditorApplication.QueuePlayerLoopUpdate();
        }

        private void StopEditorPreview()
        {
            EditorApplication.delayCall -= Preview;
            EditorApplication.delayCall -= RefreshPlayMode;
            _refreshQueued = false;
        }
#endif
    }
}