﻿using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
#if MM_URP
using UnityEngine.Rendering.Universal;
#endif

namespace MoreMountains.FeedbacksForThirdParty
{
	/// <summary>
	///     Add this class to a Camera with a URP FilmGrain post processing and it'll be able to "shake" its values by getting
	///     events
	/// </summary>
#if MM_URP
    [RequireComponent(typeof(Volume))]
#endif
    [AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMFilmGrainShaker_URP")]
    public class MMFilmGrainShaker_URP : MMShaker
    {
        [MMInspectorGroup("Film Grain Intensity", true, 51)]
        /// whether or not to add to the initial value
        [Tooltip("whether or not to add to the initial value")]
        public bool RelativeIntensity;

        /// the curve used to animate the intensity value on
        [Tooltip("the curve used to animate the intensity value on")]
        public AnimationCurve ShakeIntensity = new(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));

        /// the value to remap the curve's 0 to
        [Tooltip("the value to remap the curve's 0 to")]
        [Range(0f, 1f)]
        public float RemapIntensityZero;

        /// the value to remap the curve's 1 to
        [Tooltip("the value to remap the curve's 1 to")]
        [Range(0f, 1f)]
        public float RemapIntensityOne = 1f;

#if MM_URP
        protected Volume _volume;
        protected FilmGrain _filmGrain;
        protected float _initialIntensity;
        protected float _originalShakeDuration;
        protected AnimationCurve _originalShakeIntensity;
        protected float _originalRemapIntensityZero;
        protected float _originalRemapIntensityOne;
        protected bool _originalRelativeIntensity;

        /// <summary>
        ///     On init we initialize our values
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            _volume = gameObject.GetComponent<Volume>();
            _volume.profile.TryGet(out _filmGrain);
        }

        /// <summary>
        ///     Shakes values over time
        /// </summary>
        protected override void Shake()
        {
            var newValue = ShakeFloat(ShakeIntensity, RemapIntensityZero, RemapIntensityOne, RelativeIntensity,
                _initialIntensity);
            _filmGrain.intensity.Override(newValue);
        }

        /// <summary>
        ///     Collects initial values on the target
        /// </summary>
        protected override void GrabInitialValues()
        {
            _initialIntensity = _filmGrain.intensity.value;
        }

        /// <summary>
        ///     When we get the appropriate event, we trigger a shake
        /// </summary>
        /// <param name="intensity"></param>
        /// <param name="duration"></param>
        /// <param name="amplitude"></param>
        /// <param name="relativeIntensity"></param>
        /// <param name="attenuation"></param>
        /// <param name="channel"></param>
        public virtual void OnFilmGrainShakeEvent (AnimationCurve intensity, float duration, float remapMin,
            float remapMax, bool relativeIntensity = false,
            float attenuation = 1.0f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true,
            bool resetTargetValuesAfterShake = true,
            bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false,
            bool restore = false)
        {
            if (!CheckEventAllowed(channelData) || (!Interruptible && Shaking))
                return;

            if (stop)
            {
                Stop();
                return;
            }

            if (restore)
            {
                ResetTargetValues();
                return;
            }

            _resetShakerValuesAfterShake = resetShakerValuesAfterShake;
            _resetTargetValuesAfterShake = resetTargetValuesAfterShake;

            if (resetShakerValuesAfterShake)
            {
                _originalShakeDuration = ShakeDuration;
                _originalShakeIntensity = ShakeIntensity;
                _originalRemapIntensityZero = RemapIntensityZero;
                _originalRemapIntensityOne = RemapIntensityOne;
                _originalRelativeIntensity = RelativeIntensity;
            }

            if (!OnlyUseShakerValues)
            {
                TimescaleMode = timescaleMode;
                ShakeDuration = duration;
                ShakeIntensity = intensity;
                RemapIntensityZero = remapMin * attenuation;
                RemapIntensityOne = remapMax * attenuation;
                RelativeIntensity = relativeIntensity;
                ForwardDirection = forwardDirection;
            }

            Play();
        }

        /// <summary>
        ///     Resets the target's values
        /// </summary>
        protected override void ResetTargetValues()
        {
            base.ResetTargetValues();
            _filmGrain.intensity.Override(_initialIntensity);
        }

        /// <summary>
        ///     Resets the shaker's values
        /// </summary>
        protected override void ResetShakerValues()
        {
            base.ResetShakerValues();
            ShakeDuration = _originalShakeDuration;
            ShakeIntensity = _originalShakeIntensity;
            RemapIntensityZero = _originalRemapIntensityZero;
            RemapIntensityOne = _originalRemapIntensityOne;
            RelativeIntensity = _originalRelativeIntensity;
        }

        /// <summary>
        ///     Starts listening for events
        /// </summary>
        public override void StartListening()
        {
            base.StartListening();
            MMFilmGrainShakeEvent_URP.Register(OnFilmGrainShakeEvent);
        }

        /// <summary>
        ///     Stops listening for events
        /// </summary>
        public override void StopListening()
        {
            base.StopListening();
            MMFilmGrainShakeEvent_URP.Unregister(OnFilmGrainShakeEvent);
        }
#endif
    }

	/// <summary>
	///     An event used to trigger FilmGrain shakes
	/// </summary>
	public struct MMFilmGrainShakeEvent_URP
    {
        private static event Delegate OnEvent;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInitialization()
        {
            OnEvent = null;
        }

        public static void Register (Delegate callback)
        {
            OnEvent += callback;
        }

        public static void Unregister (Delegate callback)
        {
            OnEvent -= callback;
        }

        public delegate void Delegate (AnimationCurve intensity, float duration, float remapMin, float remapMax,
            bool relativeIntensity = false,
            float attenuation = 1.0f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true,
            bool resetTargetValuesAfterShake = true,
            bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false,
            bool restore = false);

        public static void Trigger (AnimationCurve intensity, float duration, float remapMin, float remapMax,
            bool relativeIntensity = false,
            float attenuation = 1.0f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true,
            bool resetTargetValuesAfterShake = true,
            bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false,
            bool restore = false)
        {
            OnEvent?.Invoke(intensity, duration, remapMin, remapMax, relativeIntensity, attenuation, channelData,
                resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop,
                restore);
        }
    }
}