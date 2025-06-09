using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Garifullin.Models;
using Microsoft.IdentityModel.Tokens;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Garifullin;

public partial class UpdateClientWindow : Window
{
    ContextDB.Context context;
    Client gotclient;
    public UpdateClientWindow(Client client, ContextDB.Context context)
    {
        InitializeComponent();
        this.context = context;
        this.gotclient = client;
        namebox.Text = gotclient.FirstName;
        familybox.Text = gotclient.MiddleName;
        dadbox.Text = gotclient.LastName;
        phonebox.Text = gotclient.Phone;
        mailbox.Text = gotclient.Email;
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }

    private async void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string patternmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        string patternphone = @"^8\d{10}$";
        Client client = new Client();
        client.Id = gotclient.Id;
        if (!namebox.Text.IsNullOrEmpty())
        {
            client.FirstName = namebox.Text;
        }
        if (!familybox.Text.IsNullOrEmpty())
        {
            client.MiddleName = familybox.Text;
        }
        if (!dadbox.Text.IsNullOrEmpty())
        {
            client.LastName = dadbox.Text;
        }
        if (!phonebox.Text.IsNullOrEmpty())
        {
            if (Regex.IsMatch(phonebox.Text, patternphone, RegexOptions.IgnoreCase))
            {
                if (context.Clients.FirstOrDefault(r => r.Phone == phonebox.Text && r.Id != gotclient.Id) == null)
                {
                    client.Phone = phonebox.Text;
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Номер занят", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Некорректный номер телефона!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }
        if (!mailbox.Text.IsNullOrEmpty())
        {
            if (Regex.IsMatch(mailbox.Text, patternmail, RegexOptions.IgnoreCase))
            {
                if (context.Clients.FirstOrDefault(r => r.Email == mailbox.Text && r.Id != gotclient.Id) == null)
                {
                    client.Email = mailbox.Text;
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Почта уже занята!", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Некорректная почта!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }
        if (client.Email == null && client.Phone == null)
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите почту или телефон!", ButtonEnum.Ok);
            await errorBox.ShowWindowDialogAsync(this);
            return;
        }
        else
        {
            gotclient.FirstName = client.FirstName;
            gotclient.MiddleName = client.MiddleName;
            gotclient.LastName = client.LastName;
            gotclient.Email = client.Email;
            gotclient.Phone = client.Phone;
            context.Clients.Update(gotclient);
            context.SaveChanges();
            this.Close();
        }
    }
}