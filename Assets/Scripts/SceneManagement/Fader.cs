using System.Collections;
using UnityEngine;

namespace CCC.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private Coroutine _currentActiveFade;
        
        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1f;
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1f, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0f, time);
        }

        public Coroutine Fade(float target, float time)
        {
            if(_currentActiveFade != null)
            {
                StopCoroutine(_currentActiveFade);
            }

           _currentActiveFade =  StartCoroutine(FadeRoutine(target, time));
            return _currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
