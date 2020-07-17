using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AudioClipName
{
	ObjectSelected,
	MenuClick,
	BuilderConfirm,
	BuilderCancel
}


public static class AudioManager
{
	static bool _initialized = false;
	static AudioSource _audioSource;
	static Dictionary<AudioClipName, AudioClip> audioClips =
		new Dictionary<AudioClipName, AudioClip>();

	public static bool Initialized => _initialized;

	public static void Initialize(AudioSource source)
	{
		_initialized = true;
		_audioSource = source;

		audioClips.Add(AudioClipName.BuilderConfirm, Resources.Load<AudioClip>("Audio/plant_sound"));
		audioClips.Add(AudioClipName.BuilderCancel, Resources.Load<AudioClip>("Audio/glassBreaks_sound"));
		audioClips.Add(AudioClipName.ObjectSelected, Resources.Load<AudioClip>("Audio/pick_sound"));
		audioClips.Add(AudioClipName.MenuClick, Resources.Load<AudioClip>("Audio/click_sound"));

	}

	public static void Play(AudioClipName name)
	{
		if (!_initialized)
			return;

		_audioSource.PlayOneShot(audioClips[name]);
	}
}
