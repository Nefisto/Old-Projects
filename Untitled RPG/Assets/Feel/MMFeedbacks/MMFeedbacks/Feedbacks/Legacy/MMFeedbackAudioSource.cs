﻿using UnityEngine;
using UnityEngine.Audio;

namespace MoreMountains.Feedbacks
{
    [AddComponentMenu("")]
    [FeedbackPath("Audio/AudioSource")]
    [FeedbackHelp("This feedback lets you play a target audio source, with some elements at random.")]
    public class MMFeedbackAudioSource : MMFeedback
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

        /// the possible ways to interact with the audiosource
        public enum Modes
        {
            Play,
            Pause,
            UnPause,
            Stop
        }

        [Header("AudioSource")]
        /// the target audio source to play
        [Tooltip("the target audio source to play")]
        public AudioSource TargetAudioSource;

        /// whether we should play the audio source or stop it or pause it
        [Tooltip("whether we should play the audio source or stop it or pause it")]
        public Modes Mode = Modes.Play;

        [Header("Random Sound")]
        /// an array to pick a random sfx from
        [Tooltip("an array to pick a random sfx from")]
        public AudioClip[] RandomSfx;

        [Header("Volume")]
        /// the minimum volume to play the sound at
        [Tooltip("the minimum volume to play the sound at")]
        public float MinVolume = 1f;

        /// the maximum volume to play the sound at
        [Tooltip("the maximum volume to play the sound at")]
        public float MaxVolume = 1f;

        [Header("Pitch")]
        /// the minimum pitch to play the sound at
        [Tooltip("the minimum pitch to play the sound at")]
        public float MinPitch = 1f;

        /// the maximum pitch to play the sound at
        [Tooltip("the maximum pitch to play the sound at")]
        public float MaxPitch = 1f;

        [Header("Mixer")]
        /// the audiomixer to play the sound with (optional)
        [Tooltip("the audiomixer to play the sound with (optional)")]
        public AudioMixerGroup SfxAudioMixerGroup;

        /// the duration of this feedback is the duration of the clip being played
        public override float FeedbackDuration
        {
            get => _duration;
            set => _duration = value;
        }

        protected AudioClip _randomClip;
        protected float _duration;

        /// <summary>
        ///     Custom init to cache the audiosource if required
        /// </summary>
        /// <param name="owner"></param>
        protected override void CustomInitialization (GameObject owner)
        {
            base.CustomInitialization(owner);
        }

        /// <summary>
        ///     Plays either a random sound or the specified sfx
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomPlayFeedback (Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
                return;

            var intensityMultiplier = Timing.ConstantIntensity ? 1f : feedbacksIntensity;
            switch (Mode)
            {
                case Modes.Play:
                    if (RandomSfx.Length > 0)
                    {
                        _randomClip = RandomSfx[Random.Range(0, RandomSfx.Length)];
                        TargetAudioSource.clip = _randomClip;
                    }

                    var volume = Random.Range(MinVolume, MaxVolume) * intensityMultiplier;
                    var pitch = Random.Range(MinPitch, MaxPitch);
                    _duration = TargetAudioSource.clip.length;
                    PlayAudioSource(TargetAudioSource, volume, pitch);
                    break;

                case Modes.Pause:
                    _duration = 0.1f;
                    TargetAudioSource.Pause();
                    break;

                case Modes.UnPause:
                    _duration = 0.1f;
                    TargetAudioSource.UnPause();
                    break;

                case Modes.Stop:
                    _duration = 0.1f;
                    TargetAudioSource.Stop();
                    break;
            }
        }

        /// <summary>
        ///     Plays the audiosource at the selected volume and pitch
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        protected virtual void PlayAudioSource (AudioSource audioSource, float volume, float pitch)
        {
            // we set the audio source volume to the one in parameters
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.timeSamples = 0;

            if (!NormalPlayDirection)
            {
                audioSource.pitch = -1;
                audioSource.timeSamples = audioSource.clip.samples - 1;
            }

            // we start playing the sound
            audioSource.Play();
        }

        /// <summary>
        ///     Stops the audiosource from playing
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        public override void Stop (Vector3 position, float feedbacksIntensity = 1.0f)
        {
            base.Stop(position, feedbacksIntensity);
            if (TargetAudioSource != null)
                TargetAudioSource?.Stop();
        }
    }
}