namespace Infrastructure.AppSettings
{
	public class FileStorageSettings
	{
		public int Id { get; set; }
		public string ConnectionString { get; set; }
		public string Provider { get; set; }
		public bool IsOperative { get; set; }
	}
}
