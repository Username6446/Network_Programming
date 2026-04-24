using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using Org.BouncyCastle.Crypto;
using System.Net.Mail;
using System.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
namespace Project4_IMAP_MailKit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string username = "likovvana777@gmail.com";
            string password = "lqbiudxzyjccymgo";
            Console.OutputEncoding = Encoding.UTF8;
            #region Send mail(SMTP)
            //MimeMessage message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Ivan", "Likovvana777@gmail.com"));
            //message.To.Add(new MailboxAddress("Test User", "kenaw58933@pertok.com"));
            //message.Subject = "Доброго вечора! Я з України!";
            //message.Importance = MessageImportance.High;

            //message.Body = new TextPart("plain")
            //{
            //    Text = @" Привіт, Аліна!
            //        Що ти плануєш робити на вихідних? 
            //        Є пропозиція 
            //        поїхати в аквапарк!
            //    "
            //};

            //using (var client = new SmtpClient())
            //{
            //    client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            //    client.Authenticate(username, password);
            //    client.Send(message);
            //    client.Disconnect(true);
            //}

            #endregion

            #region Get Mail Message (IMAP)


            //using (var client = new ImapClient())
            //{
            //    client.Connect("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
            //    client.Authenticate(username, password);


            //    foreach (var item in client.GetFolders(client.PersonalNamespaces[0]))
            //    {
            //        Console.WriteLine("Folder : " + item.Name);
            //    }

            //    // Read all messages in Folder Inbox

            //    //client.Inbox?.Open(FolderAccess.ReadOnly);
            //    //var uids = client.Inbox?.Search(SearchQuery.All);

            //    //foreach (var id in uids)
            //    //{
            //    //    var m = client.Inbox?.GetMessage(id);
            //    //    Console.WriteLine(m?.Subject);
            //    //}


            //    var folder = client.GetFolder(SpecialFolder.Sent);
            //    folder.Open(FolderAccess.ReadOnly);

            //    var id = folder.Search(SearchQuery.All);
            //    var lastId = id.LastOrDefault();
            //    folder.AddFlags(lastId, MessageFlags.Deleted, true);

            //    if (lastId != UniqueId.Invalid)
            //    {
            //        var m = folder.GetMessage(lastId);
            //        Console.WriteLine(m.Subject);
            //    }
            //    else
            //    {
            //        Console.WriteLine("No messages found in the Sent folder.");
            //    }


            //}

            #endregion

            #region DZ

            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    client.Authenticate(username, password);

                    Console.WriteLine("--- Оберіть папку ---");
                    Console.WriteLine("1. Входящие (Inbox)");
                    Console.WriteLine("2. Помеченные (Starred)");
                    Console.WriteLine("3. Отправленные (Sent)");
                    Console.WriteLine("4. Черновики (Drafts)");
                    Console.WriteLine("5. Покупки (Користувацький ярлик)");
                    Console.Write("\nВаш вибір: ");

                    string choice = Console.ReadLine();
                    IMailFolder folder = null;

                    switch (choice)
                    {
                        case "1":
                            folder = client.Inbox;
                            break;
                        case "2":
                            folder = client.GetFolder(SpecialFolder.Flagged);
                            break;
                        case "3":
                            folder = client.GetFolder(SpecialFolder.Sent);
                            break;
                        case "4":
                            folder = client.GetFolder(SpecialFolder.Drafts);
                            break;
                        case "5":
                            try
                            {
                                folder = client.GetFolder("Покупки");
                            }
                            catch
                            {
                                Console.WriteLine("\nПапку 'Покупки' не знайдено. Перевірте в налаштуваннях Gmail, чи увімкнено для неї 'Показувати в IMAP'.");
                                return;
                            }
                            break;
                        default:
                            Console.WriteLine("\nНевірний вибір. Вихід...");
                            return;
                    }

                    folder.Open(FolderAccess.ReadWrite);
                    Console.WriteLine($"\nВідкрито папку: {folder.Name}. Усього повідомлень: {folder.Count}\n");

                    if (folder.Count == 0)
                    {
                        Console.WriteLine("Папка порожня.");
                        return;
                    }

                    int startIndex = Math.Max(0, folder.Count - 10);
                    var summaries = folder.Fetch(startIndex, folder.Count - 1, MessageSummaryItems.Envelope);

                    for (int i = summaries.Count - 1; i >= 0; i--)
                    {
                        var summary = summaries[i];
                        string subject = summary.Envelope.Subject ?? "(Без теми)";
                        Console.WriteLine($"[{summary.Index}] Тема: {subject}");
                    }

                    Console.Write("\nВведіть номер листа (у квадратних дужках), з яким хочете взаємодіяти (або Enter для виходу): ");
                    string msgChoice = Console.ReadLine();

                    if (int.TryParse(msgChoice, out int msgIndex) && msgIndex >= 0 && msgIndex < folder.Count)
                    {
                        var uidSummary = folder.Fetch(msgIndex, msgIndex, MessageSummaryItems.UniqueId).FirstOrDefault();

                        if (uidSummary == null)
                        {
                            Console.WriteLine("Помилка: не вдалося знайти лист на сервері.");
                            return;
                        }

                        var uid = uidSummary.UniqueId;

                        Console.WriteLine("\nЩо зробити з цим листом?");
                        Console.WriteLine("1. Перемістити у Спам (Junk)");
                        Console.WriteLine("2. Видалити назавжди");
                        Console.Write("Ваш вибір: ");

                        string actionChoice = Console.ReadLine();

                        if (actionChoice == "1")
                        {
                            var junkFolder = client.GetFolder(SpecialFolder.Junk);
                            folder.MoveTo(uid, junkFolder);
                            Console.WriteLine("Лист успішно переміщено у Спам!");
                        }
                        else if (actionChoice == "2")
                        {
                            folder.AddFlags(uid, MessageFlags.Deleted, true);
                            folder.Expunge();
                            Console.WriteLine("Лист успішно видалено!");
                        }
                        else
                        {
                            Console.WriteLine("Дію скасовано.");
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(msgChoice))
                    {
                        Console.WriteLine("Невірний номер листа.");
                    }

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nСталася помилка: {ex.Message}");
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();

            #endregion

        }
    }
}
