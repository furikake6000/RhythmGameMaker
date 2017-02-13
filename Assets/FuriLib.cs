using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuriLib
{
    class MyMathf
    {
        /// <summary>
        /// UnityEngine.Mathf.Lerpの修正。tが0～1以外でも比例した値を返す。
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation value between the two floats.</param>
        /// <returns></returns>
        public static float LerpUnlimited(float a, float b, float t)
        {
            return (a + (b - a) * t);
        }
    }
}