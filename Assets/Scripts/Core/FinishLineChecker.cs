using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using SR.UI;
namespace SR
{
    public class FinishLineChecker : MonoBehaviour
    {
        private bool isComplete = false;
        [SerializeField] private CinemachineVirtualCamera _cine;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (isComplete == false)
            {
                StartCoroutine(ShowWinGame());
                _cine.Follow = this.transform;
                isComplete = true;
            };
        }

        IEnumerator ShowWinGame()
        {
            yield return new WaitForSeconds(2f);
            UIManager.Instance.ShowPopup(PopupName.Result);
        }
    }
}
