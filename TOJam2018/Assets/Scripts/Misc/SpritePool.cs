using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class SpritePool : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer _pooledObjectPrefab;
        [SerializeField] private int _numberToPool;

        private Queue<SpriteRenderer> _pool = new Queue<SpriteRenderer>();

        private GameObject _poolParent;


        #region SETUP
        private void Awake()
        {

        }

        private void Start()
        {

        }

        public void Init()
        {
            CreatePool();
        }
        #endregion

        #region LOGIC
        public SpriteRenderer GetPooledObject()
        {
            if (_pool.Count > 0)
            {
                SpriteRenderer go = _pool.Dequeue();
                go.gameObject.SetActive(true);
                return go;
            }
            else
            {
                Debug.LogWarning("** NO POOLED OBJECTS OF TYPE: " + this.name.ToString() + " **");

                CreatePooledObject();
                SpriteRenderer go = _pool.Dequeue();
                go.gameObject.SetActive(true);
                return go;
            }
        }

        public void ReturnPooledObject(SpriteRenderer obstacle)
        {
            _pool.Enqueue(obstacle);
            obstacle.gameObject.transform.SetParent(_poolParent.transform);
            obstacle.transform.localPosition = Vector3.zero;
            obstacle.gameObject.SetActive(false);
        }

        private void CreatePool()
        {
            _poolParent = new GameObject("ObstaclePool");
            _poolParent.transform.SetParent(this.transform);


            //populate pool
            for (int i = 0; i < _numberToPool; i++)
            {
                CreatePooledObject();
            }
        }

        private void CreatePooledObject()
        {
            SpriteRenderer go = Instantiate(_pooledObjectPrefab) as SpriteRenderer;
            go.gameObject.transform.SetParent(_poolParent.transform);
            go.transform.localPosition = Vector3.zero;
            go.gameObject.SetActive(false);

            _pool.Enqueue(go);
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
