using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.BombLogic
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private BombAnimator _bombAnimator;
        [SerializeField] private CameraShake _cameraShake;
        [SerializeField] private LayerMask _layerToHit;
        [SerializeField] private float _fieldOfImpact;
        [SerializeField] private float _force;

        private readonly Collider2D[] _results = new Collider2D[10];

        private CircleCollider2D _circleCollider2D;
        private AudioSource _explosionAudio;

        private void Awake()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _explosionAudio = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Explode());
            }
        }

        private IEnumerator Explode()
        {
            _bombAnimator.Explode();

            yield return new WaitForSeconds(3);
            
            int overlapCircleNonAlloc = Physics2D.OverlapCircleNonAlloc(_circleCollider2D.offset, _fieldOfImpact, _results, _layerToHit);

            _explosionAudio.Play();
            _cameraShake.Shake(10, 1);

            for (int i = 0; i < overlapCircleNonAlloc; i++)
            {
                Vector2 direction = _results[i].transform.position - transform.position;
                _results[i].GetComponent<Rigidbody2D>().AddForce(direction * _force, ForceMode2D.Impulse);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_circleCollider2D.offset, _fieldOfImpact);
        }
    }
}