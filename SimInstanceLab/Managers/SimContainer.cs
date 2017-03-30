namespace SimInstanceLab.Managers
{
    public static class SimContainer
    {
        public static ISimProvider Container { get; set; }

        public static SimMemoryProvider MemoryContainer { get; set; } = new SimMemoryProvider();

    }
}