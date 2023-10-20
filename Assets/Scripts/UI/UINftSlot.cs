using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.IO;

namespace SR.UI
{
    public class UINftSlot : MonoBehaviour
    {
        [SerializeField] private RawImage _imageIcon;
        [SerializeField] private VideoPlayer _videoPlayer;
        private RenderTexture rt;

        [SerializeField] private GameObject _loadingIcon;

        private string tempVideoFilePath;

        public void Setup(string url)
        {
            _imageIcon.color = Color.gray;
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
                }
                else
                {
                    Debug.LogError("Failed to download video: " + www.error);
                }
            }
        }

        // Play the video from a file
        private void PlayVideo(string videoFilePath)
        {
            _videoPlayer.url = videoFilePath;
            _videoPlayer.targetTexture = rt;
            _videoPlayer.Play();
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
