using System;
using System.Linq;
using CodeBase.BombLogic;
using CodeBase.Factory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Spawner
{
    public class SpawnerBomb : MonoBehaviour
    {
        [SerializeField] private Bomb _bombPrefab;
        [SerializeField] private GameObject _container;
        [SerializeField] private Transform _spawnPosition;

        private Bomb _currentBomb;
        private GameFactory _gameFactory;
        private BombInput _input;

        private void Awake()
        {
            _input = new BombInput();
            _gameFactory = new GameFactory();
            _gameFactory.CreatePoolBomb(_bombPrefab, _container.transform, 5);
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Bomb.Recharge.performed += Recharge;
        }

        private void OnDisable()
        {
            _input.Bomb.Recharge.performed -= Recharge;
            _input.Disable();
        }

        private void Recharge(InputAction.CallbackContext context)
        {
            if (_currentBomb == null)
            {
                _currentBomb = _gameFactory.PoolBombs.FirstOrDefault(bomb => bomb.gameObject.activeSelf == false);
                _currentBomb.transform.position = _spawnPosition.position;
                _currentBomb.transform.rotation = Quaternion.Euler(Vector3.zero);
                
                _currentBomb.gameObject.SetActive(true);
            }
            else
            {
                if (_currentBomb.IsExplode)
                {
                    _currentBomb.IsExplode = false;
                    
                    _currentBomb = _gameFactory.PoolBombs.FirstOrDefault(bomb => bomb.gameObject.activeSelf == false);
                    _currentBomb.transform.position = _spawnPosition.position;
                    _currentBomb.transform.rotation = Quaternion.Euler(Vector3.zero);
                
                    _currentBomb.gameObject.SetActive(true);
                }
            }

        }
    }
}