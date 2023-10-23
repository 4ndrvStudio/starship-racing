using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using SR.UI;
using Newtonsoft.Json;
namespace SR
{
    public class BuyTurnResult
    {
        public bool IsSuccess;
        public string Message;
    }


    public class ReactInteractor : MonoBehaviour
    {
        public static ReactInteractor Instance;

        [SerializeField] private GameObject _uiMobile;
        [SerializeField] private GameObject _uiDesktop;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

          //coppy address
        [DllImport("__Internal")]
        private static extern void CoppyAddress(string str);

        //Account 
        public void Send_CoppyAddress(string str)
        {
            Debug.Log("Coppy address: " + str);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                CoppyAddress(str);
#endif
        }

        //Get account
        [DllImport("__Internal")]
        private static extern void GetAccount();

        //Account 
        public void Send_GetAccount()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                GetAccount ();
#endif
        }

        public void Receive_AccountAddress(string address)
        {
            Debug.Log("Unity Connected Account :" + address);
            User.Instance.UserAddress = address;
            GameplayManager.Instance.WalletConnected();
        }
        //Get account
        [DllImport("__Internal")]
        private static extern void QuitGame();

        //Account 
        public void Send_QuitGame()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                QuitGame ();
#endif
        }

        //setmobile 
        public void Receive_DeviceTarget(bool isMobile)
        {
            //GameplayManager.Instance.IsMobile = isMobile;
            // UIInit.Instance.Setbackground(isMobile);

        }


        //Get account
        [DllImport("__Internal")]
        private static extern void BuyTurn(int count, int type);
        //public void buyturn 
        public void Send_BuyTurn(int count, int type)
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                BuyTurn(count,type);
#endif
        }


        public void Receive_BuyTurnResult(string result)
        {
            BuyTurnResult buyResult = JsonConvert.DeserializeObject<BuyTurnResult>(result);
            Debug.Log("buy status : " + buyResult.IsSuccess);
            bool isBuyTarget = buyResult.Message.Contains("Successfully");
            bool isApproveRequest = buyResult.Message.Contains("request aprrove");

            Dictionary<string, object> dict = new Dictionary<string, object> {
                {"isStartBuy", false},
                {"isSuccess", isBuyTarget},
                {"isRequestBuyTurn", false},
                {"isApproveRequest", isApproveRequest},
                {"isApprove", false},
                {"message",  buyResult.Message}
        };

            UIManager.Instance.ShowPopup(PopupName.BuyMoreTurn, dict);

        }

        [DllImport("__Internal")]
        private static extern void Approve();
        //public void buyturn 
        public void Send_Approve()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                Approve();
#endif
        }

        public void Receive_AproveCheckResult(string result)
        {
            BuyTurnResult buyResult = JsonConvert.DeserializeObject<BuyTurnResult>(result);
            bool isBuyTarget = buyResult.Message.Contains("Successfully");
            bool isApproveRequest = buyResult.Message.Contains("request aprrove");
            bool isApprove = buyResult.Message.Contains("The approval was a success.");
            Debug.Log("Aprrove Result " + isApprove);
            Dictionary<string, object> dict = new Dictionary<string, object> {
                {"isStartBuy", false},
                {"isSuccess", false},
                {"isRequestBuyTurn", false},
                {"isApproveRequest", false},
                {"isApprove", true},
                {"message", buyResult.Message}
        };

            UIManager.Instance.ShowPopup(PopupName.BuyMoreTurn, dict);
        }



    }

}
