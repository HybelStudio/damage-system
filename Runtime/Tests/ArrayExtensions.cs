namespace Hybel.DamageSystem.Tests
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Creates a new array which is a set of the original array.
        /// </summary>
        public static T[] SubSet<T>(this T[] array, int startInclusive, int endExclusive)
        {
            T[] subSet = new T[endExclusive - startInclusive];

            for (int i = 0; i < endExclusive - startInclusive; i++)
                subSet[i] = array[i + startInclusive];

            return subSet;
        }
    }
}
