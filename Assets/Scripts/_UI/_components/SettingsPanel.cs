using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HULTemplate
{
    public class SettingsPanel : MonoBehaviour
    {
        public CanvasGroup group;
        public SettingsSprite sound;
        public SettingsSprite vibration;
        public SettingsSprite music;

        private bool _music = true;

        public void OpenPanel()
        {
            group.DOFade(1f, 0.3f);
            group.interactable = true;
            group.blocksRaycasts = true;

            sound.on.SetActive(DataManager.instance.sound);
            sound.off.SetActive(!DataManager.instance.sound);

            vibration.on.SetActive(DataManager.instance.vibration);
            vibration.off.SetActive(!DataManager.instance.vibration);
        }
        public void ClosePanel()
        {
            group.DOFade(0f, 0.3f);
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        public void OnPressMusicButton()
        {
            _music = !_music;

           // if (!_music) AudioManager.instance.StopMusic();
            //else AudioManager.instance.PlayMusic();

            music.on.SetActive(_music);
            music.off.SetActive(!_music);

            //DataManager.instance.SetMusic(!DataManager.instance.music);
        }
        public void OnPressSoundButton()
        {
            DataManager.instance.SetSound(!DataManager.instance.sound);

            sound.on.SetActive(DataManager.instance.sound);
            sound.off.SetActive(!DataManager.instance.sound);

            //AudioManager.instance.StopMusic();
        }

        public void OnPressVibrationButton()
        {
            DataManager.instance.SetVibration(!DataManager.instance.vibration);

            vibration.on.SetActive(DataManager.instance.vibration);
            vibration.off.SetActive(!DataManager.instance.vibration);

        }
        [System.Serializable]
        public struct SettingsSprite
        {
            public GameObject on;
            public GameObject off;
        }
    }
}