using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound_Controller : MonoBehaviour {
	
	AudioSource _audioSource;
	bool _isMute = false;
	
	void Awake () {
		_audioSource = GetComponent<AudioSource>();
		if(PlayerPrefs.HasKey("_Mute")){
			int _value = PlayerPrefs.GetInt("_Mute");
			if(_value == 0){_isMute = false;}
			if(_value == 1){_isMute = true;}
		} else {
			_isMute = false;
			PlayerPrefs.SetInt("_Mute", 0);
		}
		if( _isMute ){ _audioSource.mute = true; } else { _audioSource.mute = false; }
	}
	
	void Update () {
		int _value = PlayerPrefs.GetInt("_Mute");
			if(_value == 0){_isMute = false;}
				if(_value == 1){_isMute = true;}
					if( _isMute ){ _audioSource.mute = true; } else { _audioSource.mute = false; }
	}
}
