using System.Collections;
using UnityEngine;

namespace CodeBase.BombLogic
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private BombAnimator _bombAnimator;
        [SerializeField] private CameraShake _cameraShake;
        [SerializeField] private AudioSource _explosionAudio;
        [SerializeField] private LayerMask _layerToHit;
        [SerializeField] private float _fieldOfImpact;
        [SerializeField] private float _force;

        private readonly Collider2D[] _results = new Collider2D[10];

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Explode());
            }
        }

        private IEnumerator Explode()
        {
            int overlapCircleNonAlloc = Physics2D.OverlapCircleNonAlloc(transform.position, _fieldOfImpact, _results, _layerToHit);

            _bombAnimator.Explode();
            _explosionAudio.Play();
            _cameraShake.Shake(10, 1);

            yield return new WaitForSeconds(0.16f);

            for (int i = 0; i < overlapCircleNonAlloc; i++)
            {
                Vector2 direction = _results[i].transform.position - transform.position;
                _results[i].GetComponent<Rigidbody2D>().AddForce(direction * _force, ForceMode2D.Impulse);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _fieldOfImpact);
        }
    }
}