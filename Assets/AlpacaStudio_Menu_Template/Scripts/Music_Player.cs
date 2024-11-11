using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music_Player : MonoBehaviour {
	[Tooltip("_audioSource defines the Audio Source component in this scene.")]
	AudioSource _audioSource;
	[Tooltip("_audioTracks defines the audio clips to be played continuously through out the scene.")]
	public AudioClip[] _audioTracks;
	[Space(20)]
	[HeaderAttribute("Music Player Options")]
	[Tooltip("_playTracks acts as the Play/Stop function of the Music Player")]
	public bool _playTracks;
	[Tooltip("Skips to the next available _audioTracks clip.")]
	public bool _nextTrack;
	[Tooltip("Skips to the previous _audioTracks clip")]
	public bool _prevTrack;
	[Tooltip("Loops the current _audioTracks clip.")]
	public bool _loopTrack;
	[Space(20)]
	[HeaderAttribute("Debugging/ReadOnly")]
	[Tooltip("_playTracks is a ReadOnly variable that displays the current _audioTracks clip that is playing")]
	public int _playingTrack;
	[Tooltip("_isMute returns the status of muting function in the Sound_Controller.")]
	public bool _isMute = false;
	
	void Awake () {		
		_audioSource = GetComponent<AudioSource>();
		_audioSource.clip = _audioTracks[0];
		_playingTrack = 0;
		//Below is the imported Awake methods for global muting.
		if(PlayerPrefs.HasKey("_Mute")){
			int _value = PlayerPrefs.GetInt("_Mute");
			if(_value == 0){_isMute = false;}
			if(_value == 1){_isMute = true;}
		} else {
			_isMute = false;
			PlayerPrefs.SetInt("_Mute", 0);
		}
		if( _isMute ){ _audioSource.mute = true; } else { _audioSource.mute = false; _audioSource.Play();}
	}
	
	void Update () {
		if(!_playTracks) _audioSource.Stop();
		if(_playTracks && !_audioSource.isPlaying) StartPlayer();		
		if(_loopTrack){ _audioSource.loop = true; } else { _audioSource.loop = false; }
		if(_nextTrack){ NextTrack(); }
		if(_prevTrack){ PreviousTrack(); }
		
		//Below is the imported Update methods for global muting.
		int _value = PlayerPrefs.GetInt("_Mute");
			if(_value == 0){_isMute = false;}
				if(_value == 1){_isMute = true;}
					if( _isMute ){ _audioSource.mute = true; } else { _audioSource.mute = false; }
	}
	
	public void StartPlayer(){
		if(!_loopTrack) { //If the Audio Source is not set to loop the clip we will play the next clip.
			NextTrack();
		} else { //If Audio Source is set to loop we will play the same track again.
			_audioSource.Play(); 
		}
	}
	
	public void NextTrack(){
		_nextTrack = false;
		_audioSource.Stop();
			int _newCount = _playingTrack+1; //Find the next track.
			
				if(_newCount > _audioTracks.Length-1) { //Loop to beginning of _audioTracks. Prevents Array Index out of range errors.
					_audioSource.clip = _audioTracks[0]; _playingTrack = 0;
				} else {
					_audioSource.clip = _audioTracks[_newCount];_playingTrack = _newCount;
				} 
				_audioSource.Play();
				Debug.Log("Called NextTrack: _next="+_newCount+" : _playing="+_playingTrack +" : _name= "+_audioTracks[_playingTrack].name);
		
	}
	
	public void PreviousTrack(){
		_prevTrack = false;
		_audioSource.Stop();
			int _newCount = _playingTrack-1; //Find the previous track
			
				if(_newCount < 0) { //Loops to end of _audioTracks. Prevents Array Index out of range errors.
					_audioSource.clip = _audioTracks[_audioTracks.Length-1]; 
					_playingTrack = _audioTracks.Length-1;
				} else {
					_audioSource.clip = _audioTracks[_newCount];
					_playingTrack = _newCount;
				}
				_audioSource.Play();
				Debug.Log("Called PreviousTrack: _next="+_newCount+" : _playing="+_playingTrack+" : _name= "+_audioTracks[_playingTrack].name);
	}

}
