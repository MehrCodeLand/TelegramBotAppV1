using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotAppV1
{
    public class Home
    {
        struct BotUpdate
        {
            public string text;
            public long id;
            public string? username;
        }

        static TelegramBotClient Bot = new TelegramBotClient("");
        static List<BotUpdate> BotUpdates = new List<BotUpdate>();

        static string fileName = @"C:\Users\Mehrshad\update.json";
        public static void Main()
        {
            // Read All Saved File

            try
            {
                var botUpdatetestStr = System.IO.File.ReadAllText(fileName);

                BotUpdates = JsonConvert.DeserializeObject<List<BotUpdate>>(botUpdatetestStr) ??
                    BotUpdates; 
            }catch(Exception ex)
            {
                Console.WriteLine($"Error {ex}");
            }

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                }
            };

            Bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);

            Console.ReadKey();
                    
        }

        private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            if(update.Type == UpdateType.Message)
            {
                if(update.Message.Type == MessageType.Text)
                {
                    var _botUpdate = new BotUpdate
                    {
                        text = update.Message.Text,
                        id = update.Message.Chat.Id,
                        username = update.Message.Chat.Username
                    };


                    //Console.WriteLine($"{username} | {id} | {text}");

                    BotUpdates.Add(_botUpdate);

                    var botUpdateStr = JsonConvert.SerializeObject(BotUpdates, Formatting.Indented);

                    // write an update
                    System.IO.File.WriteAllText(fileName, botUpdateStr);
                }
            }
        }
    }
}
