using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 以下，Dictionaryを使えば済むように感じるが，Inspectorにトリガーを登録するためにはこうせざるを得ない

// トリガーの発火に必要な条件を示したクラス
// トリガーとなるGameObjectと，その条件で表される
[System.Serializable]
public class Trigger {
		public GameObject button;
		public bool flag;
}

// トリガーの発火に必要な条件と，発火した後に動くGameObject(Target)の組を持つクラス
[System.Serializable]
public class TriggerTarget {
		public List<Trigger> triggers;
		public GameObject target;
}

