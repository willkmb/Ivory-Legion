using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using InputManager;
using NUnit.Framework;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioAmbManager : MonoBehaviour
    {
        public static AudioAmbManager instance;
         [Header("Radius of sound spawning around player")]
        [SerializeField] private float playerRadius;
        [Header("Values")]
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

        [Header("FMOD")] 
        public List<string>  loopingAudioEventName;
        public List<EventInstance>  loopingAudioEventInstances;
        public List<string> baseAudioNames = new List<string>(); // 0
        public List<string> catalystAudioNames = new List<string>(); // 1
        public List<string> staticAudioNames = new List<string>(); // 2
        public List<Vector3> staticAudioLocations = new List<Vector3>(); // 2

        private void Awake()
        {
            instance ??= this;
            
            _closeAudioPercentage = (playerRadius * miniumSpawnDistancePercentage) * 0.30f;
            _mediumAudioPercentage = (playerRadius * miniumSpawnDistancePercentage) * 0.60f;
            _largeAudioPercentage = (playerRadius * miniumSpawnDistancePercentage);

            _minTime = Mathf.Clamp(_minTime, 1, 5);
            _maxTime = Mathf.Clamp(_maxTime, 5, 10);
            _currentTime = 0f;
            summonedNoise = false;
        }

        private void Start()
        {
            Invoke("StartAudio", 0.1f);
        }

        private void StartAudio()
        {
            foreach (var soundName in loopingAudioEventName)
                AudioManager.instance.PlayFMODSound(Vector3.zero, soundName, 1f,false, false,false, 0, 0, false, 
                    false, null, null);
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
            foreach (var var in loopingAudioEventInstances.ToList())
            {
                var.stop(STOP_MODE.ALLOWFADEOUT);
                loopingAudioEventInstances.Remove(var);
                foreach (var soundName in loopingAmbSound)
                    AudioManager.instance.PlayFMODSound(Vector3.zero, soundName, 1f,false, false,false, 0, 0, false, 
                        false, null, null);
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        /// ///////////////////////////////////////////////////////////////
        private void RandomAmbNoise()
        {
            bool isCatalyst = false;
            
            string chosenAudioName = "";
            // Fetch FMOD Name Based of Audio Type Randomly Chosen
            int randomSoundList = Random.Range(0, 2);
            if (randomSoundList == 0) // BASE
                chosenAudioName = baseAudioNames[Random.Range(0, baseAudioNames.Count)];
            if (randomSoundList == 1) // CATALYST
            {
                isCatalyst = true;
                chosenAudioName  = catalystAudioNames[Random.Range(0, catalystAudioNames.Count)];
            }
            if (randomSoundList == 2) // STATIC
                chosenAudioName = staticAudioNames[Random.Range(0, staticAudioNames.Count)];
            
            Vector3 location;
            //If the random location is x% of the radius away from the player continue the script
            location = (Random.insideUnitSphere * playerRadius) + PlayerManager.instance.gameObject.transform.position;
            if (Vector3.Distance(location,  PlayerManager.instance.gameObject.transform.position) < playerRadius * miniumSpawnDistancePercentage)
            {
                RandomAmbNoise();
                return;
            }
            
            // Prevents audio from going under the floor -> change to a sphere cast so it scales well with verticality being added
            if (location.y <  PlayerManager.instance.gameObject.transform.position.y)
                location.y =  PlayerManager.instance.gameObject.transform.position.y;
                
            //Distance checks for volume and priority levels
            var minVolume = 1f; var maxVolume = 1f; var priority = 128;
            var distance = Vector3.Distance(location, PlayerManager.instance.gameObject.transform.position);
            if (distance <= _closeAudioPercentage) // Sound is 30% of radius range
            {
                minVolume = 0.3f;
                maxVolume = 0.2f;
            }
            if (distance <= _mediumAudioPercentage && distance >= _closeAudioPercentage) // Sound is 30% - 60% of radius range
            {
                minVolume = 0.1f;
                maxVolume = 0.2f;
            }
            if (distance >= _largeAudioPercentage)// Sound is 60%+ of radius range
            {
                minVolume = 0.05f;
                maxVolume = 0.1f;
            }
            
            // Plays FMOD audio
            AudioManager.instance.PlayFMODSound(location, chosenAudioName, 1f,true,false,false, minVolume, maxVolume, false, 
                false, null, null);
            
            if (isCatalyst)
                FoliageSoundChecker(location);
            
            AmbTimerSetter(2.5f, isCatalyst);
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
        private void FoliageSoundChecker(Vector3 location) // Might use this location later
        {
            List<GameObject> newFoliageList = new List<GameObject>();
            RaycastHit[] hit = Physics.SphereCastAll( PlayerManager.instance.gameObject.transform.position, playerRadius, Vector3.down,playerRadius , foliageLayerMask.value);
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
           string randomFoliageEvent = audioFoliageReactor.FetchList(); 
           
           //Gets volume based of distance
           var minVolume = 1f; var maxVolume = 1f; 
           float distance = Vector3.Distance(foliageObj.transform.position, PlayerManager.instance.gameObject.transform.position);
           if (distance <= _closeAudioPercentage) // Sound is 30% of radius range
           {
                minVolume = 0.85f;
                maxVolume = 1f;
           }
           if (distance <= _mediumAudioPercentage && distance >= _closeAudioPercentage) // Sound is 30% - 60% of radius range
           {
               minVolume = 0.50f;
               maxVolume = 0.85f;
           }
           if (distance >= _largeAudioPercentage)// Sound is 60%+ of radius range
           {
               minVolume = 0.3f;
               maxVolume = 0.5f;
           }
            
           AudioManager.instance.PlayFMODSound(audioFoliageReactor.transform.position, randomFoliageEvent, 1f,true,false,true, minVolume, maxVolume, true, 
               false, null, null); 
           
           AmbTimerSetter(1.5f, false);
        }
        
        //////////////////////////////////////////////////////////////////////////////////
        // Old Code Storage
                
        // [Header("Audio Name List")]
        // public List<string> currentAmbSoundList = new List<string>(); // Current amb names for the current area
        // public List<string> loopingAmbSoundList = new List<string>(); //Current amb Looping names for the current area
        //
        // [HideInInspector] public List<AudioSource> loopingAudioPlayersList = new List<AudioSource>(); //Looping background amb audio players
        // public List<string> staticSoundsList = new List<string>(); //Changes each area, used for sounds that require precise locations E.G Boat creak
        // public List<Vector3> staticSoundsLocations = new List<Vector3>(); //Changes each area, used for sounds that require precise locations E.G Boat creak 
        
        
        //Gets random audio
        // if(currentAmbSoundList.Count <= 1)
        //     chosenAudioName = currentAmbSoundList[0]; //Prevents game from breaking
        //     else
        // chosenAudioName = currentAmbSoundList[Random.Range(0, currentAmbSoundList.Count)];
        
        // Location setter based of if the audio is static
        // if (staticSoundsList.Contains(chosenAudioName))
        // {
        //     var placementInList = staticSoundsList.IndexOf(chosenAudioName);
        //     location = staticSoundsLocations[placementInList];
        // }
        
        // Play Audio
        // AudioManager.instance.PlayAudio(chosenAudioName, location,false,true, true
        // , minVolume, maxVolume, true, 0.8f, 1.2f, priority);
        // bool isCatalyst = !AudioManager.instance.catalystAmbAudio.TryGetValue(chosenAudioName, out var clip);
        
        // Folliage
        //Add if the sound has the authority to do this stuff
        // if (!AudioManager.instance.catalystAmbAudio.TryGetValue(soundName, out var clip))
        // {
        //     AmbTimerSetter(audioTime, false);
        //     return;
        // }
    }
}

