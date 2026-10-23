namespace BaseLibrary.Respones
{
    public record GeneralResponse(bool Flag, string Message, object? Data = null);

}
