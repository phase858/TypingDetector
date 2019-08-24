using Discord;
using Discord.WebSocket;
using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TypingDetector
{
    public class Program
    {

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += MessageReceived;

            var token = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "token.txt"));
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {

            if (message.Author.IsBot) { return; }
            var pattern = @"<@\d+>";
            var processed = Regex.Replace(message.Content, pattern, "");
            Console.WriteLine(message.Content);
            Console.WriteLine(processed);
            var warn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "warn.png");
            if (!string.IsNullOrWhiteSpace(processed))
            {
                await message.Channel.SendFileAsync(warn, message.Author.Mention);
                //await message.Channel.SendMessageAsync("No typing");
            }
            
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}