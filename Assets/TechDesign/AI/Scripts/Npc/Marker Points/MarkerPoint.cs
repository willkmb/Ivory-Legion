using System;
using Ai;
using UnityEngine;

namespace Npc.Marker_Points
{
    public class MarkerPoint : MonoBehaviour
    {
        [Header("I AM REQUIRED TO BE ON THE NAVMESH PROPERLY TO WORK")]
        [Header("Variables")] 
        [SerializeField] private MarkerType markerType;
        [SerializeField] private MarkerNpcTarget markerNpcTarget;
        [Header("IMPORTANT - I NEED TO BE SET TO CORRECT ZONE")]
        [SerializeField] private MarkerPointZone markerPointZone;
        
        private bool _requiresAction;

        [Header("Replace later with animations etc")]
        [SerializeField] private ParticleSystem testParticle;

        [Header("Values")]
        [SerializeField] private float minMovementCooldownTime;
        [SerializeField] private float maxMovementCooldownTime;
        
        
        private void Start()
        {
            switch(markerNpcTarget)
            {
                case MarkerNpcTarget.Human:
                   markerPointZone.markerPointsHumanActive.Add(gameObject);
                    break;
                case MarkerNpcTarget.Elephant:
                    markerPointZone.markerPointsElephantActive.Add(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            switch (markerType)
            {
                case MarkerType.Base:
                    break;
                case MarkerType.Action:
                    _requiresAction = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
            rend.enabled = false;
        }

        public MarkerType GetMarkerType()
        {
            return markerType;
        }

        public MarkerNpcTarget GetMpNpcTarget()
        {
            return markerNpcTarget;
        }
        public ParticleSystem GetTestPS()
        {
            return testParticle;
        }

        public bool RequiresAction()
        {
            return _requiresAction;
        }

        public float GetMinMovementCooldownTime()
        {
            return minMovementCooldownTime;
        }
        public float GetMaxMovementCooldownTime()
        {
            return maxMovementCooldownTime;
        }
    }
}
