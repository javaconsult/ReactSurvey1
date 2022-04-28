namespace ClassLibrary1.DTOs
{    
    /// <summary>
    /// Send back to user after successful login.
    /// Username, DisplayName, Email, Token
    /// </summary>
    public class UserDto
    {
        public LoginStatus LoginStatus { get; set; } 
        public string? DisplayName { get; set; }
        public string? Token { get; set; }
        public string? UserName { get; set; }      
        public string? Email {  get; set; }
    }
}