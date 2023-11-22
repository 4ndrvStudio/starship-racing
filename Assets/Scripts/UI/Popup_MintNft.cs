using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
using TMPro;

namespace SR.UI
{
    public class Popup_MintNft : UIPopup
    {
        public RawImage NftIcon;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private Button _mintBtn;
        [SerializeField] private Button _cancelBtn;
        [SerializeField] private TextMeshProUGUI _titleText;
        private RenderTexture rt;
        public bool IsImg;
        public Nft Nft;

        public void Start() 
        {
            _cancelBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.ShowPopup(PopupName.Inventory);
                Hide();
            });

            _mintBtn.onClick.AddListener(() =>
            {
                User.Instance.GetSignature(Nft.Rarity.ToString(), Nft.Id, res =>
                {
                    Dictionary<string, object> handleParams = new Dictionary<string, object>();
                    UIManager.Instance.ShowPopup(PopupName.Waiting);
                    ReactInteractor.Instance.Send_MintNft(int.Parse(res.Data.Rarity), res.Data.Nonce, res.Data.Sig, res.Data.NftId);
                    Hide();
                });
            });

        }

        public override void Show(Dictionary<string, object> customProperties = null)
        {
            base.Show(customProperties);
            if (customProperties != null)
            {
                Nft = customProperties["nft"] as Nft;
                IsImg = bool.Parse(customProperties["isImg"].ToString());
                if(IsImg) {
                    NftIcon.texture = customProperties["texture"] as Texture;
                } else {
                    SetupVideo(customProperties["videoUrl"].ToString());
                }

                _titleText.text = $"Do you want to mint <#4BEAEE>The {Nft.Name} <#fff>NFT?";
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
            NftIcon.texture = rt;

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
