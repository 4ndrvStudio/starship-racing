using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SR.UI
{
    public class Popup_RequestBuyTurn : UIPopup
    {
        [SerializeField] private Button _btnBuy;
        [SerializeField] private Button _btnClose;

        public void Start()
        {
            _btnBuy.onClick.AddListener(() =>
            {
                Dictionary<string, object> dict = new Dictionary<string, object> {
                                {"isStartBuy", true},
                                {"isSuccess", false},
                                {"isRequestBuyTurn", true},
                                {"isApproveRequest", false},
                                {"isApprove", false},
                                {"message", ""} };
                UIManager.Instance.ShowPopup(PopupName.BuyMoreTurn, dict);
                Hide();
            });
            _btnClose.onClick.AddListener(() =>
            {
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    UIManager.Instance.ShowPopup(PopupName.ResultSucess);
                }
                Hide();
            });
        }

    }

}
