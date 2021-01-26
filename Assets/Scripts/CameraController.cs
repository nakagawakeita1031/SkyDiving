using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //(古い)まずカメラを追従させる対象となるプレイヤーを格納させるための変数を用意する
    //対象をPlayerControllerスクリプトから参照できるよう変数を変更する
    [SerializeField]
    private PlayerController PlayerController;
    //キャラとカメラの距離を取得する変数
    private Vector3 offset;

    //一人称カメラのCameraコンポーネント代入用
    [SerializeField]
    private Camera fpsCamera;

    //自撮りカメラのCameraコンポーネント代入用
    [SerializeField]
    private Camera selfishCamera;

    //カメラ制御用ボタンのButtonコンポーネント代入用
    [SerializeField]
    private Button btnChangeCamera;

    //現在適用しているカメラの通し番号
    private int cameraIndex = 0;
    //Main CameraゲームオブジェクトのCameraコンポーネント代入用
    private Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        //自分(カメラ)とペンギンとの相対距離を求める
        offset = transform.position - PlayerController.transform.position;

        //Main Cameraゲームオブジェクトを代入する
        mainCamera = Camera.main;

        //ボタンのOnClickイベントにChangeCameraメソッドを追加
        btnChangeCamera.onClick.AddListener(ChangeCamera);

        //SetDefaultCameraメソッドを取得する
        SetDefaultCamera();
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

    /// <summary>
    /// カメラを変更(ボタンを押すたびに呼び出される)
    /// </summary>
    public void ChangeCamera()
    {
        //現在のカメラの通し番号に応じて、次のカメラを用意して切り替える
        switch (cameraIndex)
        {
            case 0:
                cameraIndex++;
                mainCamera.enabled = false;
                fpsCamera.enabled = true;
                break;
            case 1:
                cameraIndex++;
                fpsCamera.enabled = false;
                selfishCamera.enabled = true;
                break;
            case 2:
                cameraIndex = 0;
                selfishCamera.enabled = false;
                mainCamera.enabled = true;
                break;
        }
    }

    /// <summary>
    /// カメラを初期カメラ(三人称カメラ)に戻す
    /// </summary>
    public void SetDefaultCamera()
    {
        cameraIndex = 0;

        mainCamera.enabled = true;
        fpsCamera.enabled = false;
        selfishCamera.enabled = false;
    }
}
