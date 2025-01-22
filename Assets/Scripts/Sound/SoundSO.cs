using System;
using UnityEngine;


namespace Blaster.Sound
{
    [CreateAssetMenu(fileName = "SoundScriptableObject", menuName = "ScriptableObjects/SoundScriptableObject")]
    public class SoundSO : ScriptableObject
    {
        public GameSound[] audioList;
    }
    [Serializable]
    public struct GameSound
    {
        public SoundType soundType;
        public AudioClip audio;
        [Range(0, 1)]
        public float volume;
    }
}
