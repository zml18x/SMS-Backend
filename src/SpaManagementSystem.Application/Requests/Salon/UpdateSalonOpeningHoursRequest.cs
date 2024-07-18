using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Requests.Salon
{
    public class UpdateSalonOpeningHoursRequest
    {
        public List<OpeningHoursDto> OpeningHours { get; set; }



        public UpdateSalonOpeningHoursRequest(List<OpeningHoursDto> openingHours)
        {
            OpeningHours = openingHours;
        }
    }
}