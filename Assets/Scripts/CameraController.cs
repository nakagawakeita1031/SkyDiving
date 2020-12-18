using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //まずカメラを追従させる対象となるプレイヤーを格納させるための変数を用意する
    [SerializeField]
    private GameObject playerObj;
    //キャラとカメラの距離を取得する変数
    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        //自分(カメラ)とペンギンとの相対距離を求める
        offset = transform.position - playerObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //自分とペンギンの相対距離を保って追従させる
        if (playerObj != null)
        {
            //自分の位置はペンギンの位置からoffsetで求めた距離を保つようにUpdateで常に監視する
            transform.position = playerObj.transform.position + offset;
        }
    }
}
