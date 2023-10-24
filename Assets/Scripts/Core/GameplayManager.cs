using System.Collections;
using System.Collections.Generic;
using SR.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SR
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance;

        public string SceneMenu = "scene_menu";
        public string SceneGameplay = "scene_gameplay";

        public bool IsInGame = false;
        public int CurrentSelect=0;
        public int ResultRank;
        // Start is called before the first frame update
        void Start()
        {
            if (Instance == null)
                Instance = this;

            Application.targetFrameRate = 60;
            WalletConnected();
        }

        public void WalletConnected()
        {
            GetUserProfile();
        }
        public void GetUserProfile() => User.Instance.GetUserProfile(() =>
        {
            SceneManager.LoadScene(SceneMenu);

        });

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(SceneMenu);
            //SoundManager.Instance.PlayBackground(EBackgroundType.Menu);
        }

        public void PlayGame(int type)
        {
            CurrentSelect = type;
            UIManager.Instance.ShowPopup(PopupName.Waiting);
            User.Instance.StartGame(type,
                res =>
                {
                    IsInGame = true;
                    Dictionary<string, object> dataPopup = new Dictionary<string, object>() {
                        {"Status", true},
                        {"StartGameRespone", res}
                    };
                    ResultRank = res.Data.Result;
                    
                    UIManager.Instance.Setup(PopupName.Result, dataPopup);
                    UIManager.Instance.HidePopup(PopupName.Waiting);
                    UIManager.Instance.HidePopup(PopupName.Result);
                    SceneManager.LoadScene(SceneGameplay);

                }, err =>
                {
                   UIManager.Instance.HidePopup(PopupName.Waiting);

                   if(err.Error.Contains("numberOfTurns")) {
                        UIManager.Instance.ShowPopup(PopupName.RequestBuyTurn);
                   } else {
                        UIManager.Instance.ShowPopup(PopupName.SomethingError);
                   }
                }
            );
        }

        public void Endgame()
        {
            IsInGame =false;
            UIManager.Instance.ShowPopup(PopupName.Result);
        }
        public void QuitGameplay()
        {
            SceneManager.LoadScene(SceneMenu);
        }


    }
}
