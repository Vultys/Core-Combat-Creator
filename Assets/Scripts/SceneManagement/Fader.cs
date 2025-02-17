﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1f;
        }

        public IEnumerator FadeOut(float time)
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime / time; 
                yield return null;

            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime / time; 
                yield return null;
            }
        }
    }
}
