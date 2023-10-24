using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SR.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private Button _btnStart;
        [SerializeField] private Button _btnQuit;
        [SerializeField] private Button _buttonInventory;
        [SerializeField] private Button _buttonCoppyAddress;
        [SerializeField] private TextMeshProUGUI _numOfTurnText;
        [SerializeField] private TextMeshProUGUI _addressText;

        // Start is called before the first frame update
        void Start()
        {
            _buttonCoppyAddress.onClick.AddListener(() => {
                ReactInteractor.Instance.Send_CoppyAddress(User.Instance.UserAddress);
            });
            string address = User.Instance.UserAddress.Substring(0, 7) + "..." + User.Instance.UserAddress.Substring(User.Instance.UserAddress.Length - 6);
            _addressText.text = address;

            _buttonInventory.onClick.AddListener(() => UIManager.Instance.ShowPopup(PopupName.Inventory));

            _btnStart.onClick.AddListener(() => UIManager.Instance.ShowPopup(PopupName.SelectSpaceship));
            _btnQuit.onClick.AddListener(()=>ReactInteractor.Instance.Send_QuitGame());
        }

        // Update is called once per frame
        void LateUpdate()
        {
            _numOfTurnText.text = User.Instance.UserData.NumberOfTurns.ToString();
        }
    }
}
