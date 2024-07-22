namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a JSON Web Token (JWT) and its expiration time.
    /// </summary>
    public class JwtDto
    {
        /// <summary>
        /// Gets or sets the JWT token string.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the JWT token.
        /// </summary>
        public DateTime Expire { get; set; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtDto"/> class with the specified token and expiration time.
        /// </summary>
        /// <param name="token">The JWT token string.</param>
        /// <param name="expire">The expiration date and time of the JWT token.</param>
        public JwtDto(string token, DateTime expire)
        {
            Token = token;
            Expire = expire;
        }
    }
}