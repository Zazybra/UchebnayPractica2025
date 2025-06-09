using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Garifullin.Models;
using Microsoft.IdentityModel.Tokens;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace Garifullin;

public partial class UpdateAgentWindow : Window
{
    ContextDB.Context context;
    Agent gotagent;
    public UpdateAgentWindow(Agent agent, ContextDB.Context context)
    {
        InitializeComponent();
        this.gotagent = agent;
        this.context = context;
        namebox.Text = gotagent.FirstName;
        familybox.Text = gotagent.MiddleName;
        dadbox.Text = gotagent.LastName;
        comissionbox.Text = gotagent.DealShare.ToString();
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Agent agent = gotagent;
        if (!namebox.Text.IsNullOrEmpty())
        {
            agent.FirstName = namebox.Text;
        }
        else
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните имя", ButtonEnum.Ok);
            await errorBox.ShowWindowDialogAsync(this);
            return;
        }
        if (!familybox.Text.IsNullOrEmpty())
        {
            agent.MiddleName = familybox.Text;
        }
        else
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните фамилию", ButtonEnum.Ok);
            await errorBox.ShowWindowDialogAsync(this);
            return;
        }
        if (!dadbox.Text.IsNullOrEmpty())
        {
            agent.LastName = dadbox.Text;
        }
        else
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните отчество", ButtonEnum.Ok);
            await errorBox.ShowWindowDialogAsync(this);
            return;
        }
        if (comissionbox.Text.IsNullOrEmpty())
        {
            
            context.Agents.Update(agent);
            context.SaveChanges();
            this.Close();
        }
        else
        {
            if (!Int32.TryParse(comissionbox.Text, out int result))
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Некорректное число", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
            else
            {
                if (result >= 0 && result <= 100)
                {
                    agent.DealShare = result;
                    context.Agents.Update(agent);
                    context.SaveChanges();
                    this.Close();
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Число от 0 до 100", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
            }
        }
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}