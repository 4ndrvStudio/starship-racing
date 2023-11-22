using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.IO;
using SR.UI;

namespace SR
{
    public class NftSlot : MonoBehaviour
    {
       [SerializeField] private RawImage _imageIcon;
        [SerializeField] private RawImage _imageThumbnail;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private Button _openButton;
        
        private RenderTexture rt;

        private bool _isLoaded;
        [SerializeField] private GameObject _loadingIcon;

        private Nft _nft;

        private string tempVideoFilePath;
        private bool _isImg;

        void Start() {
           // StartCoroutine(CheckContentType("https://ipfs.io/ipfs/QmNq7xEqEuZ3s5zgSZGYAJSsWjmL8eYoB29YCRmiEh3oMg"));
            _openButton.onClick.AddListener(() => {
                if(_isLoaded == false)
                    return;

                Dictionary<string, object> handleParams = new Dictionary<string, object>() {
                    {"nft", _nft}
                };
                
                if(_isImg) {
                    handleParams.Add("texture", _imageThumbnail.texture);
                    handleParams.Add("isImg", true);
                } else {
                    handleParams.Add("videoUrl",  _videoPlayer.url);
                    handleParams.Add("isImg", false);
                }
              
                UIManager.Instance.ShowPopup(PopupName.MintNft,handleParams);
                UIManager.Instance.HidePopup(PopupName.Inventory);
            });
        }
     
        public void Setup(Nft nft)
        {
           _nft = nft;
           _loadingIcon.SetActive(true);
           _imageIcon.enabled = false;
            StartCoroutine(CheckContentType(_nft.Url));
        }



        IEnumerator CheckContentType(string url)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    byte[] data = www.downloadHandler.data;

                    if (IsVideoContent(data))
                    {
                        SetupVideo(url);
                        _isImg =false;
                    }
                    else if (IsImageContent(data))
                    {
                        SetupImage(url);
                        _isImg =true;
                    }
                    else
                    {
                        //Debug.LogError("Unsupported content type for URL: " + url);
                        SetupVideo(url);
                        _isImg =false;
                    }
                }
                else
                {
                    Debug.LogError("Failed to download content: " + www.error);
                }
                _loadingIcon.SetActive(false);
                _imageIcon.enabled = true;
            }
        }

        private void SetupImage(string imageUrl)
        {
            StartCoroutine(DownloadImage(imageUrl));
        }

        IEnumerator DownloadImage(string imageUrl)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    _imageThumbnail.texture = DownloadHandlerTexture.GetContent(www);
                    _imageThumbnail.color = Color.white;
                    _isLoaded = true;
                }
                else
                {
                    Debug.LogError("Failed to download image: " + www.error);
                }
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
            _imageIcon.texture = rt;

            rt.Release();
            _videoPlayer.targetTexture = rt;


            using (UnityWebRequest www = UnityWebRequest.Get(videoUrl))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    _loadingIcon.SetActive(false);
                    _imageIcon.color = Color.white;
                    // tempVideoFilePath = Path.Combine(Application.persistentDataPath, $"{Random.Range(1,10000000)}.mp4");
                    // File.WriteAllBytes(tempVideoFilePath, www.downloadHandler.data);

                    // PlayVideo(tempVideoFilePath);
                    _videoPlayer.source = VideoSource.Url;
                    _videoPlayer.url = videoUrl;
                    _videoPlayer.Play();
                      _isLoaded = true;
                }
                else
                {
                    Debug.LogError("Failed to download video: " + www.error);
                }
            }
        }


        private bool IsVideoContent(byte[] data)
        {

            if (data.Length >= 4)
            {
                if (data[0] == 0x00 && data[1] == 0x00 && data[2] == 0x00 && data[3] == 0x01)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsImageContent(byte[] data)
        {

            if (data.Length >= 2)
            {
                if ((data[0] == 0xFF && data[1] == 0xD8) ||
                    (data[0] == 0x89 && data[1] == 0x50))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(tempVideoFilePath) && File.Exists(tempVideoFilePath))
            {
                File.Delete(tempVideoFilePath);
            }
        }

    }
}
