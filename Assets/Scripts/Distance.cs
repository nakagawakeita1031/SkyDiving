using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour
{
    //水面のオブジェクト取得用
    [SerializeField]
    private GameObject waterBasicDaytime;


    private float distance;

    [SerializeField]
    private Text dstText;

    private bool isGoal;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //距離が0以下になったらゴールしたと判定して距離の計算は行わないようにする
        if (isGoal == true)
        {
            //Debug.Log("trueになったから距離計算終了");
            return;
        }

        //プレイヤーと水面の距離を取得する
        distance = transform.position.y - waterBasicDaytime.transform.position.y;


        

        //距離が0に以下になったら
        if (distance <= 0)
        {
            isGoal = true;
            distance = 0f;
            //Debug.Log("trueになった");
        }
        //文字列にしてゲーム画面上に距離を表示する
        dstText.text = distance.ToString("F2");
    }
}
