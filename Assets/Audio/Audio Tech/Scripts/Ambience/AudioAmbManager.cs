using System;
using System.Collections.Generic;
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
        public List<string> currentAmbSoundList = new List<string>();
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
        }
        
        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _completionTime)
            {
                RandomAmbNoise();
            }
        }
        public void ChangeAmb(AudioClip loopingAmbSound)
        {
            AudioManager.instance.audioSource.clip = loopingAmbSound;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void RandomAmbNoise()
        {
            //Gets random audio
            string chosenAudioName = currentAmbSoundList[Random.Range(0, currentAmbSoundList.Count)];
            var randomLocation = (Random.insideUnitSphere * playerRadius) + player.transform.position;
            
            //If the random location is x% of the radius away from the player continue the script
            if (Vector3.Distance(randomLocation, player.transform.position) < playerRadius * miniumSpawnDistancePercentage)
            {
                RandomAmbNoise();
                return;
            }
            // Prevents audio from going under the floor
            if (randomLocation.y < player.transform.position.y)
                randomLocation.y = player.transform.position.y;
                
            //Distance checks for volume and priority levels
            var minVolume = 1f; var maxVolume = 1f; var priority = 128;
            var distance = Vector3.Distance(randomLocation, player.transform.position);
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
            AudioManager.instance.PlayAudio(chosenAudioName, randomLocation, minVolume, maxVolume, true, 0.8f, 1.2f, priority, true);
            bool isCatalyst = !AudioManager.instance.catalystAmbAudio.TryGetValue(chosenAudioName, out var clip);
            
            AmbTimerSetter(AudioManager.instance.soundDataBase[chosenAudioName].length, isCatalyst);
        }
        private void AmbTimerSetter(float audioTime, bool isCatalyst)
        {
            _delayTime = Random.Range(audioTime * 0.025f, audioTime * 0.15f);
            _minTime = audioTime + _delayTime - (audioTime * 0.1f);
            _maxTime = audioTime + _delayTime + (audioTime * 0.25f);
            _completionTime = Random.Range(_minTime, _maxTime);
            
           //if (!isCatalyst)
              //  _currentTime = 0f;
        }
        /// ///////////////////////////////////////////////////////////////////////////////////////////////
        // Foliage Code
        [Header("Foliage Layer")]
        [SerializeField] private LayerMask foliageLayerMask;
        public void FoliageSoundChecker(string soundName)
        {
            Debug.Log("Checking if catalyst - " + soundName );
            //Add if the sound has the authority to do this stuff
            if (!AudioManager.instance.catalystAmbAudio.TryGetValue(soundName, out var clip))
                return;
            
            Debug.Log("is catalyst");
            //If sound is an amb catalyst continue
            List<GameObject> newFoliageList = new List<GameObject>();
            RaycastHit[] hit = Physics.SphereCastAll(player.transform.position, playerRadius, Vector3.down,playerRadius , foliageLayerMask.value);
            foreach (var foliage in hit)
                newFoliageList.Add(foliage.transform.gameObject);
            
            Debug.Log(newFoliageList.Count);
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
           Debug.Log("foliage played - " + randomFoliage);
           AudioManager.instance.PlayAudio(randomFoliage, foliageObj.transform.position, minVolume, maxVolume, true, 0.9f, 1.25f, priority, false);
           AmbTimerSetter(AudioManager.instance.soundDataBase[randomFoliage].length, false);
        }
    }
}

