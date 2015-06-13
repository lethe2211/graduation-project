using UnityEngine;
using System.Collections;

// 効果音の再生に関するクラス
public class SoundManager : MonoBehaviour {

		AudioSource[] audioSources;

		// Use this for initialization
		void Start () {		
				audioSources = gameObject.GetComponents<AudioSource> ();
		}

		// このメソッドに，音源の名前を引数に含めてメッセージを送ると，SoundManagerにアタッチされたAudioSourceが再生される
		void Play(string clip) {
				foreach (AudioSource audioSource in audioSources) {
						if (audioSource.clip.name == clip) 
								audioSource.PlayOneShot (audioSource.clip);
				}
		}
}
