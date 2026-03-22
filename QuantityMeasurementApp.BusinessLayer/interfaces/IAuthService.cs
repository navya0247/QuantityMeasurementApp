using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.BusinessLayer.Interfaces
{
    /// <summary>Contract for user authentication — signup, signin, profile.</summary>
    public interface IAuthService
    {
        UserProfileDTO  SignUp(SignUpDTO dto);
        AuthResponseDTO SignIn(SignInDTO dto);
        UserProfileDTO  GetProfile(string email);
    }
}
