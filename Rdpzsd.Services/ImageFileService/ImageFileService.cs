using FileStorageNetCore.Api;
using System;
using System.Threading.Tasks;

namespace Rdpzsd.Services
{
    public class ImageFileService
    {
		private readonly BlobStorageService fileStorageRepository;

		public ImageFileService(
			BlobStorageService fileStorageRepository)
		{
			this.fileStorageRepository = fileStorageRepository;
		}

		public async Task<string> GetBase64ImageUrlAsync(Guid key, int dbId)
		{
			var image = await fileStorageRepository.GetBytes(key, dbId);
			string base64ImageUrl;

			if (image != null)
			{
				base64ImageUrl = Convert.ToBase64String(image);
			}
			else
			{
				return null;
			}

			return base64ImageUrl;
		}
	}
}
