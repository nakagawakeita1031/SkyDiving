using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //(古い)まずカメラを追従させる対象となるプレイヤーを格納させるための変数を用意する
    //対象をPlayerControllerスクリプトから参照できるよう変数を変更する
    [SerializeField]
    private PlayerController PlayerController;
    //キャラとカメラの距離を取得する変数
    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        //自分(カメラ)とペンギンとの相対距離を求める
        offset = transform.position - PlayerController.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //着水状態なら
        if (PlayerController.inWater == true)
        {
            //ここで処理を終了させることで下の処理に行かない
            return;
        }

        //自分とペンギンの相対距離を保って追従させる
        if (PlayerController != null)
        {
            //自分の位置はペンギンの位置からoffsetで求めた距離を保つようにUpdateで常に監視する
            transform.position = PlayerController.transform.position + offset;
        }
    }
}
