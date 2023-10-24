using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SR.UI
{
    public enum CurrencyType
    {
        BNB = 0,
        USDT = 1,
        BUSD = 2,
        Owner = 3
    }

    public class Popup_BuyMoreTurn : UIPopup
    {
        [SerializeField] private TextMeshProUGUI _textPrice;
        [SerializeField] private TextMeshProUGUI _textTotal;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private List<Button> _buttonClose = new();
        [SerializeField] private Button _buttonContinue;
        [SerializeField] private Button _buttonApprove;
        [SerializeField] private Button _buttonCountinueBuy;
        [SerializeField] private TextMeshProUGUI _textApproveMessage;


        [SerializeField] private List<Button> _tokenSelectList;
        [SerializeField] private Image _spriteCurrency;

        [SerializeField] private List<Sprite> _spriteCurrencies = new();
        [SerializeField] private List<string> _stringPrices = new();


        [SerializeField] private TextMeshProUGUI _textButtonContinue;

        [SerializeField] private GameObject _buyContent;
        [SerializeField] private GameObject _approveContent;
        [SerializeField] private GameObject _waitingContent;
        [SerializeField] private GameObject _resultContent;

        // [SerializeField] private Image _resultImage;
        // [SerializeField] private Sprite _successSprite;
        // [SerializeField] private Sprite _failSprite;
        [SerializeField] private TextMeshProUGUI _textResultMessage;
        [SerializeField] private bool _status;

        [SerializeField] private Button _buttonPlus;
        [SerializeField] private Button _buttonMinus;
        [SerializeField] private TMP_InputField _testCount;
        int _buyCount = 10;
        [SerializeField] private int _currentType;
        [SerializeField] private float _currentPrice = 200000f;
        [SerializeField] private bool _isRequestBuyTurn;
        [SerializeField] private TextMeshProUGUI _textOwnerBalance;

        public void Start()
        {
            _buttonCountinueBuy.onClick.AddListener(() =>
            {
                HideAllPanel();
                _waitingContent.SetActive(true);
                Debug.Log("Resend Send Buy");
                ReactInteractor.Instance.Send_BuyTurn(_buyCount, _currentType);
            });
            _buttonBuy.onClick.AddListener(() =>
            {
                //GameplayManager.Instance.PlayAgain();
                BuyProcess();
                //ReactInteractor.Instance.Send_BuyTurn(_buyCount, _currentType);
            });

            _buttonApprove.onClick.AddListener(() =>
            {
                HideAllPanel();
                _waitingContent.SetActive(true);
                ReactInteractor.Instance.Send_Approve();
            });
            _buttonClose.ForEach(button =>
            {
                button.onClick.AddListener(() =>
                {
                    if (!_isRequestBuyTurn)
                    {
                        UIManager.Instance.ShowPopup(PopupName.ResultSucess);
                    }
                    HideAllPanel();
                    Hide();
                });
            });

            _buttonContinue.onClick.AddListener(() =>
            {
                if (_status == true)
                {
                    //if (_isRequestBuyTurn == false)
                        //GameplayManager.Instance.NextGame(true);

                    Hide();
                }
                else
                {
                    Hide();
                    Dictionary<string, object> dict = new Dictionary<string, object> {
                                {"isStartBuy", true},
                                {"isSuccess", false},
                                {"isRequestBuyTurn", false},
                                {"isApproveRequest", false},
                                {"isApprove", false},
                                {"message", ""} };
                    UIManager.Instance.ShowPopup(PopupName.BuyMoreTurn, dict);
                }
            });
            _buttonMinus.onClick.AddListener(() =>
            {
                if (_buyCount > 10)
                    _buyCount -= 10;
                _testCount.text = _buyCount.ToString();
                UpdatePrice();
            });

            _buttonPlus.onClick.AddListener(() =>
            {
                _buyCount += 10;
                _testCount.text = _buyCount.ToString();
                UpdatePrice();
            });

            _tokenSelectList.ForEach((tokenButton) =>
            {
                tokenButton.onClick.AddListener(() =>
                {
                    _tokenSelectList.ForEach(t => t.GetComponent<UITokenSelect>().DeSelect());
                    UITokenSelect tokenSelect = tokenButton.gameObject.GetComponent<UITokenSelect>();
                    tokenSelect.Select();
                    _currentType = (int)tokenSelect.CurrencyType;
                    _spriteCurrency.sprite = _spriteCurrencies[(int)tokenSelect.CurrencyType];
                    _textPrice.text = _stringPrices[(int)tokenSelect.CurrencyType];
                    _currentPrice = tokenSelect.Price;
                    UpdatePrice();
                    if(_currentType == 3) _textOwnerBalance.gameObject.SetActive(true);
                    else 
                        _textOwnerBalance.gameObject.SetActive(false);
                });

            });
            _tokenSelectList[0].onClick.Invoke();

        }

        public void BuyProcess()
        {
            HideAllPanel();
            switch (_currentType)
            {
                case 0:
                    ReactInteractor.Instance.Send_BuyTurn(_buyCount, _currentType);
                    _waitingContent.gameObject.SetActive(true);
                    break;
                case 1:
                    ReactInteractor.Instance.Send_BuyTurn(_buyCount, _currentType);
                    _waitingContent.gameObject.SetActive(true);
                    break;
                case 3:
                    _waitingContent.gameObject.SetActive(true);
                    User.Instance.BuyTurnBuyOwner((ulong)(_buyCount * (int)_currentPrice),
                    () =>
                    {
                        HideAllPanel();
                        _resultContent.SetActive(true);
                        _textResultMessage.text = "Buy Success!";
                        _status = true;
                        _textButtonContinue.text = "Continue";
                    }, (err) =>
                    {
                        HideAllPanel();
                        _resultContent.SetActive(true);
                        _textResultMessage.text = "Not Enough OWNER!";
                        _status = false;
                        _textButtonContinue.text ="Try again";
                    });

                    break;

            }
        }

        public void UpdatePrice()
        {
            float target = _buyCount * _currentPrice;
            if (target % 1 == 0)
            {
                _textTotal.text = target.ToString("N0");
            }
            else
            {
                _textTotal.text = target.ToString("0.#####");
            }

            if(_currentType == 3 ) {
                _textTotal.color = User.Instance.UserData.Owner < _currentPrice*_buyCount? Color.red : Color.white;
            } else
            {
                _textTotal.color = Color.white;
            }
        }

        public override void Show(Dictionary<string, object> customProperties)
        {
            base.Show(customProperties);
            _textOwnerBalance.text = "Your Owner: " + User.Instance.UserData.Owner.ToString("N0") + " Owner";

            bool isSuccess = bool.Parse(customProperties["isSuccess"].ToString());
            bool isStartBuy = bool.Parse(customProperties["isStartBuy"].ToString());

            // bool isRequestBuyTurn = bool.Parse(customProperties["isRequestBuyTurn"].ToString());
            _isRequestBuyTurn = SceneManager.GetActiveScene().buildIndex == 1;

            Debug.Log("IsResset Buy Turn " + _isRequestBuyTurn);
            bool isApproveRequest = bool.Parse(customProperties["isApproveRequest"].ToString());
            bool isApprove = bool.Parse(customProperties["isApprove"].ToString());
            string message = customProperties["message"].ToString();



            if (isStartBuy)
            {
                HideAllPanel();
                _buyContent.SetActive(true);
            }
            else
            {
                if (!isApproveRequest && !isApprove)
                {
                    HideAllPanel();
                    _status = isSuccess;
                    _resultContent.SetActive(true);
                    //_resultImage.sprite = isSuccess ? _successSprite : _failSprite;
                    _textResultMessage.text = isSuccess ? "Buy Success" : message;
                    //_buttonClose.gameObject.SetActive(false);
                    _textButtonContinue.text = isSuccess ? "Continue" : "Try again";
                }


                if (isApproveRequest || isApprove)
                {
                    HideAllPanel();
                    _approveContent.SetActive(true);
                    Debug.Log(message);
                    if (message.Contains("success") || message.Contains("fail"))
                    {

                        if (message.Contains("success"))
                        {
                            _buttonCountinueBuy.gameObject.SetActive(true);
                            _buttonApprove.gameObject.SetActive(false);
                            _textApproveMessage.text = "The approval was a success.";
                        }
                        else
                        {
                            _textApproveMessage.text = "The approval was a fail. Please try Again!";
                            _buttonCountinueBuy.gameObject.SetActive(false);
                            _buttonApprove.gameObject.SetActive(true);
                        }
                    }

                }
            }


        }
        public void HideAllPanel()
        {
            _buyContent.SetActive(false);
            _approveContent.SetActive(false);
            _resultContent.SetActive(false);
            _waitingContent.SetActive(false);
        }
        public override void Hide()
        {
            base.Hide();
            _status = false;
            HideAllPanel();
            _buyContent.SetActive(true);
            //_buttonClose.gameObject.SetActive(true);
            _buttonCountinueBuy.gameObject.SetActive(false);
            _buttonApprove.gameObject.SetActive(true);
            _textApproveMessage.text = "Please approve transaction before you continue transfer.";
            _textOwnerBalance.gameObject.SetActive(false);
        }
    }


}
