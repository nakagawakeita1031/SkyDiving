using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerCicle : MonoBehaviour
{
    [Header("花輪通過時の得点")]
    public int point;

    // Start is called before the first frame update
    void Start()
    {
        //アタッチしたゲームオブジェクト(花輪)を回転させる
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
