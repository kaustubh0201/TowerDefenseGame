using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator 
{
    public static float[,] generatePerlinNoiseMap(int width, int height, float scale){

        if (scale < 0.0001f){
            scale = 0.0001f;
        }

        float[,] noisemap = new float[width, height];

        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                noisemap[x, y] = Mathf.PerlinNoise(x / scale, y / scale); 
            }
        }

        return noisemap;

    }


    public static float[] generatePerlinNoise1D(int len, float scale){
        if (scale < 0.0001f){
            scale = 0.0001f;
        }

        float[] noisemap = new float[len];

        for(int i = 0; i < len; i++){
            noisemap[i] = Mathf.PerlinNoise(i / scale, 0.0f); 
        }

        return noisemap;


    }
}
