using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerCicle : MonoBehaviour
{
    [Header("花輪通過時の得点")]
    public int point;

    [SerializeField]
    private BoxCollider boxCollider;

    [SerializeField]
    private GameObject effectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //アタッチしたゲームオブジェクト(花輪)を回転させる
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

    }

    //花輪から見て、他のゲームオブジェクトが花輪に侵入した場合
    private void OnTriggerEnter(Collider other)
    {
        //花輪のBoxColliderのスイッチをオフにして重複判定を防止
        boxCollider.enabled = false;
        //花輪をキャラの子オブジェクトにする
        transform.SetParent(other.transform);
        //花輪をくぐった際の演出
        StartCoroutine(PlayGetEffect());
    }

    /// <summary>
    /// 花輪をくぐった際の演出
    /// </summary>
    private IEnumerator PlayGetEffect()
    {
        //DOTweenのSequenceを宣言して利用できるようにする
        Sequence sequence = DOTween.Sequence();

        //Appendを実行すると、引数でDOTeenの処理を実行できる
        //花輪のScaleを1秒かけて0にして見えなくする
        sequence.Append(transform.DOScale(Vector3.zero, 1.0f));

        //Joinを実行することで、Appendと一緒にDOTweenの処理を行える
        //花輪のScaleが1秒かけて0になるのと一緒に、プレイヤーの位置に花輪を移動させる
        sequence.Join(transform.DOLocalMove(Vector3.zero, 1.0f));

        //1秒処理を中断(待機する)
        yield return new WaitForSeconds(1.0f);

        //エフェクトを生成して、Instantiateメソッドの戻り値をeffect変数に代入
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        //エフェクトの位置(高さ)を調整する
        effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y - 1.5f, effect.transform.position.z);

        //1秒後にエフェクトを破棄する(すぐに破棄するとエフェクトがすべて再生されないため)
        Destroy(effect, 1.0f);

        //花輪を１秒後に破棄
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
