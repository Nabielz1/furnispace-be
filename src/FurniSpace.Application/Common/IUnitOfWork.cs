namespace FurniSpace.Application.Common;

public interface IUnitOfWork
{
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
