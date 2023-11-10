using System;
using UnityEngine;

namespace CodeBase.BombLogic
{
    [RequireComponent(typeof(Animator))]
    public class BombAnimator : MonoBehaviour
    {
        private readonly int _explosionHash = Animator.StringToHash("Explosion");
        private readonly int _respawnHash = Animator.StringToHash("Respawn");
        
        private Animator _animator;

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void Explode() => _animator.SetTrigger(_explosionHash);
        public void Respawn() => _animator.SetTrigger(_respawnHash);
    }
}