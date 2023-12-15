using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.BombLogic
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(AudioSource))]
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private BombAnimator _bombAnimator;
        [SerializeField] private LayerMask _layerToHit;
        [SerializeField] private float _fieldOfImpact;
        [SerializeField] private float _impulse;

        private readonly Collider2D[] _results = new Collider2D[10];

        private BombInput _bombInput;
        private CircleCollider2D _circleCollider2D;
        private AudioSource _explosionAudio;
        private CameraShake _cameraShake;
        private Rigidbody2D _rigidbody2D;

        public bool IsExplode { get; set; }

        private void Awake()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _explosionAudio = GetComponent<AudioSource>();
            _cameraShake = FindObjectOfType<CameraShake>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _bombInput = new BombInput();
        }

        private void OnEnable()
        {
            _bombInput.Enable();
            _bombInput.Bomb.Explode.performed += StartAnimationExplode;
        }

        private void OnDisable()
        {
            _bombInput.Bomb.Explode.performed -= StartAnimationExplode;
            _bombInput.Disable();
        }

        private void StartAnimationExplode(InputAction.CallbackContext context) =>
            _bombAnimator.Explode();

        private void Explode()
        {
            int overlapCircleNonAlloc =
                Physics2D.OverlapCircleNonAlloc(_circleCollider2D.offset, _fieldOfImpact, _results, _layerToHit);

            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _explosionAudio.Play();
            _cameraShake.Shake(20, 1);

            for (int i = 0; i < overlapCircleNonAlloc; i++)
            {
                Vector2 direction = _results[i].transform.position - transform.position;
                _results[i].GetComponent<Rigidbody2D>().AddForce(direction * _impulse, ForceMode2D.Impulse);
                _results[i].GetComponent<Rigidbody2D>().AddTorque(direction.x * _impulse, ForceMode2D.Impulse);
            }

            IsExplode = true;
        }

        private void Hide() =>
            StartCoroutine(OnHide());

        private IEnumerator OnHide()
        {
            yield return new WaitUntil(() => !_explosionAudio.isPlaying);
            
            gameObject.SetActive(false);
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _fieldOfImpact);
        }
    }
}