using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private float holdTime = 0.4f;
        [SerializeField] private float refreshRate = 0.1f;
        [SerializeField] private Slider slider;

        public Action OnFinish;
        private Coroutine _progress;
        private float _currentProgress;

        private void Start() => ResetProgress();

        /// <summary>
        /// Pauses the progress bar charging
        /// </summary>
        public void PauseProgress()
        {
            if (_progress != null) StopCoroutine(_progress);
        }
        
        /// <summary>
        /// Hide the progress bar and reset its value.
        /// </summary>
        public void ResetProgress()
        {
            PauseProgress();
            
            slider.gameObject.SetActive(false);
            slider.value = 0;
            _currentProgress = 0;
        }

        /// <summary>
        /// Start charging the progress bar.
        /// </summary>
        public void StartProgress()
        {
            slider.gameObject.SetActive(true);
            _progress = StartCoroutine(Progress());
        }

        private IEnumerator Progress()
        {
            while (_currentProgress < holdTime)
            {
                _currentProgress += refreshRate;
                slider.value = Mathf.Min(_currentProgress / holdTime, 1);
                
                yield return new WaitForSeconds(refreshRate);
            }
            
            OnFinish?.Invoke();
            ResetProgress();
        }
    }
}
