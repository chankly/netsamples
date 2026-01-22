using Casbin;
using HSoft.AnotherCasbin.Api.Domain;
using HSoft.AnotherCasbin.Api.Domain.Security;

namespace HSoft.AnotherCasbin.Api.Services
{
    public class MatchFilterService
    {
        private readonly Enforcer _enforcer;
        private readonly List<Sale> _userSales;

        public MatchFilterService(List<Sale> userSales, IWebHostEnvironment webHostEnvironment)
        {
            _userSales = userSales ?? throw new ArgumentNullException(nameof(userSales));
            //_enforcer = new Enforcer("model.conf", "policy.csv");

            var modelFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Infrastructure", "Security", "model.conf");
            _enforcer = new Enforcer(modelFilePath);
            LoadPolicies();
        }

        private void LoadPolicies()
        {
            // Clear existing policies
            _enforcer.ClearPolicy();

            // Add policies based on user's sales
            foreach (var sale in _userSales)
            {
                _enforcer.AddPolicy(
                    "user", // subject
                    sale.SeasonId.ToString(),
                    sale.CompetitionId.ToString(),
                    sale.TeamId?.ToString() ?? "",
                    sale.MatchId?.ToString() ?? "");
            }
        }

        public List<Match> FilterMatches(List<Match> allMatches)
        {
            var permissions = _enforcer.GetPermissionsForUser("user");

            // Step 1: Filter matches based on access rules
            var accessibleMatches = allMatches.Where(match => _enforcer.Enforce("user", new MatchWrapper(match))).ToList();

            // Step 2: Calculate allowed assets intersection from all sales
            var allowedAssets = GetAllowedAssetsIntersection();

            // Step 3: Filter assets in each accessible match
            foreach (var match in accessibleMatches)
            {
                match.Assets = match.Assets
                    .Where(asset => allowedAssets.Contains(asset))
                    .ToList();
            }

            return accessibleMatches;
        }

        private HashSet<string> GetAllowedAssetsIntersection()
        {
            if (_userSales.Count == 0) return new HashSet<string>();

            // Start with all assets from first sale
            var intersection = new HashSet<string>(_userSales[0].Assets);

            // Intersect with assets from other sales
            for (int i = 1; i < _userSales.Count; i++)
            {
                intersection.IntersectWith(_userSales[i].Assets);
            }

            return intersection;
        }
    }
}
