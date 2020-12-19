using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //アタッチしたオブジェクトの物理特性情報を操作する変数
    private Rigidbody rb;

    private float x;
    private float z;


    // Start is called before the first frame update
    void Start()
    {
        //まずはじめにアタッチするオブジェクトの物理特性情報を取得し変数に格納
        rb = GetComponent<Rigidbody>();
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

            //TODO 水しぶきエフェクトを生成

            Debug.Log("着水" + inWater);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
