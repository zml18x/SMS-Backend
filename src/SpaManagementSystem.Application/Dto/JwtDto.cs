namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Data Transfer Object for conveying JWT information between layers of the application.
    /// This DTO contains the JWT itself and its expiration date, used primarily for handling user authentication sessions.
    /// </summary>
    public class JwtDto
    {
        /// <summary>
        /// Gets or sets the JSON Web Token (JWT) string.
        /// </summary>
        /// <value>The JWT used for authenticating user requests.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the token.
        /// </summary>
        /// <value>The date and time at which the token will no longer be valid.</value>
        public DateTime Expire { get; set; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtDto"/> class with specified token and expiration details.
        /// </summary>
        /// <param name="token">The JWT used for session authentication.</param>
        /// <param name="expire">The expiration date and time of the JWT.</param>
        public JwtDto(string token, DateTime expire)
        {
            Token = token;
            Expire = expire;
        }
    }
}