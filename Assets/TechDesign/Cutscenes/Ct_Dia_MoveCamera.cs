using System.Collections.Generic;
using InputManager;
using UnityEngine;

namespace Cutscene
{
    public class CtDiaMoveCamera : MonoBehaviour
    {
        private bool _doOnce;
        public Animation anim;
        private Camera _mainCam;
        [SerializeField] private Camera animCam;
        
        [Header("Replace Objs")]
        [Header("PEOPLE HOLDER -> GOODS HOLDER/FREE BODO")]
        [SerializeField] private List<GameObject> oldObj;
        [SerializeField] private List<GameObject> newObj;
        private void Awake()
        {
            _mainCam =  Camera.main;
            _doOnce = false;
        }
        
        public void MoveCamera()
        {
            if (_doOnce)
                return;
            
            _mainCam.enabled = false;
            anim.Play();
            _doOnce = true;
            PlayerManager.instance.inCutscene = true;
            Invoke("ReplaceObjs", anim.clip.length);
        }

        public void ReturnCamera()
        {
            _mainCam.enabled = true;
            PlayerManager.instance.inCutscene = false;
            anim.Stop();
            CtDiaMoveCamera ctx = this;
            ctx.enabled = false;
        }

        private void ReplaceObjs()
        {
            foreach (GameObject obj in oldObj)
                obj.SetActive(false);
            foreach (GameObject obj in newObj)
                obj.SetActive(false);
        }
    }
}

