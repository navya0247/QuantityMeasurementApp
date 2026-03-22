using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    /// <summary>Data access contract for user authentication persistence.</summary>
    public interface IUserRepository
    {
        void Save(UserEntity user);
        UserEntity FindByEmail(string email);
        bool ExistsByEmail(string email);
        void UpdateLastLogin(string email);
    }
}
