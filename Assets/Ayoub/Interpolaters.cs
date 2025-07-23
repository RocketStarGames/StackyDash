using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//easing
namespace AyoubInterpolaters {
    public static class Interpolate {

        public static float Map01(this float t, float min, float max) {
            float val = t - min;
            return val / (max - min);
        }
        private static string warningMsg = "[float] Interpolaters expect a 0-1 input range: t=";

        public static float ISmoothStart2(this float t) {
           // CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return t * t;
        }
        public static float ISmoothStart3(this float t) {
          //  CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return t * t * t;
        }
        public static float ISmoothStart4(this float t) {
           // CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return t * t * t * t;
        }
        public static float ISmoothStop2(this float t) {
           // CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return 1 - ((1 - t) * (1 - t));
        }
        public static float ISmoothStop3(this float t) {
//CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return 1 - ((1 - t) * (1 - t) * (1 - t));
        }
        public static float ISmoothStop4(this float t) {
          //  CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return 1 - ((1 - t) * (1 - t) * (1 - t) * (1 - t));
        }
        public static float IMix(this float t, float a, float b, float w) {
            return (1 - w) * a + (w) * b;
        }
        public static float ICrossfade(this float t, float a, float b) {
           // CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return t.IMix(a, b, t);
        }
        public static float Invert(this float t) {
           // CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return 1 - t;
        }
        public static float Scale(this float t, float f) {
            return t * f;
        }
        public static float ReverseScale(this float t, float f) {
            return t.Invert() * f;
        }
        public static float IArch(this float t) {
           // CDebug.LogWarningIf(t > 1 || t < 0, warningMsg + t);
            return (t * (1 - t)) * 4;
        }
        public static float IBell4(this float t) {
            float c = (1 - t) * (t * t) + t * (1 - (1 - t) * (1 - t));
            return (c * (1 - c)) * 4;
        }
        public static float IBell6(this float t) {
            return t.ISmoothStop3() * t.ISmoothStart3();
        }
        public static float IEase2(this float t) {
            return t.IMix(t.ISmoothStart2(), t.ISmoothStop2(), t);
        }
        public static float IEase3(this float t) {
            return t.IMix(t.ISmoothStart3(), t.ISmoothStop3(), t);
        }
        public static float IEase4(this float t) {
            return t.IMix(t.ISmoothStart4(), t.ISmoothStop4(), t);
        }
        /// <summary>
        /// Interpolates 't', 0-1 before 'i' and 1-0 after 'o'.
        /// </summary>
        /// <param name="t">Factor 0-1</param>
        /// <param name="i">In  Key, 0-1 less than 'o'</param>
        /// <param name="o">Out Key, 0-1 greater than 'i'</param>
        /// <returns></returns>
        public static float IFadeIO(this float t, float i, float o) {
            return Mathf.Min(t / i, 1, (1 - t) / (1 - o));
        }
    }
}