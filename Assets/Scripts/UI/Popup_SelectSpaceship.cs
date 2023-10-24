using System.Collections;
using System.Collections.Generic;
using SR.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SR
{
    public class Popup_SelectSpaceship : UIPopup
    {
        [SerializeField] private List<Button> _typeList = new();
        [SerializeField] private Button _btnStartGame;
        [SerializeField] private Button _btnCancel;

        private int _currentType = 0;

        void Start()
        {

            _typeList.ForEach(typeS =>
            {
                typeS.onClick.AddListener(() =>
                {
                    _typeList.ForEach(t => t.GetComponent<UISpaceshipSelect>().DeSelect());
                    UISpaceshipSelect tokenSelect = typeS.gameObject.GetComponent<UISpaceshipSelect>();
                    tokenSelect.Select();
                    _currentType = tokenSelect.TypeSelect;
                });
            });
            _btnStartGame.onClick.AddListener(() => {
                GameplayManager.Instance.PlayGame(_currentType);
                Hide();
            });
            _btnCancel.onClick.AddListener(Hide);
        }

        public override void Show(Dictionary<string, object> customProperties = null)
        {
            base.Show(customProperties);
            _currentType = 0;
            _typeList[0].onClick.Invoke();

        }
    }
}
