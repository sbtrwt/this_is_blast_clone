using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Sound
{
    public class SoundService 
    {
        private SoundSO soundScriptableObject;
        private AudioSource audioEffects;
        private AudioSource backgroundMusic;
        private AudioSource dialogSound;

        public SoundService(SoundSO soundScriptableObject, AudioSource audioEffectSource, AudioSource bgMusicSource, AudioSource dialogSound)
        {
            this.soundScriptableObject = soundScriptableObject;
            audioEffects = audioEffectSource;
            backgroundMusic = bgMusicSource;
            PlaybackgroundMusic(SoundType.BackgroundMusic, true);
            this.dialogSound = dialogSound;
        }

        public void PlaySoundEffects(SoundType soundType, bool loopSound = false)
        {
            PlaySound(audioEffects, soundType, loopSound);
        }

        private void PlaybackgroundMusic(SoundType soundType, bool loopSound = false)
        {
            PlaySound(backgroundMusic, soundType, loopSound);
        }

        public void PlayDialogSound(SoundType soundType, bool loopSound = false)
        {
            PlaySound(dialogSound, soundType, loopSound);
        }

        private void PlaySound(AudioSource audioSource, SoundType soundType, bool loopSound)
        {
            GameSound gameSound = GetSoundClip(soundType);
            AudioClip clip = null;
            if (gameSound.audio != null)
            {
                clip = gameSound.audio;
                audioSource.volume = gameSound.volume;
            }
            else
                Debug.LogError("No Audio Clip selected.");
            if (clip != null)
            {
                audioSource.loop = loopSound;
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        public void StopDialogSound()
        {
            if(dialogSound.isPlaying)
            dialogSound.Stop();
        }

        private GameSound GetSoundClip(SoundType soundType)
        {
            return Array.Find(soundScriptableObject.audioList, item => item.soundType == soundType);
           
        }
    }

}