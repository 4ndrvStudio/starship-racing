using System.Collections;
using System.Collections.Generic;
using SR.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SR
{
    public class Popup_Result : UIPopup
    {
        [SerializeField] private TextMeshProUGUI _textTitle;
        [SerializeField] private TextMeshProUGUI _textYourTurn;
        [SerializeField] private TextMeshProUGUI _textOwner;
        [SerializeField] private GameObject _itemOb;
        [SerializeField] private GameObject _itemHolder;
        [SerializeField] private Button _btnAgain;
        [SerializeField] private Button _btnCancel;

        [SerializeField] private List<GameObject> _itemList = new();

        private void Start()
        {
            _btnAgain.onClick.AddListener(() => UIManager.Instance.ShowPopup(PopupName.SelectSpaceship));
            _btnCancel.onClick.AddListener(() => {
                GameplayManager.Instance.QuitGameplay();
                Hide();
            });
        }

        public override void Setup(object data)
        {
            base.Setup(data);
            Dictionary<string, object> dataInput = data as Dictionary<string, object>;
            bool status = bool.Parse(dataInput["Status"].ToString());
            StartGameRespone startGameRespone = dataInput["StartGameRespone"] as StartGameRespone;

            _itemList.ForEach(item => Destroy(item));
            _itemList.Clear();
            if (status)
            {
                _textTitle.text = "Win";
                startGameRespone.Data.Bags.ForEach(item =>
                {
                    GameObject itemOb = Instantiate(_itemOb, _itemHolder.transform);
                    itemOb.GetComponent<ItemSlot>().Setup(item);
                    _itemList.Add(itemOb);
                    itemOb.gameObject.SetActive(true);
                });
            } else {
                _textTitle.text = "Fail";
                GameObject itemOb = Instantiate(_itemOb, _itemHolder.transform);
                itemOb.GetComponent<ItemSlot>().SetupNull();
                _itemList.Add(itemOb);
                itemOb.gameObject.SetActive(true);

            }

        }

        public void LateUpdate()
        {
            _textYourTurn.text = User.Instance.UserData.NumberOfTurns.ToString();
            _textOwner.text = User.Instance.UserData.Owner.ToString();
        }
    }
}
