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

        public override void Setup(object data)
        {
            base.Setup(data);
            Dictionary<string, object> dataInput = new Dictionary<string, object>();
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
                });
            } else {
                _textTitle.text = "Fail";
                GameObject itemOb = Instantiate(_itemOb, _itemHolder.transform);
                itemOb.GetComponent<ItemSlot>().SetupNull();
                _itemList.Add(itemOb);
            }

        }

        public void LateUpdate()
        {
            _textYourTurn.text = User.UserData.NumberOfTurns.ToString();
            _textOwner.text = User.UserData.Token.ToString();
        }
    }
}
