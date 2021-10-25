using DiscordRPC;
using System.Diagnostics;

namespace GlitchCode.Managers
{
    public static class DiscordRPCManager
    {
        static DiscordRpcClient client;

        public static void Initializate(string file = "No File", string project = "No project")
        {
            client = new DiscordRpcClient("854763230080794664");
            client.SetPresence(new RichPresence()
            {
                Details = "Project: " + project,
                State = "File: " + file,
                Timestamps = Timestamps.Now,
                Buttons = new Button[]
                {
                        new Button()
                        {
                            Label = "Get GlitchCode",
                            Url = "https://github.com/MichixYT/GlitchCode"
                        }
                },
                Assets = new Assets()
                {
                    LargeImageKey = "large",
                    LargeImageText = "GlitchCode"
                }
            });
            client.Initialize();
            Debug.WriteLine(client.IsInitialized);
        }

        public static void Deinitialize()
        {
            client.Deinitialize();
        }

        public static void UpdateFile(string file)
        {
            client.UpdateState("File: " + file);
        }

        public static void UpdateProject(string project)
        {
            client.UpdateDetails("Project: " + project);
        }
    }
}
