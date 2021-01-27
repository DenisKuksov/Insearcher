using UnityEngine;

public class ColorWorker
{   

    public int CalculateColorDistance(Color32 st, Color32 nd)
    {
        int difference = st.r - nd.r + st.g - nd.g + st.b - nd.b;
        return difference < 0 ? -difference : difference;
    }

    public Color32 RandomizeColorBetween(int min, int max)
    {
        return new Color32((byte)Random.Range(min, max), (byte)Random.Range(min, max), (byte)Random.Range(min, max), 255);
    }

    public void SetRandomizedColorBetween(int min, int max, ref Color32 color)
    {
        color.r = (byte)Random.Range(min, max);
        color.g = (byte)Random.Range(min, max);
        color.b = (byte)Random.Range(min, max);
    }

    public Color32 CalculateInversedColor(Color32 color)
    {
        color.r = (byte)(255 - color.r);
        color.g = (byte)(255 - color.g); 
        color.b = (byte)(255 - color.b);
        return color;
    }

    public Color32 CalculateMidColor(Color32 st, Color32 nd)
    {
        return new Color32((byte)((st.r + st.r) * 0.5f), (byte)((st.r + st.r) * 0.5f), (byte)((st.r + st.r) * 0.5f), 255);
    }

    public void SetMidColor(Color32 st, Color32 nd, ref Color32 color)
    {
        color.r = (byte)((st.r + st.r) * 0.5f);
        color.g = (byte)((st.g + st.g) * 0.5f);
        color.b = (byte)((st.b + st.b) * 0.5f);
    }

    public Color32 CalculateMidColorWithWeights(Color32 st, Color32 nd, float nd_weight)
    {
        Color32 result = st;
        result.r = (byte)Mathf.Abs(nd_weight * (st.r - nd.r) - st.r);
        result.g = (byte)Mathf.Abs(nd_weight * (st.g - nd.g) - st.g);
        result.b = (byte)Mathf.Abs(nd_weight * (st.b - nd.b) - st.b);

        /* Red for ex (IM DOVNICH, SEE BETTER ANOTHER METHOD)
         *  st.r = 10; stw = 0.1 => ndw = 1 - 0.1
         *  nd.r = 20; ndw = 0.9
         *
         *  st.r = abs(0.9 * (10 - 20) - 10) = abs(-9 - 10) = 19
         *  
         *  -----------------------------------------------------
         *  
         *  st.r = 20; stw = 0.1 => ndw = 1 - 0.1
         *  nd.r = 10; ndw = 0.9
         *  
         *  st.r = abs(0.9 * (20 - 10) - 20) = abs(9 - 20) = 11
         */
        
        return result;
    }

    public Color32 CalculateMidColorWithWeights(Color[] colors, float[] weights)
    {
        Color result = new Color(0, 0, 0, 1);

        for(int i = 0; i < colors.Length; i++)
        {
            result.r += (colors[i].r * weights[i]);
            result.g += (colors[i].g * weights[i]);
            result.b += (colors[i].b * weights[i]);
        }

        //result.r *= 255f;
       //result.g *= 255f;
        //result.b *= 255f;

        return result;
    }

}
 