using HandyControl.Controls;
using HandyControl.Data;
using Octokit;
using System.Diagnostics;
using System.Reflection;

namespace GlitchCode.Functionalities.Func
{
    class UpdateChecker : IFuncionality
    {
        static GitHubClient client = new GitHubClient(new ProductHeaderValue("GlitchCode"));

        Release latestRelease = client.Repository.Release.GetAll("MichixYT", "GlitchCode").Result[0];

        public void onEnable()
        {
            string releaseVersion = latestRelease.TagName.Replace("-Dev", "").Replace(".", "").Replace("v", "");
            if (releaseVersion.ToCharArray().Length == 3) releaseVersion += "0";
            if (int.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", "")) < int.Parse(releaseVersion))
                Growl.Ask(new GrowlInfo()
                {
                    Message = "Update found! Do you want to update?",
                    ConfirmStr = "Yes",
                    CancelStr = "No",
                    ActionBeforeClose = isConfirmed =>
                    {
                        if (!isConfirmed) return true;
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = latestRelease.HtmlUrl,
                            UseShellExecute = true
                        });
                        return true;
                    }
                });
        }
    }
}
