using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    /// <summary>
    /// Marker interface for Redis-specific quantity repository.
    /// Extends IQuantityMeasurementRepository — inherits all CRUD contracts.
    /// Used for dependency injection to distinguish Redis from SQL Server repo.
    /// </summary>
    public interface IRedisQuantityRepository : IQuantityMeasurementRepository
    {
        // All methods inherited from IQuantityMeasurementRepository:
        // Save, GetAll, GetByOperation, GetByMeasureType, GetErrored, GetTotalCount, DeleteAll
    }
}
