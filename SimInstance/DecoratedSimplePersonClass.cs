namespace SimInstance
{
    public class DecoratedSimplePersonClass
    {
        [SimRegex("[A-Z][a-z]{5}")]
        public string Name { get; set; }
        [SimRegex("[A-Z][a-z]{4}")]
        public string SurName { get; set; }
        [SimRange(0,100)]
        public int Age { get; set; }
        [SimRegex("[A-Z][a-z]{5} Street")]
        public string Street { get; set; }
    }
}