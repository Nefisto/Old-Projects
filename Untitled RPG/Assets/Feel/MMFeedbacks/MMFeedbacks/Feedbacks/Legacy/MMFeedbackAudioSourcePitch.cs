﻿using UnityEngine;

namespace MoreMountains.Feedbacks
{
	/// <summary>
	///     This feedback lets you control the pitch of an AudioSource over time
	/// </summary>
	[AddComponentMenu("")]
    [FeedbackPath("Audio/AudioSource Pitch")]
    [FeedbackHelp("This feedback lets you control the pitch of a target AudioSource over time.")]
    public class MMFeedbackAudioSourcePitch : MMFeedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;

        /// sets the inspector color for this feedback
#if UNITY_EDITOR
        public override Color FeedbackColor
        {
            get { return MMFeedbacksInspectorColors.SoundsColor; }
        }
#endif
        /// returns the duration of the feedback
        public override float FeedbackDuration
        {
            get => ApplyTimeMultiplier(Duration);
            set => Duration = value;
        }

        [Header("Pitch Feedback")]
        /// the channel to emit on
        [Tooltip("the channel to emit on")]
        public int Channel;

        /// the duration of the shake, in seconds
        [Tooltip("the duration of the shake, in seconds")]
        public float Duration = 2f;

        /// whether or not to reset shaker values after shake
        [Tooltip("whether or not to reset shaker values after shake")]
        public bool ResetShakerValuesAfterShake = true;

        /// whether or not to reset the target's values after shake
        [Tooltip("whether or not to reset the target's values after shake")]
        public bool ResetTargetValuesAfterShake = true;

        [Header("Pitch")]
        /// whether or not to add to the initial value
        [Tooltip("whether or not to add to the initial value")]
        public bool RelativePitch;

        /// the curve used to animate the intensity value on
        [Tooltip("the curve used to animate the intensity value on")]
        public AnimationCurve PitchTween = new(new Keyframe(0, 1f), new Keyframe(0.5f, 0f), new Keyframe(1, 1f));

        /// the value to remap the curve's 0 to
        [Range(-3f, 3f)]
        [Tooltip("the value to remap the curve's 0 to")]
        public float RemapPitchZero;

        /// the value to remap the curve's 1 to
        [Range(-3f, 3f)]
        [Tooltip("the value to remap the curve's 1 to")]
        public float RemapPitchOne = 1f;

        /// <summary>
        ///     Triggers the corresponding coroutine
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomPlayFeedback (Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
                return;
            var intensityMultiplier = Timing.ConstantIntensity ? 1f : feedbacksIntensity;
            MMAudioSourcePitchShakeEvent.Trigger(PitchTween, FeedbackDuration, RemapPitchZero, RemapPitchOne,
                RelativePitch,
                intensityMultiplier, ChannelData(Channel), ResetShakerValuesAfterShake, ResetTargetValuesAfterShake,
                NormalPlayDirection, Timing.TimescaleMode);
        }

        /// <summary>
        ///     On stop we stop our transition
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomStopFeedback (Vector3 position, float feedbacksIntensity = 1)
        {
            if (!Active || !FeedbackTypeAuthorized)
                return;
            base.CustomStopFeedback(position, feedbacksIntensity);
            MMAudioSourcePitchShakeEvent.Trigger(PitchTween, FeedbackDuration, RemapPitchZero, RemapPitchOne,
                RelativePitch, stop: true);
        }
    }
}