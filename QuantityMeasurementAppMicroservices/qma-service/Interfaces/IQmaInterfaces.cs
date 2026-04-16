using System.Collections.Generic;
using QmaService.DTO;
using QmaService.Entities;

namespace QmaService.Interfaces
{
    public interface IQuantityMeasurementService
    {
        QuantityMeasurementDTO        Compare(QuantityDTO first, QuantityDTO second);
        QuantityMeasurementDTO        Convert(QuantityDTO source, object targetUnit);
        QuantityMeasurementDTO        Add(QuantityDTO first, QuantityDTO second, object targetUnit);
        QuantityMeasurementDTO        Subtract(QuantityDTO first, QuantityDTO second);
        QuantityMeasurementDTO        Divide(QuantityDTO first, QuantityDTO second);
        List<QuantityMeasurementDTO>  GetAllMeasurements();
        List<QuantityMeasurementDTO>  GetByOperation(string operation);
        List<QuantityMeasurementDTO>  GetByMeasureType(string measureType);
        List<QuantityMeasurementDTO>  GetErrored();
        int                           GetTotalCount();
        void                          DeleteAll();
    }

    public interface IQuantityMeasurementRepository
    {
        void                              Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity>   GetAll();
        List<QuantityMeasurementEntity>   GetByOperation(string operation);
        List<QuantityMeasurementEntity>   GetByMeasureType(string measureType);
        List<QuantityMeasurementEntity>   GetErrored();
        int                               GetTotalCount();
        void                              DeleteAll();
    }

    // Extended interface for Redis-specific operations
    public interface IRedisQuantityRepository : IQuantityMeasurementRepository { }
}
