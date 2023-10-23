using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using TMPro;

namespace SR
{
    public class SpaceshipController : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spaceshipsList = new();
        [SerializeField] private CinemachineVirtualCamera _cineCam;
        [SerializeField] private TextMeshProUGUI _timeText;
        private bool isStart = false;
        private int _currentSelect;
        private int _resultRank;
        private float TimeCal;

        private void Setup(int currentSelect)
        {
            _currentSelect = currentSelect;
            _resultRank = GameplayManager.Instance.ResultRank-1;
            _cineCam.Follow = _spaceshipsList[currentSelect];
            TimeCal = 3f;
        }


        // Start is called before the first frame update
        void Start()
        {
            Setup(1);
        }

        // Update is called once per frame
        void Update()
        {
            if (TimeCal > 0 && !isStart)
            {
                TimeCal -= Time.deltaTime;
                _timeText.text = TimeCal.ToString("#");
                
            } 

            if(TimeCal<=0 && isStart == false)
            {
                _timeText.gameObject.SetActive(false);
                StartRace();
                isStart = true;
            }
        }

        public void StartRace()
        {


            float speed = 20f;
            List<float> targetSpeed = new(3);
            targetSpeed.Add(speed);
            float speed2 = Random.Range(speed + 0.1f, speed + 1f);
            targetSpeed.Add(speed2);
            targetSpeed.Add(Random.Range(speed2 + 0.1f, speed2 + 1f));

            float targetSpacshipSpeed1 = targetSpeed[_resultRank];
            targetSpeed.RemoveAt(_resultRank);
            _spaceshipsList[_currentSelect].transform.DOMoveY(60f, targetSpacshipSpeed1).SetEase(Ease.InSine);

            for (int i = 0; i< _spaceshipsList.Count; i++)
            {
                if (i != _currentSelect)
                {
                    if(targetSpeed.Count>1)
                    {
                        int index = Random.Range(0,targetSpeed.Count);
                        float targetSpacshipSpeed = targetSpeed[index];
                        _spaceshipsList[i].transform.DOMoveY(60f, targetSpacshipSpeed).SetEase(Ease.InSine);
                        targetSpeed.RemoveAt(index);
                    } else
                    {
                        _spaceshipsList[i].transform.DOMoveY(60f, targetSpeed[0]).SetEase(Ease.InSine);
                    }


                }
            }
         }
    }
}
