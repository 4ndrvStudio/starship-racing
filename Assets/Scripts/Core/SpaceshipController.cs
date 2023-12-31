using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

namespace SR
{
    public class SpaceshipController : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spaceshipsList = new();
        [SerializeField] private CinemachineVirtualCamera _cineCam;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private List<float> targetSpeed;
        [SerializeField] private bool isStart = false;
        [SerializeField] private Transform _followPoint;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _timeAudioClip;

        private int _currentSelect;
        private int _resultRank;
        [SerializeField] private float TimeCal =3;

        private void Setup(int currentSelect)
        {
            _currentSelect = currentSelect;
            _resultRank = GameplayManager.Instance.ResultRank-1;
            _cineCam.Follow = _followPoint;
            TimeCal = 3f;
        }


        // Start is called before the first frame update
        void Start()
        {
            _audioSource.clip = _timeAudioClip;
            _audioSource.Play();
            targetSpeed = new();
            SoundManager.Instance.StopBackground();
            SoundManager.Instance.PlayBackground(EBackgroundType.Gameplay);
            Setup(GameplayManager.Instance.CurrentSelect);
        }

        // Update is called once per frame
        void Update()
        {
            _followPoint.transform.position = new Vector3(0, _spaceshipsList[_currentSelect].position.y, 0f);

            if (TimeCal > 0 && !isStart)
            {
                TimeCal -= Time.deltaTime;
                _timeText.text = TimeCal.ToString("#");
                
            } 

            if(TimeCal<=0 && isStart == false)
            {
                isStart = true;
                _timeText.gameObject.SetActive(false);
                StartRace();
                _audioSource.Stop();
            }
        }

        public void StartRace()
        {

            Debug.Log("Call");
            float speed = 25f;
            targetSpeed.Add(speed);
            float speed2 = Random.Range(speed + 0.1f, speed + 0.3f);
            targetSpeed.Add(speed2);
            targetSpeed.Add(Random.Range(speed2 + 0.1f, speed2 + 0.3f));
            Debug.Log(targetSpeed.Count);
            float targetSpacshipSpeed1 = targetSpeed[_resultRank];
            targetSpeed.RemoveAt(_resultRank);
            _spaceshipsList[_currentSelect].transform.DOMoveY(230f, targetSpacshipSpeed1).SetEase(Ease.InSine);

            for (int i = 0; i< _spaceshipsList.Count; i++)
            {
                if (i != _currentSelect)
                {
                    if(targetSpeed.Count>1)
                    {
                        int index = Random.Range(0,targetSpeed.Count);
                        float targetSpacshipSpeed = targetSpeed[index];
                        _spaceshipsList[i].transform.DOMoveY(230f, targetSpacshipSpeed).SetEase(Ease.InSine);
                        targetSpeed.RemoveAt(index);
                    } else
                    {
                        _spaceshipsList[i].transform.DOMoveY(230f, targetSpeed[0]).SetEase(Ease.InSine);

                    }


                }
            }
            targetSpeed.Clear();

         }
    }
}
