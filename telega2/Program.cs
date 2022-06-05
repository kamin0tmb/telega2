using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using Telegram.Bot;
using telega2.Controllers;
using telega2.Services;
using telega2.Configuration;


namespace telega
{
    class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Starting Service");
            await host.RunAsync();
            Console.WriteLine("Service stopped");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<IFileHandler, AudioFileHandler>();
            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
            
        }
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                DownloadsFolder = "D:\\4",
                BotToken = "5384319470:AAEu74oy_RJP7WS-5LgnJrPG93RISS-EV2U",
                AudioFileName = "audio",
                InputAudioFormat = "ogg",
            };
        }

    }
}
