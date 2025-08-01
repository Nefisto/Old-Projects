﻿using UnityEngine;

namespace MoreMountains.Feedbacks
{
	/// <summary>
	///     this feedback will "hold", or wait, until all previous feedbacks have been executed, and will then pause the
	///     execution of your MMFeedbacks sequence, for the specified duration
	/// </summary>
	[AddComponentMenu("")]
    [FeedbackHelp(
        "This feedback will 'hold', or wait, until all previous feedbacks have been executed, and will then pause the execution of your MMFeedbacks sequence, for the specified duration.")]
    [FeedbackPath("Pause/Holding Pause")]
    public class MMFeedbackHoldingPause : MMFeedbackPause
    {
        /// sets the color of this feedback in the inspector
#if UNITY_EDITOR
        public override Color FeedbackColor
        {
            get { return MMFeedbacksInspectorColors.HoldingPauseColor; }
        }
#endif
        public override bool HoldingPause => true;

        /// the duration of this feedback is the duration of the pause
        public override float FeedbackDuration
        {
            get => ApplyTimeMultiplier(PauseDuration);
            set => PauseDuration = value;
        }

        /// <summary>
        ///     On custom play we just play our pause
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomPlayFeedback (Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
                return;

            StartCoroutine(PlayPause());
        }
    }
}