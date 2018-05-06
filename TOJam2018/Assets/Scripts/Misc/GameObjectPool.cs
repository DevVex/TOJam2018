using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace TOJAM
{
    public class GameObjectPool : MonoBehaviour
    {

        [SerializeField] private GameObject _pooledObjectPrefab;
        [SerializeField] private int _numberToPool;

        private Queue<GameObject> _pool = new Queue<GameObject>();

        private GameObject _poolParent;
        private bool _poolCreated = false;

        #region SETUP
        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (_poolCreated == false)
            {
                CreatePool();
            }
        }
        #endregion

        #region LOGIC
        public GameObject GetPooledObject()
        {
            if (_pool.Count > 0)
            {
                GameObject go = _pool.Dequeue();
                go.gameObject.SetActive(true);

                return go;
            }
            else
            {

                CreatePooledObject();
                GameObject go = _pool.Dequeue();
                go.gameObject.SetActive(true);
                return go;
            }
        }

        public void ReturnPooledObject(GameObject obstacle)
        {
            _pool.Enqueue(obstacle);
            obstacle.gameObject.transform.SetParent(_poolParent.transform);
            obstacle.transform.localPosition = new Vector3(-100f, 0f,0f);
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
            GameObject go = Instantiate(_pooledObjectPrefab) as GameObject;
            go.gameObject.transform.SetParent(_poolParent.transform);
            go.transform.localPosition = new Vector3(-100f, 0f, 0f);
            go.gameObject.SetActive(false);

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
