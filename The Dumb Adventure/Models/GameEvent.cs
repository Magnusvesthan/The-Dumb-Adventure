namespace SurvivalGame.Models
{
    public class GameEvent
    {
        public required string Scenario { get; set; }
        public required List<EventOption> Options { get; set; }
    }
}