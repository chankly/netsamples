namespace HSoft.AnotherCasbin.Api.Domain
{
    public class Sale
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Guid CompetitionId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? MatchId { get; set; }
        public List<string> Assets { get; set; }
    }
}
