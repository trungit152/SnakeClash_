﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Inspector
{
    [Serializable]
    public class StringStringDictionary : SerializableDictionary<string, string> { }

    [Serializable]
    public class ObjectObjectDictionary : SerializableDictionary<UnityEngine.Object, UnityEngine.Object> { }

    [Serializable]
    public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> { }

    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        TKey[] mKeys;
        [SerializeField]
        TValue[] mValues;

        public SerializableDictionary()
        {
        }

        public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict.Count)
        {
            var enumerator = dict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var kvp = enumerator.Current;
                //foreach (var kvp in dict)
                //{
                this[kvp.Key] = kvp.Value;
            }
        }

        public void CopyFrom(IDictionary<TKey, TValue> dict)
        {
            this.Clear();
            var enumerator = dict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var kvp = enumerator.Current;
                //foreach (var kvp in dict)
                //{
                this[kvp.Key] = kvp.Value;
            }
        }

        public void OnAfterDeserialize()
        {
            if (mKeys != null && mValues != null && mKeys.Length == mValues.Length)
            {
                this.Clear();
                int n = mKeys.Length;
                for (int i = 0; i < n; ++i)
                {
                    this[mKeys[i]] = mValues[i];
                }

                mKeys = null;
                mValues = null;
            }

        }

        public void OnBeforeSerialize()
        {
            int n = this.Count;
            mKeys = new TKey[n];
            mValues = new TValue[n];

            int i = 0;
            var enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var kvp = enumerator.Current;
                //foreach (var kvp in this)
                //{
                mKeys[i] = kvp.Key;
                mValues[i] = kvp.Value;
                ++i;
            }
        }
    }
}