using Bot.Models;
using Newtonsoft.Json;
using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    public static ITelegramBotClient bot = new TelegramBotClient("6098355720:AAEX9G4Bh4z8Y6cYrxSAHr29Cfy5udmSpUc");
    private static Dictionary<long, string> userStates = new Dictionary<long, string>();

    static void Main(string[] args)
    {
        Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };
        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );

        Console.ReadLine();
    }
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text != null)
                {
                    if (message.Text.ToLower() == "/start")
                    {
                        await HandleStartCommand(botClient, message);
                    }
                    else if (message.Text.ToLower() == "/ilikefootball")
                    {
                        await HandleILikeFootballCommand(botClient, message);
                    }
                    else if (message.Text.ToLower() == "інформація про тренера" || message.Text.ToLower() == "/coachinformation")
                    {
                        await HandleFootballCoachCommand(botClient, message);
                    }
                    else if (message.Text.ToLower() == "прапор країни уєфа" || message.Text.ToLower() == "/flag")
                    {
                        await HandleFootballCountriesCommand(botClient, message);
                    }
                    else if (message.Text.ToLower() == "розклад футбольних матчів на сьогодні" || message.Text.ToLower() == "/schedulefortoday")
                    {
                        await HandlePredictionInput(botClient, message);
                    }
                    else if (message.Text.ToLower() == "серія a, b та c")
                    {
                        await HandlePredictionItalyInput(botClient, message);
                    }
                    else if (message.Text.ToLower() == "ліга 1, 2 та 3")
                    {
                        await HandlePredictionFranceInput(botClient, message);
                    }
                    else if (message.Text.ToLower() == "iнформація про команду" || message.Text.ToLower() == "/teaminformation")
                    {
                        await HandleFootballInformationCommand(botClient, message);
                    }
                    else if (message.Text.ToLower() == "місця проведення футбольних матчів" || message.Text.ToLower() == "/venue")
                    {
                        await HandleFootballVenuesCommand(botClient, message);
                    }
                    else if (userStates.ContainsKey(message.Chat.Id))
                    {
                        string state = userStates[message.Chat.Id];
                        if (state == "Напиши тренера")
                        {
                            await HandleCoachInput(botClient, message);
                        }
                        else if (state == "Напиши країну УЄФА")
                        {
                            await HandleCountriesInput(botClient, message);
                        }
                        else if (state == "Напиши команду")
                        {
                            await HandleInformationInput(botClient, message);
                        }
                        if (state == "Напиши місто")
                        {
                            await HandleVenuesInput(botClient, message);
                        }
                    }
                    else if (message.Voice != null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "Я вас не розумію");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "Перепрошую, але Ваш текст не зрозумілий, \nповерніться до головного функціоналу, натиснувши на /ilikefootball");
                    }
                }
                else if (message.Voice != null)
                {
                    await botClient.SendTextMessageAsync(message.Chat, text: "Я не розумію голосових повідомленнь.\nПрошу відправте текст!");
                }
                else if (message.Document != null)
                {
                    await botClient.SendTextMessageAsync(message.Chat, text: "Я не розумію документів.\nПрошу відправте текст!");
                }
                else if (message.Photo != null)
                {
                    await botClient.SendTextMessageAsync(message.Chat, text: "Я не розумію фотографій.\nПрошу відправте текст!");
                }
                else if (message.Video  != null) 
                {
                    await botClient.SendTextMessageAsync(message.Chat, text: "Я не розумію відео.\nПрошу відправте текст!");
                }
                else if (message.Audio != null)
                {
                    await botClient.SendTextMessageAsync(message.Chat, text: "Я не розумію музики.\nПрошу відправте текст!");
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat, text: "Я не розумію.\nПрошу відправте текст!");
                }
            }
        }
        catch(FormatException)
        {
            await Console.Out.WriteLineAsync("ff");
        }
    }

    private static Task HandleVoiceMessage(ITelegramBotClient botClient, Message message)
    {
        throw new NotImplementedException();
    }

    /*Початок*/
    public static async Task HandleStartCommand(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendTextMessageAsync(message.Chat, text: "Вітаю у футбольному боті!\nДля продовження напиши /ilikefootball\n\nЯкщо ти заплутаєшся,\nто клікай на меню у лівому нижньому куті!\nПриємного користування!");
    }
    /*Інструкція*/
    public static async Task HandleILikeFootballCommand(ITelegramBotClient botClient, Message message)
    {
        ReplyKeyboardMarkup reply = new(
            new[]
            {
                new KeyboardButton[] {"Місця проведення футбольних матчів"},
                new KeyboardButton[] {"Iнформація про команду"},
                new KeyboardButton[] {"Розклад футбольних матчів на сьогодні"},
                new KeyboardButton[] {"Інформація про тренера"},
                new KeyboardButton[] {"Прапор країни УЄФА"}
            }
        )
        {
            ResizeKeyboard = true
        };
        await botClient.SendTextMessageAsync(message.Chat, "Оберіть, що Вам треба", replyMarkup: reply);
    }
    /*coach*/
    public static async Task HandleFootballCoachCommand(ITelegramBotClient botClient, Message message)
    {
        userStates[message.Chat.Id] = "Напиши тренера";
        await botClient.SendTextMessageAsync(message.Chat, "Щоб дізнатися інформацію про тренера,\nнапиши його прізвище англійською мовою");
    }
    public static async Task HandleCoachInput(ITelegramBotClient botClient, Message message)
    {
        string Name = message.Text;
        var httpClient = new HttpClient();
        var url = $"https://localhost:7223/Coach?Name={Name}";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Coach>(responseContent);
        if (result.response.Count > 0)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Ім'я : {result.response[0].firstname} {result.response[0].lastname}\nВік : {result.response[0].age}" +
                $"\nНаціональність : {result.response[0].nationality}\n");
            await botClient.SendTextMessageAsync(message.Chat, $"{result.response[0].photo}");
            string connectionString = "Host=localhost;Username=postgres;Password=root;Database=postgres";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = "CREATE TABLE IF NOT EXISTS coach (id SERIAL PRIMARY KEY, lastname VARCHAR(50) UNIQUE, age VARCHAR(50), nationality VARCHAR(50))";
                using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string insertDataQuery = @"
                        INSERT INTO coach (lastname,age,nationality) 
                        VALUES (@lastname, @age, @nationality)
                        ON CONFLICT (lastname) DO NOTHING";
                using (NpgsqlCommand command = new NpgsqlCommand(insertDataQuery, connection))
                {
                    command.Parameters.AddWithValue("lastname", result.response[0].lastname);
                    command.Parameters.AddWithValue("age", result.response[0].age);
                    command.Parameters.AddWithValue("nationality", result.response[0].nationality);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat, "Тренера не знайдено.");
        }
        userStates.Remove(message.Chat.Id);
    }
    /*Countries*/
    private static async Task HandleFootballCountriesCommand(ITelegramBotClient botClient, Message message)
    {
        userStates[message.Chat.Id] = "Напиши країну УЄФА";
        await botClient.SendTextMessageAsync(message.Chat, "Щоб дізнатися прапор країни,\nнапиши її назву англійською мовою");
    }
    private static async Task HandleCountriesInput(ITelegramBotClient botClient, Message message)
    {
        string Name = message.Text;
        var httpClient = new HttpClient();
        var url = $"https://localhost:7223/Countries?Name={Name}";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Countries>(responseContent);
        if (result.response.Count > 0)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Країна : {result.response[0].name}");
            await botClient.SendTextMessageAsync(message.Chat, $"{result.response[0].flag}");

            string connectionString = "Host=localhost;Username=postgres;Password=root;Database=postgres";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = "CREATE TABLE IF NOT EXISTS country (id SERIAL PRIMARY KEY, name VARCHAR(50) UNIQUE, flag VARCHAR(100))";
                using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string insertDataQuery = @"
                    INSERT INTO country (name, flag) 
                    VALUES (@name, @flag)
                    ON CONFLICT (name) DO NOTHING";
                using (NpgsqlCommand command = new NpgsqlCommand(insertDataQuery, connection))
                {
                    command.Parameters.AddWithValue("name", result.response[0].name);
                    command.Parameters.AddWithValue("flag", result.response[0].flag);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat, "Країну не знайдено.");
        }
        userStates.Remove(message.Chat.Id);
    }
    /*Prediction*/
    private static async Task HandlePredictionInput(ITelegramBotClient botClient, Message message)
    {
        ReplyKeyboardMarkup reply = new(
            new[]
            {
                new KeyboardButton[] {"Серія A, B та C"},
                new KeyboardButton[] {"Ліга 1, 2 та 3"}
            }
        )
        {
            ResizeKeyboard = true
        };
        await botClient.SendTextMessageAsync(message.Chat, "Оберіть лігу", replyMarkup: reply);
    }
    private static async Task HandlePredictionFranceInput(ITelegramBotClient botClient, Message message)
    {
        DateTime today = DateTime.Today;
        string formattedDate = today.ToString("yyyy-MM-dd");
        using (var httpClient = new HttpClient())
        {
            var url = $"https://localhost:7223/Prediction?from={formattedDate}&to={formattedDate}&country_id=3";
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ModelPrediction>>(responseContent);
                string scheduleText = "Розклад:\n\n";
                foreach (var prediction in result)
                {
                    scheduleText += $"Ліга : {prediction.league_name}\n";
                    scheduleText += $"Час : {prediction.match_time}\n";
                    scheduleText += $"Грають : {prediction.match_hometeam_name} - {prediction.match_awayteam_name} \n";
                    scheduleText += $"Рахунок : ({prediction.match_hometeam_score}) - ({prediction.match_awayteam_score})\n";
                    scheduleText += $"Стадіон : {prediction.match_stadium}\n\n";
                    string connectionString = "Host=localhost;Username=postgres;Password=root;Database=postgres";

                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        string createTableQuery = "CREATE TABLE IF NOT EXISTS franceLiga (id SERIAL PRIMARY KEY, liga VARCHAR(50), time VARCHAR(10), hometeam VARCHAR(50) UNIQUE, awayteam VARCHAR(50), date VARCHAR(15))";
                        using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        string insertDataQuery = @"
                        INSERT INTO franceLiga (liga, time, hometeam, awayteam, date) 
                        VALUES (@liga, @time, @hometeam, @awayteam, @date)
                        ON CONFLICT (hometeam) DO NOTHING";
                        using (NpgsqlCommand command = new NpgsqlCommand(insertDataQuery, connection))
                        {
                            command.Parameters.AddWithValue("liga", prediction.league_name);
                            command.Parameters.AddWithValue("time", prediction.match_time);
                            command.Parameters.AddWithValue("hometeam", prediction.match_hometeam_name);
                            command.Parameters.AddWithValue("awayteam", prediction.match_awayteam_name);
                            command.Parameters.AddWithValue("date", prediction.match_date);
                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }
                }
                await botClient.SendTextMessageAsync(message.Chat, scheduleText);
                await botClient.SendTextMessageAsync(message.Chat, "Щоб повернутися до головного функціоналу на тискай на\n/ilikefootball");
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat, "Сьогодні матчів немає.");
                Console.WriteLine(ex.ToString());
            }
        }
    }
    private static async Task HandlePredictionItalyInput(ITelegramBotClient botClient, Message message)
        {
        DateTime today = DateTime.Today;
        string formattedDate = today.ToString("yyyy-MM-dd");
        using (var httpClient = new HttpClient())
        {
            var url = $"https://localhost:7223/Prediction?from={formattedDate}&to={formattedDate}&country_id=5";
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ModelPrediction>>(responseContent);
                string scheduleText = "Розклад:\n\n";
                foreach (var prediction in result)
                {
                    scheduleText += $"Ліга : {prediction.league_name}\n";
                    scheduleText += $"Час : {prediction.match_time}\n";
                    scheduleText += $"Грають : {prediction.match_hometeam_name} - {prediction.match_awayteam_name} \n";
                    scheduleText += $"Рахунок : ({prediction.match_hometeam_score}) - ({prediction.match_awayteam_score})\n";
                    scheduleText += $"Стадіон : {prediction.match_stadium}\n\n";
                    string connectionString = "Host=localhost;Username=postgres;Password=root;Database=postgres";

                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        string createTableQuery = "CREATE TABLE IF NOT EXISTS italyLiga (id SERIAL PRIMARY KEY, liga VARCHAR(50), time VARCHAR(50), hometeam VARCHAR(50) UNIQUE, awayteam VARCHAR(50), date VARCHAR(50))";
                        using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        string insertDataQuery = @"
                        INSERT INTO italyLiga (liga, time, hometeam, awayteam, date) 
                        VALUES (@liga, @time, @hometeam, @awayteam, @date)
                        ON CONFLICT (hometeam) DO NOTHING";
                        using (NpgsqlCommand command = new NpgsqlCommand(insertDataQuery, connection))
                        {
                            command.Parameters.AddWithValue("liga", prediction.league_name);
                            command.Parameters.AddWithValue("time", prediction.match_time);
                            command.Parameters.AddWithValue("hometeam", prediction.match_hometeam_name);
                            command.Parameters.AddWithValue("awayteam", prediction.match_awayteam_name);
                            command.Parameters.AddWithValue("date", prediction.match_date);
                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }
                }
                await botClient.SendTextMessageAsync(message.Chat, scheduleText);
                await botClient.SendTextMessageAsync(message.Chat, "Щоб повернутися до головного функціоналу на тискай на\n/ilikefootball");
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat, "Сьогодні матчів немає.");
                Console.WriteLine(ex.ToString());
            }
        }
    }
    /*TeamInformation*/
    public static async Task HandleFootballInformationCommand(ITelegramBotClient botClient, Message message)
    {
        userStates[message.Chat.Id] = "Напиши команду";
        await botClient.SendTextMessageAsync(message.Chat, "Щоб дізнатися інформацію про команду,\nнапиши її назву англійською мовою");
    }
    public static async Task HandleInformationInput(ITelegramBotClient botClient, Message message)
    {
        string teamName = message.Text;

        var httpClient = new HttpClient();
        var url = $"https://localhost:7223/TeamsInformation?Name={Uri.EscapeDataString(teamName)}";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TeamInformation>(responseContent);
        if (result.Response.Length > 0)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Назва : {result.Response[0].Team.Name}\nКраїна : {result.Response[0].Team.Country}\nРік заснування : " +
                $"{result.Response[0].Team.Founded}");
            await botClient.SendTextMessageAsync(message.Chat, $"{result.Response[0].Team.Logo}");
            await botClient.SendTextMessageAsync(message.Chat, $"Домашній стадіон : {result.Response[0].Venue.Name}\nАдреса стадіону : {result.Response[0].Venue.Address}" +
                $", {result.Response[0].Venue.City}\nКількість сидячих місць : {result.Response[0].Venue.Capacity}");
            await botClient.SendTextMessageAsync(message.Chat, $"{result.Response[0].Venue.Image}");
            string connectionString = "Host=localhost;Username=postgres;Password=root;Database=postgres";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = "CREATE TABLE IF NOT EXISTS team (id SERIAL PRIMARY KEY, name VARCHAR(150) UNIQUE, country VARCHAR(50),founted VARCHAR(5),stadion VARCHAR(50))";
                using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string insertDataQuery = @"
                    INSERT INTO team (name, country,founted,stadion) 
                    VALUES (@name, @country, @founted, @stadion) 
                    ON CONFLICT (name) DO NOTHING";
                using (NpgsqlCommand command = new NpgsqlCommand(insertDataQuery, connection))
                {
                    command.Parameters.AddWithValue("name", result.Response[0].Team.Name);
                    command.Parameters.AddWithValue("country", result.Response[0].Team.Country);
                    command.Parameters.AddWithValue("founted", result.Response[0].Team.Founded);
                    command.Parameters.AddWithValue("stadion", result.Response[0].Venue.Name);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat, "Інформацію про футбольну команду не знайдено.");
        }
        userStates.Remove(message.Chat.Id);
    }
    /*Venues*/
    public static async Task HandleFootballVenuesCommand(ITelegramBotClient botClient, Message message)
    {
        userStates[message.Chat.Id] = "Напиши місто";
        await botClient.SendTextMessageAsync(message.Chat, "Щоб дізнатися про стадіон у місті,\nнапиши назву цього міста англійською мовою");
    }
    public static async Task HandleVenuesInput(ITelegramBotClient botClient, Message message)
    {
        string cityName = message.Text;

        var httpClient = new HttpClient();
        var url = $"https://localhost:7223/Venues?Name={Uri.EscapeDataString(cityName)}";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Venues>(responseContent);
        if (result.Response.Length > 0)
        {
            await botClient.SendTextMessageAsync(message.Chat, $"Назва стадіону : {result.Response[0].Name}\nВулиця : {result.Response[0].Address}\nМісто : {result.Response[0].City}\nКраїна : {result.Response[0].Country}" +
                $"\nКількість сидячих місць : {result.Response[0].Capacity}\nПокриття : {result.Response[0].Surface}");
            await botClient.SendTextMessageAsync(message.Chat, $"{result.Response[0].Image}");
            string connectionString = "Host=localhost;Username=postgres;Password=root;Database=postgres";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = "CREATE TABLE IF NOT EXISTS venue (id SERIAL PRIMARY KEY, name VARCHAR(150) UNIQUE, city VARCHAR(50),country VARCHAR(50),capacity VARCHAR(50),surface VARCHAR(50))";
                using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string insertDataQuery = @"
                    INSERT INTO venue (name, city, country, capacity, surface) 
                    VALUES (@name, @city, @country, @capacity, @surface)  
                    ON CONFLICT (name) DO NOTHING";
                using (NpgsqlCommand command = new NpgsqlCommand(insertDataQuery, connection))
                {
                    command.Parameters.AddWithValue("name", result.Response[0].Name);
                    command.Parameters.AddWithValue("city", result.Response[0].City);
                    command.Parameters.AddWithValue("country", result.Response[0].Country);
                    command.Parameters.AddWithValue("capacity", result.Response[0].Capacity);
                    command.Parameters.AddWithValue("surface", result.Response[0].Surface);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat, "Місце проведення футбольних матчів для даного міста не знайдено.");
        }
        userStates.Remove(message.Chat.Id);
    }
    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }
}