using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioAmbManager : MonoBehaviour
    {
        public static AudioAmbManager instance;
        
        [Header("Audio Name List")]
        public List<string> currentAmbSoundList = new List<string>(); // Current amb names for the current area
        public List<string> loopingAmbSoundList = new List<string>(); //Current amb Looping names for the current area

        [HideInInspector] public List<AudioSource> loopingAudioPlayersList = new List<AudioSource>(); //Looping background amb audio players
         public List<string> staticSoundsList = new List<string>(); //Changes each area, used for sounds that require precise locations E.G Boat creak
         public List<Vector3> staticSoundsLocations = new List<Vector3>(); //Changes each area, used for sounds that require precise locations E.G Boat creak
        
         [Header("Radius of sound spawning around player")]
        [SerializeField] private float playerRadius;
        [Header("Temp")]
        [SerializeField] private PlayerInterface player;
        [Header("Variables")]
        //Values
        private float _closeAudioPercentage;
        private float _mediumAudioPercentage;
        private float _largeAudioPercentage;
        [UnityEngine.Range(0.25f, 0.75f)] [SerializeField] private float miniumSpawnDistancePercentage;
        //Timer Values
        private float _currentTime;
        private float _completionTime;
        private float _delayTime;
        private float _minTime;
        private float _maxTime;
        private void Awake()
        {
            instance ??= this;
            
            _closeAudioPercentage = playerRadius * 0.30f;
            _mediumAudioPercentage = playerRadius * 0.60f;
            _largeAudioPercentage = playerRadius;

            _minTime = Mathf.Clamp(_minTime, 1, 5);
            _maxTime = Mathf.Clamp(_maxTime, 5, 10);
            _currentTime = 0f;
            summonedNoise = false;
        }

        private void Start()
        {
            foreach (var soundName in loopingAmbSoundList)
                AudioManager.instance.PlayAudio(soundName, transform.position, true, false, false,
                    1, 1, false, 1, 1, 128);
        }

        [HideInInspector] public bool summonedNoise;
        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _completionTime && !summonedNoise)
            {
                summonedNoise = true;
                RandomAmbNoise();
            }
        }
        public void ChangeLoopingAmb(List<string> loopingAmbSound)
        {
            foreach (var var in loopingAudioPlayersList.ToList())
            {
                var.Stop();
                loopingAudioPlayersList.Remove(var);
                foreach (var audioSource in loopingAmbSound)
                    AudioManager.instance.PlayAudio(audioSource, transform.position, true, false, false,
                        1, 1, false, 1, 1, 128);
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        /// ///////////////////////////////////////////////////////////////
        private void RandomAmbNoise()
        {
            string chosenAudioName;
            //Gets random audio
            if(currentAmbSoundList.Count <= 1)
                chosenAudioName = currentAmbSoundList[0]; //Prevents game from breaking
            else
                chosenAudioName = currentAmbSoundList[Random.Range(0, currentAmbSoundList.Count)];
            
            // Locatio setter based of if the audio is static
            Vector3 location;
            if (staticSoundsList.Contains(chosenAudioName))
            {
                var placementInList = staticSoundsList.IndexOf(chosenAudioName);
                location = staticSoundsLocations[placementInList];
            }
            else
            {
                //If the random location is x% of the radius away from the player continue the script
                location = (Random.insideUnitSphere * playerRadius) + player.transform.position;
                if (Vector3.Distance(location, player.transform.position) < playerRadius * miniumSpawnDistancePercentage)
                {
                    RandomAmbNoise();
                    return;
                }
            }
            
            // Prevents audio from going under the floor
            if (location.y < player.transform.position.y)
                location.y = player.transform.position.y;
                
            //Distance checks for volume and priority levels
            var minVolume = 1f; var maxVolume = 1f; var priority = 128;
            var distance = Vector3.Distance(location, player.transform.position);
            if (distance <= _closeAudioPercentage) // Sound is 30% of radius range
            {
                minVolume = 0.85f;
                maxVolume = 1f;
                priority = 128;
            }
            if (distance <= _mediumAudioPercentage && distance >= _closeAudioPercentage) // Sound is 30% - 60% of radius range
            {
                minVolume = 0.50f;
                maxVolume = 0.85f;
                priority = 100;
            }
            if (distance >= _largeAudioPercentage)// Sound is 60%+ of radius range
            {
                minVolume = 0.3f;
                maxVolume = 0.5f;
                priority = 80;
            }
            AudioManager.instance.PlayAudio(chosenAudioName, location,false,true, true
                , minVolume, maxVolume, true, 0.8f, 1.2f, priority);
            bool isCatalyst = !AudioManager.instance.catalystAmbAudio.TryGetValue(chosenAudioName, out var clip);
            
            AmbTimerSetter(AudioManager.instance.soundDataBase[chosenAudioName].length, isCatalyst);
        }
        private void AmbTimerSetter(float audioTime, bool isCatalyst)
        {
            _delayTime = Random.Range(audioTime * 0.15f, audioTime * 0.35f);
            _minTime = audioTime + _delayTime - (audioTime * 0.1f);
            _maxTime = audioTime + _delayTime + (audioTime * 0.25f);
            _completionTime = Random.Range(_minTime, _maxTime);

            if (!isCatalyst)
            {
                _currentTime = 0f;
                summonedNoise = false;
            }
        }
        /// ///////////////////////////////////////////////////////////////////////////////////////////////
        // Foliage Code
        [Header("Foliage Layer")]
        [SerializeField] private LayerMask foliageLayerMask;
        public void FoliageSoundChecker(string soundName, float audioTime)
        {
            //Add if the sound has the authority to do this stuff
            if (!AudioManager.instance.catalystAmbAudio.TryGetValue(soundName, out var clip))
            {
                AmbTimerSetter(audioTime, false);
                return;
            }
                
            
            //If sound is an amb catalyst continue
            List<GameObject> newFoliageList = new List<GameObject>();
            RaycastHit[] hit = Physics.SphereCastAll(player.transform.position, playerRadius, Vector3.down,playerRadius , foliageLayerMask.value);
            foreach (var foliage in hit)
                newFoliageList.Add(foliage.transform.gameObject);

            if (newFoliageList.Count > 0)
                FoliageAmbSounds(newFoliageList);
        }

        private void FoliageAmbSounds(List<GameObject> foliageList)
        {
           //Gets a random foliage from list
            var foliageObj = foliageList[Random.Range(0, foliageList.Count)];
           AudioFoliageReactor audioFoliageReactor = foliageObj.transform.GetComponent<AudioFoliageReactor>();
           
           //Gets random sound from the chosen foliage obj
           List<string> foliageSoundList = audioFoliageReactor.FetchList();
           string randomFoliage = foliageSoundList[Random.Range(0, foliageSoundList.Count)]; 
           
           //Gets volume based of distance
           var minVolume = 1f; var maxVolume = 1f; var priority = 128;
           float distance = Vector3.Distance(foliageObj.transform.position, player.transform.position);
           if (distance <= _closeAudioPercentage) // Sound is 30% of radius range
           {
                minVolume = 0.85f;
                maxVolume = 1f;
                priority = 128;
           }
           if (distance <= _mediumAudioPercentage && distance >= _closeAudioPercentage) // Sound is 30% - 60% of radius range
           {
               minVolume = 0.50f;
               maxVolume = 0.85f;
               priority = 100;
           }
           if (distance >= _largeAudioPercentage)// Sound is 60%+ of radius range
           {
               minVolume = 0.3f;
               maxVolume = 0.5f;
               priority = 80;
           }
           AudioManager.instance.PlayAudio(randomFoliage, foliageObj.transform.position, false,true, false,
               minVolume, maxVolume, true, 0.9f, 1.25f, priority);
           AmbTimerSetter(AudioManager.instance.soundDataBase[randomFoliage].length, false);
        }
    }
}

