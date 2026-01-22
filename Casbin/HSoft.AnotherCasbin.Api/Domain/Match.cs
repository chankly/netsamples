namespace HSoft.AnotherCasbin.Api.Domain
{
    public class Match
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Guid CompetitionId { get; set; }
        public Guid TeamHomeId { get; set; }
        public Guid TeamAwayId { get; set; }
        public List<string> Assets { get; set; } = new List<string>();
    }
}
