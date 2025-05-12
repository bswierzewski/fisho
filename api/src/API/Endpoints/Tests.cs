using System.Diagnostics.Contracts;
using Web.Infrastructure;

namespace API.Endpoints
{
    public class Tests : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapGet(GetInts);
        }

        public async Task<IEnumerable<int>> GetInts(ISender sender)
        {
            return await Task.FromResult(new int[] { 1, 2, 3, 4, 5 });
        }
    }
}
