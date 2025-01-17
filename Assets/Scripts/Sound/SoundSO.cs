using System;
using UnityEngine;


namespace Blaster.Sound
{
    [CreateAssetMenu(fileName = "SoundScriptableObject", menuName = "ScriptableObjects/SoundScriptableObject")]
    public class SoundSO : ScriptableObject
    {
        public Sounds[] audioList;
    }
    [Serializable]
    public struct Sounds
    {
        public SoundType soundType;
        public AudioClip audio;
    }
}
