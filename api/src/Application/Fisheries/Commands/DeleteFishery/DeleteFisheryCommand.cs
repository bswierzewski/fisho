namespace Fishio.Application.Fisheries.Commands.DeleteFishery;

public record DeleteFisheryCommand(int Id) : IRequest<Unit>;
