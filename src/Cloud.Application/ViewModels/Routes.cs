namespace Cloud.Application.ViewModels;

public static class Routes
{
    public const string Root = "/";

    public static class Home
    {
        public const string Base = "/";
        public const string Index = Base;
    }

    public static class Authentication
    {
        public const string Base = $"/auth";
        public const string Login = $"{Base}/login";
        public const string Logout = $"{Base}/logout";
        public const string Register = $"{Base}/register";
    }
}
