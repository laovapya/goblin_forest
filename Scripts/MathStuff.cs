using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathStuff
{
    static public float perlin(float x, float y, float scale, int octaves, float lacunarity, float persistence)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0; // for clamping

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency / scale, y * frequency / scale) * amplitude;
            maxValue += amplitude;
            frequency *= lacunarity;
            amplitude *= persistence;
        }

        return total / maxValue; // clamp to 0 1 
    }
}
