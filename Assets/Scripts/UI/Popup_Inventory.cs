using System.Collections;
using System.Collections.Generic;
using SR.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SR
{
    public class Popup_Inventory : UIPopup
    {
        [SerializeField] private GameObject _itemSlotOb;
        [SerializeField] private GameObject _itemHolder;
        [SerializeField] private TextMeshProUGUI _textOwner;
        [SerializeField] private List<GameObject> _itemSpawnedList = new();

        [SerializeField] private Transform _iconWaiting;
        [SerializeField] private Button _buttonBack;
        public void Start()
        {
            _buttonBack.onClick.AddListener(() => Hide());
        }

        public void LateUpdate()
        {
            _textOwner.text = User.Instance.UserData.Owner.ToString("N0") + " Owner";
        }

        public override void Show(Dictionary<string, object> customProperties)
        {
            base.Show(customProperties);
            FectData();
        }

        public override void Hide()
        {
            ClearData();
            base.Hide();
            
        }

        public void FectData()
        {
            _iconWaiting.gameObject.SetActive(true);
            User.Instance.GetUserBags(GetList, GetListError);
            //_textOwner.text = User.UserData.Owner.ToString("N0") + " Owner";

        }
        public void GetList(GetBagsRespone bagsRespone)
        {
            ClearData();
            bagsRespone.Data.ForEach(item =>
            {
                if (item.Amount > 0)
                {
                    GameObject itemSlot = Instantiate(_itemSlotOb, _itemHolder.transform);
                    itemSlot.GetComponent<ItemSlot>().Setup(item);
                    itemSlot.gameObject.SetActive(true);
                    _itemSpawnedList.Add(itemSlot);
                }
            });
            _iconWaiting.gameObject.SetActive(false);
        }

        public void ClearData()
        {
            _itemSpawnedList.ForEach(item => Destroy(item));
            _itemSpawnedList.Clear();
        }

        public void GetListError(GetBagsRespone bagsRespone)
        {
            Debug.Log("GetList Error");
            _iconWaiting.gameObject.SetActive(false);

        }

    }
}
