using System;
using System.Collections.Generic;

namespace SimInstanceLab.Managers.Helpers
{
    public static class RandomSeedHelper
    {
        public static Random Random = new Random((int)DateTime.Now.Ticks);

        private static int _seed;
        private static int _count;


        public static int Seed
        {
            get { return _seed; }
            set
            {
                Random = new Random(value);
                _seed = value;
            }
        }

        public static void InitializeSeed(int seed)
        {
            Seed = seed;
        }

        public static int GetNextAutoIncrementalNumber()
        {
            _count++;
            return _count;
        }

        public static IEnumerable<int> GetRandomNumber()
        {
            while (true)
            {
                yield return Random.Next();
            }
        }
    }
}