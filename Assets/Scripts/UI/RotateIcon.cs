using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RotateIcon : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(0, 0, -360), 0.7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

}
