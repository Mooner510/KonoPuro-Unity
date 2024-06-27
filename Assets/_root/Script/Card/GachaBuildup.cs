using System;
using System.Collections.Generic;
using _root.Script.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _root.Script.Card
{
    public class GachaBuildup : MonoBehaviour
    {
        private readonly List<Tuple<GameObject, Vector3, float, float>> _shakeList = new();
        
        public void GachaBuildup1Started()
        {
            AudioManager.PlaySoundInstance("Audio/GACHA-1");
            ShakeObject(GameObject.FindGameObjectWithTag("MainCamera"), 0.1f, 0.4f);
            ShakeObject(gameObject, 0.1f, 0.4f);
        }
        
        private void ShakeObject(GameObject obj, float shakeAmount, float shakeTime)
        {
            _shakeList.Add(new Tuple<GameObject, Vector3, float, float>(obj, obj.transform.position, shakeAmount, shakeTime));
        }

        private void Update()
        {
            for (var i = 0; i < _shakeList.Count; i++)
            {
                if (_shakeList[i].Item4 > 0)
                {
                    _shakeList[i].Item1.transform.position = Random.insideUnitSphere * _shakeList[i].Item3 + _shakeList[i].Item2;
                    _shakeList[i] = new Tuple<GameObject, Vector3, float, float>(_shakeList[i].Item1, _shakeList[i].Item2, _shakeList[i].Item3, _shakeList[i].Item4 - Time.deltaTime);
                }
                else
                {
                    _shakeList[i].Item1.transform.position = _shakeList[i].Item2;
                    _shakeList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void GachaBuildup2Audio(string path)
        {
            AudioManager.PlaySoundInstance(path);
        }

        public void GachaBuildup1Ended()
        {
            SceneManager.LoadScene("GachaDirectingBuildupScene");
        }

        public void GachaBuildup2Ended()
        {
            SceneManager.LoadScene("GachaDirectingScene");
        }
    }
}