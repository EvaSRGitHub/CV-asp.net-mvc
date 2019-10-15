namespace CVApp.Common.Services.Contracts
{
    public interface ICloudinaryService
    {
        string PublicId { get; }

        string Upload(string filePath);

        string DeleteCloudinaryImg(string publicId);
    }
}
