﻿using UnityEngine;

namespace M8.Noise.Module {
    public class Billow : ModuleBase {
        /// Maximum number of octaves for the the Billow noise module.
        public const int MAX_OCTAVE = 30;

        /// <summary>
        /// Frequency of the first octave.
        /// </summary>
        public float frequency = 1.0f;

        /// <summary>
        /// The lacunarity is the frequency multiplier between successive
        /// octaves.
        ///
        /// For best results, set the lacunarity to a number between 1.5 and
        /// 3.5.
        /// </summary>
        public float lacunarity = 2.0f;

        /// <summary>
        /// Quality of the billowy noise.
        /// </summary>
        public Quality quality = Quality.Cubic;

        /// <summary>
        /// The number of octaves controls the amount of detail in the billowy
        /// noise.
        ///
        /// The larger the number of octaves, the more time required to
        /// calculate the billowy-noise value.
        /// </summary>
        public int octaveCount { get { return mOctaveCount; } set { mOctaveCount = Mathf.Clamp(value, 1, MAX_OCTAVE); } }

        /// <summary>
        /// The persistence value controls the roughness of the billowy noise.
        ///
        /// For best results, set the persistence value to a number between
        /// 0.0 and 1.0.
        /// </summary>
        public float persistence = 0.5f;

        /// <summary>
        /// The seed value used by the billowy-noise function.
        /// </summary>
        public int seed = 0;

        public override float GetValue(float x, float y, float z) {
            float value = 0.0f;
            float signal = 0.0f;
            float curPersistence = 1.0f;
            long _seed;

            x *= frequency;
            y *= frequency;
            z *= frequency;

            for(int curOctave = 0; curOctave < mOctaveCount; curOctave++) {
                // Get the coherent-noise value from the input value and add it to the
                // final result.
                _seed = (seed + curOctave) & 0xffffffff;
                signal = Generate.GradientCoherent3D(x, y, z, (int)_seed, quality);
                signal = 2.0f * Mathf.Abs(signal) - 1.0f;
                value += signal * curPersistence;

                // Prepare the next octave.
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
                curPersistence *= persistence;
            }
            value += 0.5f;

            return value;
        }

        protected int mOctaveCount = 6;
    }
}