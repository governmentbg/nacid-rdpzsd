using Infrastructure.Constants;
using Infrastructure.Integrations.RndIntegration.Dtos;
using Infrastructure.User.Enums;
using System.Collections.Generic;

namespace Infrastructure.User
{
	public class UserContext
	{
		public int? UserId { get; set; }
		public string UserFullname { get; set; }
		public UserType UserType { get; set; }
		public List<string> Permissions { get; set; } = new List<string>();
		public List<string> UserEducationalQualificationPermissions { get; set; } = new List<string>();

		// Only if UserType.Rsd is <> null
		public InstitutionDto Institution { get; set; }

		public UserContext()
		{
		}

		public UserContext(int userId, string userFullname, List<string> permissions, string userType)
		{
			UserId = userId;
			UserFullname = userFullname;
			Permissions = permissions;
			UserType = ReturnUserType(userType);
		}

		public void ConstructUserEducationalQualifications()
		{
			if (UserType == UserType.Ems)
			{
				UserEducationalQualificationPermissions.AddRange(new List<string> {
					EducationalQualificationConstants.ProfessionalBachelor,
					EducationalQualificationConstants.Bachelor,
					EducationalQualificationConstants.MasterSecondary,
					EducationalQualificationConstants.MasterHigh,
					EducationalQualificationConstants.Doctor
				});
			}
			else if (UserType == UserType.Rsd)
			{
				if (Institution != null)
				{
					if (Institution.HasBachelor)
					{
						UserEducationalQualificationPermissions.AddRange(new List<string> {
							EducationalQualificationConstants.ProfessionalBachelor,
							EducationalQualificationConstants.Bachelor
						});
					}

					if (Institution.HasMaster)
					{
						UserEducationalQualificationPermissions.AddRange(new List<string> {
							EducationalQualificationConstants.MasterSecondary,
							EducationalQualificationConstants.MasterHigh
						});
					}

					if (Institution.HasDoctoral)
					{
						UserEducationalQualificationPermissions.Add(EducationalQualificationConstants.Doctor);
					}
				}
			}
		}

		private UserType ReturnUserType(string userType)
		{
			return userType switch {
				string emsString when emsString.Contains("emsUser") && (Permissions.Contains(PermissionConstants.RdpzsdRead) || Permissions.Contains(PermissionConstants.RdpzsdEdit) || Permissions.Contains(PermissionConstants.RdpzsdStickers)) => UserType.Ems,
				string rsdString when rsdString.Contains("rsdUser") => UserType.Rsd,
				_ => UserType.Unauthorized,
			};
		}
	}
}
