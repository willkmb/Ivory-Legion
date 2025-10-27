using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Audio
{
    public class AudioAmbArea : MonoBehaviour
    {
        [SerializeField] private bool isFirstArea;
        
        [Header("All Random Amb Noises For This Area")]
        public List<string> ambList = new List<string>();
        [Header("All Looping Amb Sounds (E.G WIND)")]
        public List<string> ambLoopingList = new List<string>();
        [Header("AmbList sounds that require precise locations when played")]
        public List<string> staticSoundsList = new List<string>();
        [Header("Locations for those static sounds")]
        public List<Vector3> staticLocationsList = new List<Vector3>();
        [Header("Locations for those static sounds")]
        public List<AudioSplineAmbSounds> splineAmbAudioList = new List<AudioSplineAmbSounds>();

        private void Start()
        {
            if (isFirstArea)
                FirstArea();
        }

        private void FirstArea()
        {
            AudioAmbManager.instance.staticSoundsList =  staticSoundsList;
            AudioAmbManager.instance.staticSoundsLocations = staticLocationsList;
        }
        private void OnTriggerEnter(Collider other)
        {
            Interfaces.Interfaces.IPlayer player = other.transform.GetComponent<Interfaces.Interfaces.IPlayer>();
            if (player != null)
            {
                AudioAmbManager.instance.ChangeLoopingAmb(ambLoopingList);
                AudioAmbManager.instance.currentAmbSoundList = ambList;
                AudioAmbManager.instance.staticSoundsList =  staticSoundsList;
                AudioAmbManager.instance.staticSoundsLocations = staticLocationsList;
            }
        }
        
    }
}

