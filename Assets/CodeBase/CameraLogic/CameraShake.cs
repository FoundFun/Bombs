using System.Collections;
using Cinemachine;
using UnityEngine;

namespace CodeBase
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShake : MonoBehaviour
    {
        private CinemachineBasicMultiChannelPerlin _shaker;

        private void Awake() =>
            _shaker = GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        public void Shake(float intensity, float time) =>
            StartCoroutine(OnShake(intensity, time));

        private IEnumerator OnShake(float intensity, float time)
        {
            float startTime = time;

            _shaker.m_AmplitudeGain = intensity;

            while (_shaker.m_AmplitudeGain > 0)
            {
                _shaker.m_AmplitudeGain = Mathf.Lerp(0, intensity, time / startTime);
                time -= Time.deltaTime;

                yield return null;
            }
        }
    }
}