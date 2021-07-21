using UnityEngine;

namespace InteropCall
{
    public class Program
    {
        public static void Main(Transform transform, int iterations)
        {
            for(int i = 0; i < iterations; i++)
            {
                transform.Translate(0f, 0f, 0f);
            }
        }
    }
}
