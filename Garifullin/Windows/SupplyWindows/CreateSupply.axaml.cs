using System.Collections.ObjectModel;
using Avalonia;
using Garifullin.Models;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;

namespace Garifullin;

public partial class CreateSupply : Window
{
    ContextDB.Context context;
    public ObservableCollection<Agent> agents { get; }
    public ObservableCollection<Client> clients { get; }
    public ObservableCollection<RealEstate> estates { get; }
    public CreateSupply()
    {
        InitializeComponent();
        context = new ContextDB.Context();
        agents = new ObservableCollection<Agent>(context.Agents.ToList());
        clients = new ObservableCollection<Client>(context.Clients.ToList());
        estates = new ObservableCollection<RealEstate>(context.RealEstates.ToList());
        this.DataContext = this;
        agentbox.SelectedIndex = 0;
        clientbox.SelectedIndex = 0;
        estatebox.SelectedIndex = 0;
    }

    private void CancelClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }

    private async void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(agentbox.SelectedIndex!= null && clientbox.SelectedIndex!= null && estatebox.SelectedIndex!=null && !pricebox.Text.IsNullOrEmpty())
        {
            if(float.TryParse(pricebox.Text,out float result))
            {
                if (result > 0)
                {
                    Supply supply = new Supply();
                    var agent = agentbox.SelectedItem as Agent;
                    var client = clientbox.SelectedItem as Client;
                    var estate = estatebox.SelectedItem as RealEstate;
                    supply.ClientId = client.Id;
                    supply.AgentId = agent.Id;
                    supply.RealEstateId = estate.Id;
                    supply.Price = result;
                    context.Supplies.Add(supply);
                    context.SaveChanges();
                    this.Close();
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите цену больше 0", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную цену", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }
        else
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните все поля", ButtonEnum.Ok);
            await errorBox.ShowWindowDialogAsync(this);
            return;
        }
    }
}