using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR.UI
{
    public enum PopupName
    {
        None,
        Result,
        ResultSucess,
        ResultFail,
        BuyMoreTurn,
        Waiting,
        Finish,
        Inventory,
        Reward,
        OpenAnimation,
        RequestBuyTurn,
        SomethingError,
        SelectSpaceship

    }

    public class UIPopup : MonoBehaviour
    {
        [SerializeField] private PopupName _popupName = PopupName.None;
        public PopupName PopupName => _popupName;

        protected Dictionary<string, object> _customProperties;

        public virtual void Setup(object data =null) {}

        public virtual void Show(Dictionary<string, object> customProperties = null)
        {
            this._customProperties = customProperties;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            _customProperties = null;
            gameObject.SetActive(false);
        }


    }
}
