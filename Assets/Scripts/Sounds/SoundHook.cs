using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class SoundHook : MonoBehaviour
    {
        public Sound m_Sound;
        public void Play() { m_Sound.Play(); }
        public void Stop() { m_Sound.Stop(); }
    }
}