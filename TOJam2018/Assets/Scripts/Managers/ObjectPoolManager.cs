using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    [System.Serializable]
    public class PlatformDifficultyPools
    {
        public Constants.PlatformDifficulty Difficulty;
        public PlatformPool[] PlatformPools;
    }

    public class ObjectPoolManager : MonoBehaviour
    {

        [SerializeField] private PlatformDifficultyPools[] _pools;

        private static ObjectPoolManager _instance;
        public static ObjectPoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<ObjectPoolManager>();
                }
                return _instance;
            }
        }

        public void Init()
        {
            foreach(PlatformDifficultyPools pdp in _pools)
            {
                foreach (PlatformPool pool in pdp.PlatformPools)
                    pool.Init(pdp.Difficulty);
            }
        }

        public PlatformBase GetPooledObject(Constants.PlatformDifficulty diff)
        {
            foreach (PlatformDifficultyPools pdp in _pools)
            {
                if (pdp.Difficulty == diff)
                {
                    int random = Random.Range(0, pdp.PlatformPools.Length);

                    return pdp.PlatformPools[random].GetPooledObject();
                }
            }

            return null;
        }

        public void ReturnPooledObject( PlatformBase platform)
        {
            foreach (PlatformDifficultyPools pdp in _pools)
            {
                if (pdp.Difficulty == platform.Difficulty)
                {
                    foreach (PlatformPool pool in pdp.PlatformPools)
                    {
                        if (pool == platform.PlatformPoolRef)
                            pool.ReturnPooledObject(platform);
                    }
                }
            }
        }
    }
}

