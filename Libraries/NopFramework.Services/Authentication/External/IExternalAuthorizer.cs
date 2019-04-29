namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 外部授权
    /// </summary>
    public partial interface IExternalAuthorizer
    {
        AuthorizationResult Authorize(OpenAuthenticationParameters parameters);
    }
}
