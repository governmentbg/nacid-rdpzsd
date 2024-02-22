using Infrastructure.Integrations.EmsIntegration.Dtos;
using Rdpzsd.Models.Models.Rdpzsd;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Lot
{
    public class PersonLotActionDto
    {
        public PersonLotAction PersonLotAction { get; set; }
        public string UserEmail { get; set; }
        public string UserType { get; set; }

        public PersonLotActionDto(PersonLotAction personLotAction, List<UserEmsDto> emsUsers)
        {
            var user = emsUsers.Single(e => e.Id == personLotAction.UserId);

            PersonLotAction = personLotAction;
            UserEmail = user.Email;
            UserType = user.UserType;
        }
    }

    public static class PersonLotActionDtoExtensions
    {
        public static List<PersonLotActionDto> ToPersonLotActionDto(this List<PersonLotAction> personLotActions, List<UserEmsDto> userEmsDtos)
        {
            return personLotActions.Select(e => new PersonLotActionDto(e, userEmsDtos)).ToList();
        }
    }
}
