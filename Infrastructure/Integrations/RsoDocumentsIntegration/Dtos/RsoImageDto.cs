namespace Infrastructure.Integrations.RsoDocumentsIntegration.Dtos
{
    public class RsoImageDto
    {
        //Пореден номер на изображението
        public int ImgNum { get; set; }

        //Base64 encoded масив с изображението
        public string ImgData { get; set; }
    }
}
