using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Audio
{
    public class AudioAmbArea : MonoBehaviour
    {
        [SerializeField] private bool isFirstArea;
        [Header("Audio Splines")] 
        public List<AudioSplineAmbSounds>  audioSplinesList = new List<AudioSplineAmbSounds>(); 
        [Header("All Random Amb Noises For This Area")]
        public List<string> ambList = new List<string>();
        [Header("All Looping Amb Sounds (E.G WIND)")]
        public List<string> ambLoopingList = new List<string>();
        [Header("All Looping Amb Sounds (E.G WIND)")] [SerializeField]
        private bool changeLoopingAmb = false;
        public List<string> catalystAudioNames = new List<string>();
        [Header("AmbList sounds that require precise locations when played")]
        public List<AudioAmbStaticObj> staticSoundObjList = new List<AudioAmbStaticObj>();

        private void Start()
        {
            if (isFirstArea)
            {
                FirstArea();
                foreach (var var in audioSplinesList)
                {
                    var.inUse = true;
                }
            }
        }

        private void FirstArea()
        {
            AudioAmbStaticManager.instance.audioStaticAmbSounds = staticSoundObjList;
        }
        private void OnTriggerEnter(Collider other)
        {
            Interfaces.Interfaces.IPlayer player = other.transform.GetComponent<Interfaces.Interfaces.IPlayer>();
            if (player != null)
            {
                AudioAmbManager.instance.baseAudioNames = ambList;
                AudioAmbManager.instance.catalystAudioNames = catalystAudioNames;
                AudioAmbStaticManager.instance.audioStaticAmbSounds = staticSoundObjList;

                if (changeLoopingAmb)
                    AudioAmbManager.instance.ChangeLoopingAmb(ambLoopingList);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Interfaces.Interfaces.IPlayer player = other.transform.GetComponent<Interfaces.Interfaces.IPlayer>();
            if (player != null)
            {
                foreach (var var in audioSplinesList) 
                {
                    var.inUse =  false;
                }
            }
        }
    }
}

