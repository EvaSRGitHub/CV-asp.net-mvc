namespace CVApp.Common.GeneratePDF.Contracts
{
    public interface IHtmlToPdfConverter
    {
        byte[] Convert(string basePath, string htmlCode, FormatType formatType, OrientationType orientationType);
    }
}
