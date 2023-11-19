//日本語対応
using UnityEngine;

/// <summary> ダメージ処理用のインターフェイス。Enemy（ザコとボス両方）で継承する。 </summary>
public  interface IReceiveWater 
{
    /// <summary> 水に当たったときに呼ばれる</summary>
    public  void ReceiveWater();

}
