using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    // Start is called before the first frame update
    void Start()
    {
        //まずはじめにアタッチするオブジェクトの物理特性情報を取得し変数に格納
        rb = GetComponent<Rigidbody>();

        //初期の姿勢を設定(頭を水面方向に向ける)
        transform.eulerAngles = straightRotation;

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
        
    }
}
