using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = create();
                }

                return instance;
            }
        }

        protected static T create()
        {
            T created = FindObjectOfType<T>();

            if (created == null)
            {
                created = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
            }

            return created;
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}
