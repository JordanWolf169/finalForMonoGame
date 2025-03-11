using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace NEITGameEngine.Sound
{
    public class SoundManager
    {
        private Dictionary<string, SoundEffect> _soundEffects;
        private Dictionary<string, SoundEffectInstance> _soundEffectInstances;
        public SoundManager() { 
            _soundEffects = new Dictionary<string, SoundEffect>();
            _soundEffectInstances = new Dictionary<string, SoundEffectInstance>();
        }
        
        //Load a sound effect
        public void LoadSoundEffect(string name, SoundEffect soundEffect)
        {
            if (!_soundEffects.ContainsKey(name))
            {
                _soundEffects[name] = soundEffect;
            }
        }

        //Play Sound Effect
        public void PlaySoundEffect(string name)
        {
            if (_soundEffects.ContainsKey(name))
            {
                _soundEffects[name].Play();
            }
        }

        //Create sound effect instance to control audio
        public void CreateSoundEffectInstance(string name)
        {
            if (!_soundEffectInstances.ContainsKey(name) && _soundEffects.ContainsKey(name))
            {
                _soundEffectInstances[name] = _soundEffects[name].CreateInstance();
            }
        }

        //Play Sound Effect Instance
        public void PlaySoundEffectInstance(string name, bool loop = false)
        {
            if (_soundEffectInstances.ContainsKey(name))
            {
                var instance = _soundEffectInstances[name];
                if (instance.State != SoundState.Playing)
                {
                    //play the audio
                    instance.IsLooped = loop;
                    instance.Play();
                }
            }
        }

        //Stop the Sound Effect Instance
        public void StopSoundEffectInstance(string name)
        {
            if (_soundEffectInstances.ContainsKey(name))
            {
                //stop the audio
                _soundEffectInstances[name].Stop();
            }
        }

        //Utility function to check if audio is playing
        public bool IsSoundEffectInstancePlaying(string name)
        {
            return _soundEffectInstances.ContainsKey(name) && _soundEffectInstances[name].State == SoundState.Playing;
        }

    }
}
