namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>
    /// Sent by the frontend after a successful Google Sign-In popup.
    /// Contains the raw Google ID Token (JWT) for server-side verification.
    /// </summary>
    public class GoogleAuthDTO
    {
        /// <summary>
        /// The Google ID token returned by Google Identity Services (GIS)
        /// after the user completes the Google sign-in popup.
        /// </summary>
        public string IdToken { get; set; } = string.Empty;
    }
}
