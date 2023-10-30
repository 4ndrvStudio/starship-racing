using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public enum EBackgroundType
    {
        Menu,
        Gameplay,
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [SerializeField] private AudioSource _audioSourceBackground;
        [SerializeField] private AudioSource _audioSourceOnetime;
        [SerializeField] private AudioClip _soundMenuBackground;
        [SerializeField] private AudioClip _soundGameplayBackground;
        [SerializeField] private AudioClip _soundWining;
        [SerializeField] private AudioClip _soundFail;
        [SerializeField] private AudioClip _soundSelect;
        [SerializeField] private AudioClip _soundOpenBox;
        [SerializeField] private AudioClip _soundClick;

        void Awake()
        {
            if (Instance == null) Instance = this;

        }

        public void PlayButtonClick() => _audioSourceOnetime.PlayOneShot(_soundClick);

        public void PlayOneTime(AudioClip audioClip)
        {
            _audioSourceOnetime.PlayOneShot(audioClip);
        }

        public void StopBackground()
        {
            _audioSourceBackground.Stop();
        }

        public void PlayBackground(EBackgroundType backgroundType)
        {

            switch (backgroundType)
            {
                case EBackgroundType.Menu:
                    _audioSourceBackground.clip = _soundMenuBackground;

                    _audioSourceBackground.Play();
                    break;
                case EBackgroundType.Gameplay:
                    _audioSourceBackground.clip = _soundGameplayBackground;

                    _audioSourceBackground.Play();
                    break;
            }
        }

        public void PlayWinning() => _audioSourceOnetime.PlayOneShot(_soundWining);
        public void PlayFail() => _audioSourceOnetime.PlayOneShot(_soundFail);
        public void PlaySelect() => _audioSourceOnetime.PlayOneShot(_soundSelect);
        public void PlayOpenBox() => _audioSourceOnetime.PlayOneShot(_soundOpenBox);
    }
}
