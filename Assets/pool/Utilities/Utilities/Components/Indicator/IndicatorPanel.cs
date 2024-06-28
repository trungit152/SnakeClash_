﻿////
/// Credit https://unitylist.com/p/6je/Unity-Asset-Easy-Indicators
////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utilities.Components
{
    public class IndicatorPanel : MonoBehaviour
    {
        [Header("User-Assigned Variables")]
        public GameObject offScreen;
        public GameObject onScreen;

        private void Start()
        {
            if (offScreen == null && onScreen == null)
                Debug.Log("IndicatorPanel's off-screen & on-screen gameobject is not assigned!");
        }

        public void SlideTransition(Transform target, Vector3 startPos, Vector3 endPos, float duration, bool DisableOnFinish)
        {
            StartCoroutine(CoSlideTransition(target, startPos, endPos, duration, DisableOnFinish));
        }

        public void FadeTransition(Transform target, int targetAlpha, float duration, bool DisableOnFinish)
        {
            StartCoroutine(CoFadeTransition(target, targetAlpha, duration, DisableOnFinish));
        }

        public void ScaleTransition(Transform target, Vector2 startSize, Vector3 endSize, float duration, bool DisableOnFinish)
        {
            StartCoroutine(CoScaleTransition(target, startSize, endSize, duration, DisableOnFinish));
        }

        public void RotateTransition(Transform target, Quaternion startRotation, Quaternion endRotation, float duration, bool DisableOnFinish)
        {
            StartCoroutine(CoRotateTransition(target, startRotation, endRotation, duration, DisableOnFinish));
        }

        #region Coroutine for sliding transition

        /// <summary>
        /// Coroutine for animating the position of a target's indicator from a starting position to an ending position with a duration.
        /// </summary>
        private IEnumerator CoSlideTransition(Transform target, Vector3 startPos, Vector3 endPos, float duration, bool DisableOnFinish)
        {
            float ratio = 0;
            float multiplier = 1 / duration;

            target.localPosition = startPos;

            while (target.localPosition != endPos)
            {
                //Increment time
                ratio += Time.deltaTime * multiplier;

                //Adjust scale using Lerp
                target.localPosition = Vector3.Lerp(startPos, endPos, ratio);

                yield return null;
            }

            if (DisableOnFinish)
                target.gameObject.SetActive(false);
        }

        #endregion

        #region Coroutine for scaling transition

        /// <summary>
        /// Coroutine for animating the scale of a target's indicator from a starting size to an ending size with a duration.
        /// </summary>
        private IEnumerator CoScaleTransition(Transform target, Vector3 startSize, Vector3 endSize, float duration, bool DisableOnFinish)
        {
            float ratio = 0;
            float multiplier = 1 / duration;

            target.localScale = startSize;

            while (target.localScale != endSize)
            {
                //Increment time
                ratio += Time.deltaTime * multiplier;

                //Adjust scale using Lerp
                target.localScale = Vector3.Lerp(startSize, endSize, ratio);

                yield return null;
            }

            if (DisableOnFinish)
                target.gameObject.SetActive(false);
        }

        #endregion

        #region Coroutine for fading transition

        /// <summary>
        /// Coroutine for animating the alpha of a target's indicator from a starting size to an ending size with a duration.
        /// </summary>
        private IEnumerator CoFadeTransition(Transform target, int targetAlpha, float duration, bool DisableOnFinish)
        {
            //Find each graphic object and store it. Includes all images, texts, etc.
            Graphic[] graphics = target.GetComponentsInChildren<Graphic>(true);

            if (graphics.Length > 0)
                for (int i = 0; i < graphics.Length; i++)
                {
                    //Initial set-up for the alpha to work with CrossFadeAlpha
                    if (targetAlpha >= 1)
                        graphics[i].canvasRenderer.SetAlpha(0);
                    else
                        graphics[i].canvasRenderer.SetAlpha(1);

                    //Use the CrossFadeAlpha to do fading transition
                    graphics[i].CrossFadeAlpha(targetAlpha, duration, false);
                }

            yield return new WaitForSeconds(duration);

            if (DisableOnFinish)
                target.gameObject.SetActive(false);
        }

        #endregion

        #region Coroutine for rotation transition

        /// <summary>
        /// Coroutine for animating the rotation of a target's indicator from a starting rotation to an ending rotation with a duration.
        /// </summary>
        private IEnumerator CoRotateTransition(Transform target, Quaternion startRotation, Quaternion endRotation, float duration, bool DisableOnFinish)
        {
            float ratio = 0;
            float multiplier = 1 / duration;

            target.localRotation = startRotation;

            while (target.localRotation != endRotation)
            {
                //Increment time
                ratio += Time.deltaTime * multiplier;

                // Rotations
                target.localRotation = Quaternion.Lerp(startRotation, endRotation, ratio);

                yield return null;
            }

            if (DisableOnFinish)
                target.gameObject.SetActive(false);
        }

        #endregion

    }
}