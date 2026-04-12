using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.BusinessLayer.Interfaces
{
    /// <summary>
    /// Verifies a Google ID token and returns (or auto-creates) the app user.
    /// </summary>
    public interface IGoogleAuthService
    {
        /// <summary>
        /// Validates the Google ID token, finds or registers the user,
        /// and returns a signed JWT + profile just like a normal sign-in.
        /// </summary>
        Task<AuthResponseDTO> SignInWithGoogleAsync(string idToken);
    }
}
