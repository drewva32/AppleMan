using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionMusicFader : MonoBehaviour
{
    [SerializeField] private EMusicType musicType;
    private Dictionary<EMusicType, Action> musicDictionary = new Dictionary<EMusicType, Action>();
    private MusicAudioController _musicAudioController;

    private bool _init;
    private void Awake()
    {
        _musicAudioController = AudioManager.Instance.MusicAudioController;
        
        // musicDictionary.Add(EMusicType.BattleThemeOne, _musicAudioController.FadeToBattleThemeOne);
        // musicDictionary.Add(EMusicType.BattleThemeThree, _musicAudioController.FadeToBattleThemeThree);
        // musicDictionary.Add(EMusicType.PlatformerThemeEleven, _musicAudioController.FadeToPlatformerThemeEleven);
        // musicDictionary.Add(EMusicType.PlatformerThemeFive, _musicAudioController.FadeToPlatformerThemeFive);
        // musicDictionary.Add(EMusicType.PlatformerThemeOne, _musicAudioController.FadeToPlatformerThemeOne);
        // musicDictionary.Add(EMusicType.ShortLoopSix,_musicAudioController.FadeToShortLoopSix);
        // musicDictionary.Add(EMusicType.ShortLoopThree,_musicAudioController.FadeToShortLoopThree);
    }

    private void OnEnable()
    {
        if (_init)
        {
            Action FadeOutThenIn = _musicAudioController.musicDictionary[musicType];
            FadeOutThenIn();
        }
        _init = true;
    }
}
