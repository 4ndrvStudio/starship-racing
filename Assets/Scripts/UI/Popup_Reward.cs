using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SR.UI
{
    public class Popup_Reward : UIPopup
    {
        [SerializeField] private List<Button> _buttonDone = new();

        [SerializeField] private GameObject _itemOb;
        [SerializeField] private GameObject _itemHoder;
        [SerializeField] private TextMeshProUGUI _textAmountOwner1;
        [SerializeField] private TextMeshProUGUI _textAmountOwner2;

        [SerializeField] private GameObject _content1;
        [SerializeField] private GameObject _content2;
        [SerializeField] private GameObject _content3;

        private List<GameObject> _listNftOb = new();

        public  void Start()
        {
                _buttonDone.ForEach(button => {
                button.onClick.AddListener(() => Hide());
            });
        }

        public void HideAllContent()
        {
            _content1.SetActive(false);
            _content2.SetActive(false);
            _content3.SetActive(false);
        }

        public override void Show(Dictionary<string, object> customProperties)
        {
            base.Show(customProperties);
            SoundManager.Instance.PlayOpenBox();
            OpenBagsRespone reward = customProperties["data"] as OpenBagsRespone;
            Reward data = reward.Data;

            //Debug.Log(data.Owner);
            if (data.Nft.Count == 0 && data.Owner == null)
            {
                HideAllContent();
                _content3.SetActive(true);
            }
            else if (data.Nft.Count == 0)
            {
                HideAllContent();
                _content1.SetActive(true);
                _textAmountOwner1.text = data.Owner.ToString();
                _textAmountOwner2.text = data.Owner.ToString();

            }
            else
            {
                HideAllContent();
                _content2.SetActive(true);
                _textAmountOwner1.text = data.Owner.ToString();
                _textAmountOwner2.text = data.Owner == null ? "0 Owner" : data.Owner.ToString();
                _listNftOb.ForEach(ob => Destroy(ob));
                _listNftOb.Clear();

                data.Nft.ForEach(nftUrl => SetupNft(nftUrl));
                _itemHoder.SetActive(true);
            }
        }

        public void SetupNft(string nftLink)
        {
            GameObject nftOb = Instantiate(_itemOb, _itemHoder.transform);
            nftOb.gameObject.SetActive(true);
            nftOb.GetComponent<UINftSlot>().Setup(nftLink);
            _listNftOb.Add(nftOb);

        }
    }

}

