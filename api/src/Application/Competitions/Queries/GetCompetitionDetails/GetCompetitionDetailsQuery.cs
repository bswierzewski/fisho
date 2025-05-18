using Fishio.Application.Competitions.Queries.GetOpenCompetitions;

namespace Fishio.Application.Competitions.Queries.GetCompetitionDetails;

public class GetCompetitionDetailsQuery : IRequest<CompetitionDetailsDto?>
{
    public int Id { get; set; }

    public GetCompetitionDetailsQuery(int id)
    {
        Id = id;
    }
}
