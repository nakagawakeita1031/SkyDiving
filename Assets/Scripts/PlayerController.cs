﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class PlayerController : MonoBehaviour
{
    //プレイヤーを降下・移動に関して変数を用意する
    [Header("プレイヤーの移動速度")]
    //プレイヤーの移動速度
    public float moveSpeed;

    [Header("プレイヤーの降下速度")]
    //プレイヤーの降下速度
    public float fallSpeed;

    //着水or未着水をbool型のinWaterで設定
    [Header("着水判定。着水済みならtrue")]
    public bool inWater;

    //水しぶきエフェクト用の変数。Prefabをアサインするため。
    [SerializeField, Header("水しぶきエフェクト")]
    private GameObject splashEffectPrefab = null;

    //水しぶきの効果音用の変数Audioアサイン
    [SerializeField,Header("水しぶきSE")]
    private AudioClip splashSE = null;

    //アタッチしたオブジェクトの物理特性情報を操作する変数
    private Rigidbody rb;
    private float x;
    private float z;

    //降下中キャラの角度を変更(水面に頭)
    private Vector3 straightRotation = new Vector3(180, 0, 0);

    //花輪を通過した際の得点の合計値管理用
    private int score;

    //Textオブジェクトと紐づけ
    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private Button btnChangeAttitude;

    //キャラの状態の種類
    public enum AttitudeType
    {
        Straight,//直滑降(通常時)
        Prone,//伏せ
    }

    [Header("現在のキャラの姿勢")]
    public AttitudeType attitudeType;

    private Vector3 proneRotation = new Vector3(-90, 0, 0);

    //姿勢変更が可能になるまでの計測用変数
    private float attitudeTimer;

    //姿勢変更が可能になるまでのチャージ時間変数
    private float chargeTime = 2.0f;

    [SerializeField]
    private Image imgGauge;

    [Header("チャージ完了")]
    public bool charge_Completed;

    [SerializeField]
    private ShinyEffectForUGUI shinyEffect;

    private Animator anim;

    private void Awake()
    {
        btnChangeAttitude.interactable = false;
        charge_Completed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //まずはじめにアタッチするオブジェクトの物理特性情報を取得し変数に格納
        rb = GetComponent<Rigidbody>();

        //初期の姿勢を設定(頭を水面方向に向ける)
        transform.eulerAngles = straightRotation;

        //現在の姿勢を「直滑空」に変更(今までの姿勢)
        attitudeType = AttitudeType.Straight;

        //ボタンのOnClickイベントにChangeAttitudeメソッドを追加する
        btnChangeAttitude.onClick.AddListener(ChangeAttitude);

        anim = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        //操作するキーの名前
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        //キーを入力した際に正しいかどうかチェック
        //Debug.Log(x);
        //Debug.Log(z);

        //オブジェクトの物理特性情報を変更(x速度,y速度,z速度)
        rb.velocity = new Vector3(x * moveSpeed, -fallSpeed, z * moveSpeed);
        //変更した情報を表示する
        //Debug.Log(rb.velocity);
    }

    //IsTriggerがONでコライダーが持つゲームオブジェクトを通過した際に呼び出される
    private void OnTriggerEnter(Collider col)
    {
        //通過したゲームオブジェクトのコライダーのタグが"Water"でbool値がfalse(未着水)なら
        if (col.gameObject.tag == "Water" && inWater == false)
        {
            //bool値を着水済み(true)にする
            inWater = true;

            //水しぶきエフェクトを生成し、生成された水しぶきエフェクトをeffect変数に代入
            GameObject effect = Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);
            //水しぶきのエフェクトの発生地点をx,yそしてzから-0.5の位置に発生させる
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z - 0.5f);
            //effect変数を利用して、エフェクトを２秒後に破壊する
            Destroy(effect, 2.0f);

            AudioSource.PlayClipAtPoint(splashSE, transform.position);

            //着水後コルーチンメソッドを呼び出す
            StartCoroutine(OutOfWater());

            //Debug.Log("着水" + inWater);
        }

        //侵入したゲームオブジェクトのTagがFlowerCircleなら
        if (col.gameObject.tag == "FlowerCircle")
        {
            //Debug.Log("花輪ゲット");

            //侵入したFlowerCircle Tagを持つゲームオブジェクト(Collider)の親オブジェクト(FlowerCircle)
            //にアタッチされているFlowerCircleスクリプトを取得して、point変数を参照し、得点を加算する
            score += col.transform.parent.GetComponent<FlowerCicle>().point;

            //文字列に追加してint型やfloat型の情報を表示する場合には、ToString()メソッドを省略できる
            //Debug.Log("現在の得点：" + score);
            //TODO得点加算を追加する
            txtScore.text = score.ToString();
            //TODO画面に表示されている得点表示を更新する処理を追加する
        }

    }

    /// <summary>
    /// 水面に顔を出す
    /// </summary>
    /// <returns></returns>
    private IEnumerator OutOfWater()
    {
        //1秒待つ
        yield return new WaitForSeconds(1.0f);

        //RigidbodyコンポーネントのIsKinematicにスイッチを入れてキャラの操作を停止する
        rb.isKinematic = true;

        //キャラの姿勢(回転)を変更する
        transform.eulerAngles = new Vector3(-30, 180, 0);

        //DOTweenを利用して、１秒かけて水中から水面へとキャラを移動させる
        transform.DOMoveY(0.05f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //スペースキーを押したら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //姿勢の変更
            ChangeAttitude();
        }

        //姿勢が直下降の場合＆charge_Conpletedがfalseの場合
        if (attitudeType == AttitudeType.Straight && charge_Completed == false)
        {
            //タイマーを加算する　＝　チャージを行う
            attitudeTimer += Time.deltaTime;

            btnChangeAttitude.interactable = false;

            //ゲージ表示を更新
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            //タイマーがチャージ時間(満タン)になったら
            if (attitudeTimer >= chargeTime)
            {
                //タイマー値をチャージ時間で止めるようにする
                attitudeTimer = chargeTime;

                charge_Completed = true;

                btnChangeAttitude.interactable = true;

                //満タン時エフェクト
                shinyEffect.Play(0.5f);


            }
        }

        //姿勢が伏せの場合
        if (attitudeType == AttitudeType.Prone)
        {
            //タイマーを減算する
            attitudeTimer -= Time.deltaTime;

            //ゲージ表示を更新
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            //タイマー(チャージ)が0以下になったら
            if (attitudeTimer <= 0)
            {
                //タイマーをリセットし、再度計測できるようにする
                attitudeTimer = 0;
                //attitudeType = AttitudeType.Straight;
                //transform.DORotate(straightRotation, 0.25f);
                //rb.drag = 0f;
                //btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 90), 0.25f);
                //charge_Completed = false;

                btnChangeAttitude.interactable = false;

                ChangeAttitude();


            }
        }

    }

    /// <summary>
    /// 姿勢の変更
    /// </summary>
    private void ChangeAttitude()
    {
        //現在の姿勢に応じて姿勢を変更する
        switch (attitudeType)
        {
            //現在の姿勢が「直滑空」だったら
            case AttitudeType.Straight:

                //未チャージ状態(チャージ中)なら
                if (charge_Completed == false)
                {
                    //以降の処理を行わない＝未チャージ状態なのでチャージ時の処理を行わないようにする
                    return;
                }

                //チャージ状態を未チャージ状態にする
                charge_Completed = false;

                //現在の姿勢を「伏せ」に変更
                attitudeType = AttitudeType.Prone;

                //キャラを回転させて「伏せ」にする
                transform.DORotate(proneRotation, 0.25f, RotateMode.WorldAxisAdd);

                //空気抵抗値を上げて落下速度を遅くする
                rb.drag = 25.0f;

                //ボタンの子オブジェクト画像を回転させる
                btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 180), 0.25f);

                //伏せの状態に遷移するための条件を指定する(idle→stan)
                anim.SetBool("Prone", true);

                //処理を抜ける(次のcaseには処理がはいらない)
                break;

            //現在の姿勢が「伏せ」だったら
            case AttitudeType.Prone:

                //現在の姿勢を「直滑空」に変更
                attitudeType = AttitudeType.Straight;

                //キャラを回転させて「直滑空」にする
                transform.DORotate(straightRotation, 0.25f);

                //空気抵抗値を元に戻して落下速度を戻す
                rb.drag = 0f;

                //ボタンの子オブジェクトの画像を回転させる
                btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 90), 0.25f);

                //伏せの状態を止めるための遷移の条件を指定する(stan→idle)
                anim.SetBool("Prone", false);

                //処理を抜ける
                break;
        }
    }
}
