using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Requests.Salon
{
    public class UpdateOpeningHoursRequest
    {
        public List<OpeningHoursDto> OpeningHours { get; set; }



        public UpdateOpeningHoursRequest(List<OpeningHoursDto> openingHours)
        {
            OpeningHours = openingHours;
        }
    }
}