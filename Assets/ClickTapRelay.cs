using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTapRelay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Click判定
        if (Input.GetMouseButtonDown(0))
        {
            //クリックの当たり判定情報を取得
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

            //接触するコライダーが一つ以上あるか確認
            if(hits.Length != 0)
            {
                foreach(RaycastHit2D hit in hits)
                {
                    //Keyコンポーネントが付与されていればKeyとして認識
                    Key k = hit.collider.gameObject.GetComponent<Key>();
                    if (k)
                    {
                        //該当するキーのClicked通知を行う
                        k.KeyClicked();
                    }
                }
            }
        }

        //Tap判定
        if (Input.touchCount != 0)
        {
            //Touch情報をforeachで全取得
            foreach (Touch t in Input.touches)
            {
                //tに格納されたTouch情報がTouchされ始めの状態のものだったら
                if(t.phase == TouchPhase.Began)
                {
                    //タップの当たり判定情報を取得
                    Ray ray = Camera.main.ScreenPointToRay(t.position);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

                    //接触するコライダーが一つ以上あるか確認
                    if (hits.Length != 0)
                    {
                        foreach (RaycastHit2D hit in hits)
                        {
                            //Keyコンポーネントが付与されていればKeyとして認識
                            Key k = hit.collider.gameObject.GetComponent<Key>();
                            if (k)
                            {
                                //該当するキーのTouched通知を行う
                                k.KeyTouched();
                            }
                        }
                    }
                }
            }
        }
	}
}
