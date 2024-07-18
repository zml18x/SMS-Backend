namespace SpaManagementSystem.Application.Requests.Salon
{
    public class CreateSalonRequest
    {
        public string Name { get; init; }
        public string PhoneNumber { get; init; }
        public string Email { get; init; }



        public CreateSalonRequest(string name, string phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}