using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TOJAM
{
    public class PlatformPool : MonoBehaviour
    {
        [SerializeField] private PlatformBase _pooledObjectPrefab;
        [SerializeField] private int _numberToPool;

        private Queue<PlatformBase> _pool = new Queue<PlatformBase>();

        private GameObject _poolParent;
        private bool _poolCreated = false;

        private Constants.PlatformDifficulty _storedDiff;

        #region SETUP
        private void Awake()
        {
           
        }

        public void Init(Constants.PlatformDifficulty diff)
        {
            if (_poolCreated == false)
            {
                _storedDiff = diff;
                CreatePool();
            }
        }
        #endregion

        #region LOGIC
        public PlatformBase GetPooledObject()
        {
            if (_pool.Count > 0)
            {
                PlatformBase go = _pool.Dequeue();
                go.gameObject.SetActive(true);

                return go;
            }
            else
            {

                CreatePooledObject();
                PlatformBase go = _pool.Dequeue();
                go.gameObject.SetActive(true);
                return go;
            }
        }

        public void ReturnPooledObject(PlatformBase obstacle)
        {
            _pool.Enqueue(obstacle);
            obstacle.gameObject.transform.SetParent(_poolParent.transform);
            obstacle.transform.localPosition = new Vector3(-100f, 0f, 0f);
            obstacle.gameObject.SetActive(false);
        }

        private void CreatePool()
        {
            _pool.Clear();
            _poolParent = new GameObject("ObstaclePool " + _pooledObjectPrefab.name);
            _poolParent.transform.SetParent(this.transform);

            //populate pool
            for (int i = 0; i < _numberToPool; i++)
            {
                CreatePooledObject();
            }

            _poolCreated = true;
        }

        private void CreatePooledObject()
        {
            PlatformBase go = Instantiate(_pooledObjectPrefab) as PlatformBase;
            go.gameObject.transform.SetParent(_poolParent.transform);
            go.transform.localPosition = new Vector3(-100f, 0f, 0f);
            go.gameObject.SetActive(false);

            go.Difficulty = _storedDiff;
            go.PlatformPoolRef = this;

            _pool.Enqueue(go);
        }

        public GameObject GetRandomObstacleType()
        {
            Array values = Enum.GetValues(typeof(GameObject));
            return (GameObject)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
        #endregion

        #region CLEANUP
        private void OnDestroy()
        {

        }

        private void Reset()
        {

        }
        #endregion
    }

}
