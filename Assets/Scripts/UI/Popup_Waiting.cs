using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SR.UI
{
    public class Popup_Waiting : UIPopup
    {

        [SerializeField] private Transform _iconWaiting;
        private Tweener tweener;
        
        public void Start() {
            _iconWaiting.DORotate(new Vector3(0, 0, -360), 0.7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);

        }

        public override void Show(Dictionary<string, object> customProperties)
        {
            base.Show(customProperties);
        }

        public override void Hide() {
            base.Hide();
                tweener.Kill();
        }

    }

}