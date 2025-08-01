﻿using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	/// <summary>
	///     This feedback will let you change the speed of a target animator, either once, or instantly and then reset it, or
	///     interpolate it over time
	/// </summary>
	[AddComponentMenu("")]
    [FeedbackHelp(
        "This feedback will let you change the speed of a target animator, either once, or instantly and then reset it, or interpolate it over time")]
    [FeedbackPath("GameObject/Animator Speed")]
    public class MMFeedbackAnimatorSpeed : MMFeedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;

        /// sets the inspector color for this feedback
#if UNITY_EDITOR
        public override Color FeedbackColor
        {
            get { return MMFeedbacksInspectorColors.GameObjectColor; }
        }
#endif

        public enum Modes
        {
            Once,
            InstantThenReset,
            OverTime
        }

        public virtual float GetTime() => Timing.TimescaleMode == TimescaleModes.Scaled ? Time.time : Time.unscaledTime;

        public virtual float GetDeltaTime()
            => Timing.TimescaleMode == TimescaleModes.Scaled ? Time.deltaTime : Time.unscaledDeltaTime;

        [Header("Animation")]
        /// the animator whose parameters you want to update
        [Tooltip("the animator whose parameters you want to update")]
        public Animator BoundAnimator;

        [Header("Speed")]
        /// whether to change the speed of the target animator once, instantly and reset it later, or have it change over time
        [Tooltip(
            "whether to change the speed of the target animator once, instantly and reset it later, or have it change over time")]
        public Modes Mode = Modes.Once;

        /// the new minimum speed at which to set the animator - value will be randomized between min and max
        [Tooltip("the new minimum speed at which to set the animator - value will be randomized between min and max")]
        public float NewSpeedMin;

        /// the new maximum speed at which to set the animator - value will be randomized between min and max
        [Tooltip("the new maximum speed at which to set the animator - value will be randomized between min and max")]
        public float NewSpeedMax;

        /// when in instant then reset or over time modes, the duration of the effect
        [Tooltip("when in instant then reset or over time modes, the duration of the effect")]
        [MMFEnumCondition("Mode", (int)Modes.InstantThenReset, (int)Modes.OverTime)]
        public float Duration = 1f;

        /// when in over time mode, the curve against which to evaluate the new speed
        [Tooltip("when in over time mode, the curve against which to evaluate the new speed")]
        [MMFEnumCondition("Mode", (int)Modes.OverTime)]
        public AnimationCurve Curve = new(new Keyframe(0, 0f), new Keyframe(0.5f, 1f), new Keyframe(1, 0f));

        protected Coroutine _coroutine;
        protected float _initialSpeed;
        protected float _startedAt;

        /// <summary>
        ///     On Play, checks if an animator is bound and triggers parameters
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomPlayFeedback (Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
                return;

            if (BoundAnimator == null)
            {
                Debug.LogWarning("No animator was set for " + name);
                return;
            }

            if (!IsPlaying)
                _initialSpeed = BoundAnimator.speed;

            if (Mode == Modes.Once)
                BoundAnimator.speed = DetermineNewSpeed();
            else
                _coroutine = StartCoroutine(ChangeSpeedCo());
        }

        /// <summary>
        ///     A coroutine used in ForDuration mode
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator ChangeSpeedCo()
        {
            if (Mode == Modes.InstantThenReset)
            {
                IsPlaying = true;
                BoundAnimator.speed = DetermineNewSpeed();
                yield return MMCoroutine.WaitFor(Duration);
                BoundAnimator.speed = _initialSpeed;
                IsPlaying = false;
            }
            else if (Mode == Modes.OverTime)
            {
                IsPlaying = true;
                _startedAt = GetTime();
                var newTargetSpeed = DetermineNewSpeed();
                while (GetTime() - _startedAt < Duration)
                {
                    var time = MMFeedbacksHelpers.Remap(GetTime() - _startedAt, 0f, Duration, 0f, 1f);
                    var t = Curve.Evaluate(time);
                    BoundAnimator.speed = Mathf.Max(0f,
                        MMFeedbacksHelpers.Remap(t, 0f, 1f, _initialSpeed, newTargetSpeed));
                    yield return null;
                }

                BoundAnimator.speed = _initialSpeed;
                IsPlaying = false;
            }
        }

        /// <summary>
        ///     Determines the new speed for the target animator
        /// </summary>
        /// <returns></returns>
        protected virtual float DetermineNewSpeed() => Mathf.Abs(Random.Range(NewSpeedMin, NewSpeedMax));

        /// <summary>
        ///     On stop, turns the bool parameter to false
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomStopFeedback (Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            BoundAnimator.speed = _initialSpeed;
        }
    }
}