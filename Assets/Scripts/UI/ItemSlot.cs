using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SR.UI
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private Image _imageIcon;
        [SerializeField] private List<Sprite> _listItemSprite = new();
        [SerializeField] private Button _buttonOpen;
        [SerializeField] private BagsRespone _bag;
        [SerializeField] private TextMeshProUGUI _textQuantity;
        [SerializeField] private GameObject _quantityFrame;
        public bool CanOpen = false;
        
        // Start is called before the first frame update
        void Start()
        {
            _buttonOpen.onClick.AddListener(OnOpen);

        }
        
        public void Setup(BagsRespone bag) {
            _imageIcon.sprite = _listItemSprite[bag.Type-1];
            _bag = bag;
            _textQuantity.text = bag.Amount.ToString();
        }
        public void SetupNull() {
            _quantityFrame.gameObject.SetActive(false);
            _textQuantity.gameObject.SetActive(false);
            _imageIcon.sprite = _listItemSprite[3];

        }

        public void OnOpen() {
            if (CanOpen == false) return;

            UIManager.Instance.ShowPopup(PopupName.OpenAnimation, new Dictionary<string, object> {
                 {"BagType", _bag.Type}
             });
            User.Instance.OpenBags(_bag.Type, _bag.Amount, (res) =>
            {
                Dictionary<string, object> dict = new Dictionary<string, object>() {
                     {"data", res}
                 };
                UIManager.Instance.ShowPopup(PopupName.Reward, dict);
            },
            (err) =>
            {
                Debug.Log("open fail");

            });

            UIManager.Instance.HidePopup(PopupName.Inventory);
        }
    }
}
