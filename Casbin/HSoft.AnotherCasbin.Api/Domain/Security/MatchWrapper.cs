namespace HSoft.AnotherCasbin.Api.Domain.Security
{
    public class MatchWrapper
    {

        private readonly Match _match;

        public MatchWrapper(Match match)
        {
            _match = match;
        }

        public string Id => _match.Id.ToString();
        public string SeasonId => _match.SeasonId.ToString();
        public string CompetitionId => _match.CompetitionId.ToString();
        public string TeamHomeId => _match.TeamHomeId.ToString();
        public string TeamAwayId => _match.TeamAwayId.ToString();
    }
}
