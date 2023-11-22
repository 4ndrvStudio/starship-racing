using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;

namespace SR.UI
{
    public class Popup_MintNftResult : UIPopup
    {
         public Popup_MintNft _popupMintNft;
        private RenderTexture rt;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private VideoPlayer _m_VideoPlayer;


        [SerializeField] private GameObject _failContent;
        [SerializeField] private GameObject _successContent;

        [Header("Fail")]
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private Button _againBtn;

        [Header("Success")]
        [SerializeField] private TextMeshProUGUI _successText;
        [SerializeField] private RawImage _nftIcon;
        [SerializeField] private Button _doneBtn;

        public Nft Nft;

        public void Start()
        {
            _againBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.ShowPopup(PopupName.MintNft);
                Hide();
            });
            _doneBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.ShowPopup(PopupName.Inventory);
                Hide();
            });
        }

        public override void Show(Dictionary<string, object> customProperties = null)
        {
            base.Show(customProperties);

            bool isSuccess = bool.Parse(customProperties["isSuccess"].ToString());
            _failContent.SetActive(!isSuccess);
            _successContent.SetActive(isSuccess);


            if (isSuccess)
            {
                _successText.text = $"You have won an NFT worth <#4BEAEE>${_popupMintNft.Nft.Price}";
                if (_popupMintNft.IsImg)
                {
                    _nftIcon.texture = _popupMintNft.NftIcon.texture;
                }
                else
                {
                    SetupVideo(_m_VideoPlayer.url);
                }

            }
            else
            {
                _errorText.text = "Mint Fail: " + customProperties["message"].ToString();
            }
        }


        private void SetupVideo(string url)
        {

            StartCoroutine(DownloadVideo(url));

        }

        IEnumerator DownloadVideo(string videoUrl)
        {
            rt = new RenderTexture(250, 250, 16, RenderTextureFormat.ARGB32);
            rt.Create();
            _nftIcon.texture = rt;

            rt.Release();
            _videoPlayer.targetTexture = rt;


            using (UnityWebRequest www = UnityWebRequest.Get(videoUrl))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    _videoPlayer.source = VideoSource.Url;
                    _videoPlayer.url = videoUrl;
                    _videoPlayer.Play();
                }
                else
                {
                    Debug.LogError("Failed to download video: " + www.error);
                }
            }
        }
    }
}
