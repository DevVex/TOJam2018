using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class LoadingRotate : MonoBehaviour
    {

        public float Speed = 1f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(this.gameObject.activeInHierarchy == true)
            {
                Vector3 targetEuler = new Vector3(0f, 0f, this.transform.rotation.eulerAngles.z + Speed);

                Quaternion targetRot = Quaternion.Euler(targetEuler);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRot, Time.time );
            }

        }
    }
}

