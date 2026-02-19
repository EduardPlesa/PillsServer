namespace PillsServer.Common
{
    public class Pill
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Stock { get; set; } = 0;
        public int[] PillsToTakePerTimeOfDay { get; set; } = [0,0,0];
        public DateTime? LastTaken { get; set; }
    }
}
