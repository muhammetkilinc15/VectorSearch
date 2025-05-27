using System;

namespace VectorSearchWithMssql.Helper
{
    public class SimilartyCalculater
    {
        public static double CalculateCosineSimilarity(float[] vector1, float[] vector2)
        {
            if (vector1 == null || vector2 == null)
                throw new ArgumentNullException("Vektörler null olamaz.");

            if (vector1.Length != vector2.Length)
                throw new ArgumentException("Vektörlerin uzunlukları aynı olmalı.");

            double dotProduct = 0.0;
            double magnitude1 = 0.0;
            double magnitude2 = 0.0;

            for (int i = 0; i < vector1.Length; i++)
            {
                dotProduct += vector1[i] * vector2[i];
                magnitude1 += vector1[i] * vector1[i];
                magnitude2 += vector2[i] * vector2[i];
            }

            if (magnitude1 == 0 || magnitude2 == 0)
                return 0; // Sıfır vektör ise similarity 0 kabul edelim

            return dotProduct / (Math.Sqrt(magnitude1) * Math.Sqrt(magnitude2));
        }

        public static double CalculateEuclideanDistance(float[] vector1, float[] vector2)
        {
            if (vector1 == null || vector2 == null)
                throw new ArgumentNullException("Vektörler null olamaz.");

            if (vector1.Length != vector2.Length)
                throw new ArgumentException("Vektörlerin uzunlukları aynı olmalı.");

            double sum = 0.0;

            for (int i = 0; i < vector1.Length; i++)
            {
                double difference = vector1[i] - vector2[i];
                sum += difference * difference;
            }

            return Math.Sqrt(sum);
        }

        public static double CalculateManhattanDistance(float[] vector1, float[] vector2)
        {
            if (vector1 == null || vector2 == null)
                throw new ArgumentNullException("Vektörler null olamaz.");
            if (vector1.Length != vector2.Length)
                throw new ArgumentException("Vektörlerin uzunlukları aynı olmalı.");
            double sum = 0.0;
            for (int i = 0; i < vector1.Length; i++)
            {
                sum += Math.Abs(vector1[i] - vector2[i]);
            }
            return sum;
        }
    }
}
