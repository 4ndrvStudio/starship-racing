using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Unity.VisualScripting;

namespace SR
{
    public class User : MonoBehaviour
    {
        public static User Instance;
        public string UserAddress = "0x3735bbb3921bd1e363e02fd47f83d200c613df78";
        public UserData UserData;
        public int SynceTime = 0;
        public string RoundId;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void GetUserProfile(Action onGetSuccess = null)
        {
            UnityApiService.Instance.Get("profile", "address", UserAddress,
              res =>
              {
                  Debug.Log(res);

                  UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(res);
                  UserData = userResponse.Data;

                  if (UserData.CurrentRoundId != null)
                  {
                      Debug.Log("UserData " + UserData);
                      RoundId = UserData.CurrentRoundId;
                  }

                  if (UserData.UserId != null)
                      onGetSuccess?.Invoke();
                  else if (SynceTime < 5)
                  {
                      Debug.Log("UserData Is Null and try to get it!");
                      StartCoroutine(SynceProfile());
                      SynceTime += 1;
                  }
                  else
                  {
                      Debug.Log("You have not user profile in this game. Please Sign to Openworld Web!!");
                      //ReactInteractor.Instance.Send_QuitGame();
                  }
              }
              , err =>
              {
                  Debug.Log(err);
              },
              isRoot: true
          );
        }
        public IEnumerator SynceProfile()
        {
            yield return new WaitForSeconds(2f);

            SynceProfileRequest synceProfileRequest = new SynceProfileRequest
            {
                address = UserAddress
            };

            string json = JsonUtility.ToJson(synceProfileRequest);

            UnityApiService.Instance.Post("sync-profile", null, json,
              res =>
              {
                  Debug.Log("Synce success " + res);
                  //GameplayManager.Instance.GetUserProfile();
              }
              , err =>
              {
                  //GameplayManager.Instance.GetUserProfile();
                  Debug.Log(err);
              },
               isRoot: true
            );
        }

        public void StartGame(int selectType,Action<StartGameRespone> onSuccess, Action<StartGameRespone> onFail)
        {
            StartGameRequest startGameRequest = new StartGameRequest()
            {
                userAddress = UserData.UserAddress,
                userId = UserData.UserId,
                type  = selectType
            };

            string jsonBody = JsonUtility.ToJson(startGameRequest);

            UnityApiService.Instance.Post("start-game", null, jsonBody,
                res =>
                {
                    Debug.Log(res);
                    StartGameRespone respone = JsonConvert.DeserializeObject<StartGameRespone>(res);
                    onSuccess?.Invoke(respone);
                }, err =>
                {
                    StartGameRespone error = JsonConvert.DeserializeObject<StartGameRespone>(err);
                    Debug.Log(error.Error);
                    onFail?.Invoke(error);
                }
            );
        }

       
        public void GetUserBags(Action<GetBagsRespone> onSuccess, Action<GetBagsRespone> onFail)
        {
            UnityApiService.Instance.Get("bags", "userId", UserData.UserId,
              res =>
              {
                  GetBagsRespone getBagsResponeResponse = JsonConvert.DeserializeObject<GetBagsRespone>(res);
                  //Debug.Log(JsonConvert.SerializeObject(getBagsResponeResponse));
                  onSuccess?.Invoke(getBagsResponeResponse);
              }
              , err =>
              {
                  GetBagsRespone getBagsResponeResponse = JsonConvert.DeserializeObject<GetBagsRespone>(err);
                  onFail?.Invoke(getBagsResponeResponse);
              }
          );
        }

        public void OpenBags(int typeBag, int amountBag, Action<OpenBagsRespone> onSuccess = null, Action<OpenBagsRespone> onFail = null)
        {
            OpenBagsRequest openBagsRequest = new OpenBagsRequest
            {
                userId = UserData.UserId,
                type = typeBag,
                amount = amountBag
            };

            string json = JsonUtility.ToJson(openBagsRequest);

            UnityApiService.Instance.Post("openbags", null, json,
              res =>
              {
                  OpenBagsRespone respone = JsonConvert.DeserializeObject<OpenBagsRespone>(res);
                  GetUserProfile();
                  onSuccess?.Invoke(respone);
              }
              , err =>
              {
                  OpenBagsRespone openBagsRespone = JsonConvert.DeserializeObject<OpenBagsRespone>(err);
                  onFail?.Invoke(openBagsRespone);
              });
        }

        public void BuyTurnBuyOwner(ulong totalOwner, Action onSuccess = null, Action<BuyTurnByOwnerRespone> onFail = null)
        {
            BuyTurnByOwnerRequest buyTurnBuyOwnerRequest = new BuyTurnByOwnerRequest()
            {
                userId = UserData.UserId,
                address = UserData.UserAddress,
                totalOwner = totalOwner
            };

            string json = JsonUtility.ToJson(buyTurnBuyOwnerRequest);

            UnityApiService.Instance.Post("buy-turn-by-owner", null, json,
              res =>
              {
                  Debug.Log("Buy Success!");
                  GetUserProfile(() => onSuccess?.Invoke());
              }
              , err =>
              {
                  BuyTurnByOwnerRespone buyTurnByOwnerRespone = JsonConvert.DeserializeObject<BuyTurnByOwnerRespone>(err);
                  onFail?.Invoke(buyTurnByOwnerRespone);
              },
              isRoot: true
            );
        }



    }

    public class UserResponse
    {
        public UserData Data;
    }
    [Serializable]
    public class UserData
    {
        public string UserAddress;
        public string UserId;
        public string Token;
        public int NumberOfTurns;
        public int Stage;
        public int TokenOwnerBuyed;
        public int TotalPoint;
        public int UsdBuyed;
        public string CurrentStageId;
        public string CurrentRoundId;
        public ulong Owner;
    }
    public class SynceProfileRequest
    {
        public string address;
    }

    public class StartGameRequest
    {
        public string userAddress;
        public string userId;
        public int type;
    }

    public class StartGameRespone
    {
        public int? Status;
        public string? Error;
        public StartGameData? Data;
    }

    public class StartGameData {
        public string RoundId;
        public int Result;
        public List<BagsRespone> Bags;
    }

    public class GetBagsRespone
    {
        public string? Error;
        public List<BagsRespone>? Data;

    }
    public class BagsRespone
    {
        public int Type;
        public int Amount;
        public BagsRespone(int types, int amounts)
        {
            Type = types;
            Amount = amounts;
        }
    }

    public class OpenBagsRequest
    {
        public string userId;
        public int type;
        public int amount;
    }
    public class OpenBagsRespone
    {
        public string? Error;
        public Reward? Data;
    }

    public class Reward
    {
        public int? Owner;
        public List<string> Nft;
    }

    public class BuyTurnByOwnerRequest
    {
        public string userId;
        public string address;
        public ulong totalOwner;
    }
    public class BuyTurnByOwnerRespone
    {
        public string? Error;
        public string? Message;
    }


}
