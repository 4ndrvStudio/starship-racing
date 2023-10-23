using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SR.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private Button _btnStart;
        [SerializeField] private Button _btnQuit;

        // Start is called before the first frame update
        void Start()
        {
            _btnStart.onClick.AddListener(() => UIManager.Instance.ShowPopup(PopupName.SelectSpaceship));
            _btnQuit.onClick.AddListener(()=>ReactInteractor.Instance.Send_QuitGame());
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
