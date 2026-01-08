using System.Collections.Generic;
using InputManager;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioSplineAmbSounds : MonoBehaviour
    {
        [SerializeField] private SplineContainer Spline;
        [Header("List of Amb Sounds (E.G Waves)")]
        [SerializeField] private List<string> splineSoundsList;
        public List<Vector3> _bezierKnotsList;

        [Header("Variables")]
        public bool inUse = false;
        [SerializeField] private float maxVolumeDistance;
       
        [Header("Values")]
        [SerializeField] private float minDelay;
        [SerializeField] private float maxDelay;
        
        private void Awake()
        {
            transform.position = Vector3.zero;
        }

        private void Start()
        {
            for(int i = 0; i < Spline.Spline.Count; i++)
            {
                BezierKnot knot = Spline.Spline[i];
                _bezierKnotsList.Add(knot.Position);
            }

            _delay = 1f;
        }

        private float _timer;
        private float _delay;
        private void Update()
        {
            if(!inUse)
                return;
            
            _timer += Time.deltaTime;
            if(_timer >= _delay)
                RandomSplineSound();
        }

        private void RandomSplineSound()
        {
            string chosenAudioEventName;
            //Gets random audio
            if(splineSoundsList.Count <= 1)
                chosenAudioEventName = splineSoundsList[0]; //Prevents game from breaking
            else
                chosenAudioEventName = splineSoundsList[Random.Range(0, splineSoundsList.Count)];

            //SplineUtility.GetNearestPoint(Spline.Spline, tempPlayer.transform.position, out float3 nearest, out float normalisedCurvePos);
            
            // Gets closest knot to the players current position
            float smallestDistance = Mathf.Infinity;
            Vector3 smallestPosition =  Vector3.zero;
   
            foreach (Vector3 pos in _bezierKnotsList)
            {
                float distance = Vector3.Distance(pos, PlayerManager.instance.gameObject.transform.position);
                if (distance <= smallestDistance)
                {
                    smallestDistance = distance;
                    smallestPosition = pos;
                }
            }
            // Sets the volume based of how close the player is to the sound
             var minVolume = 1f; var maxVolume = 1f;
             var minPitch = 0.8f; var maxPitch = 1.2f;
            if (smallestDistance >= maxVolumeDistance * 0.35f)
            {
                minVolume *= 0.15f;
                maxVolume *= 0.35f;
            }
            if (smallestDistance >= maxVolumeDistance * 0.35f && smallestDistance <= maxVolumeDistance * 0.65f)
            {
                minVolume *= 0.35f;
                maxVolume *= 0.75f;
            }
            if (smallestDistance >= maxVolumeDistance * 0.65f)
            {
                minVolume *= 0.75f;
                maxVolume *= 1f;
            }
            AudioManager.instance.PlayFMODSound(smallestPosition, chosenAudioEventName, 3f, true, false, 
                false, true, minVolume, maxVolume, 
                true, minPitch, maxPitch, 
                true, false, null, null);
          
            _delay = Random.Range(minDelay, maxDelay);
            _timer = 0f;
        }
    }
}

