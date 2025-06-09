using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Garifullin.Models;
using Microsoft.IdentityModel.Tokens;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace Garifullin;

public partial class UpdateSupply : Window
{
    ContextDB.Context context;
    Supply gotsupply;
    public ObservableCollection<Agent> agents { get; }
    public ObservableCollection<Client> clients { get; }
    public ObservableCollection<RealEstate> estates { get; }
    public UpdateSupply(Supply supply,ContextDB.Context context)
    {
        InitializeComponent();
        this.context = context;
        gotsupply = supply;
        agents = new ObservableCollection<Agent>(context.Agents.ToList());
        clients = new ObservableCollection<Client>(context.Clients.ToList());
        estates = new ObservableCollection<RealEstate>(context.RealEstates.ToList());
        this.DataContext = this;
        agentbox.SelectedItem = context.Agents.FirstOrDefault(r => r.Id == supply.AgentId);
        clientbox.SelectedItem = context.Clients.FirstOrDefault(r => r.Id == supply.ClientId);
        estatebox.SelectedItem = context.RealEstates.FirstOrDefault(r => r.Id == supply.RealEstateId);
        pricebox.Text = supply.Price.ToString();
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (agentbox.SelectedIndex != null && clientbox.SelectedIndex != null && estatebox.SelectedIndex != null && !pricebox.Text.IsNullOrEmpty())
        {
            if (float.TryParse(pricebox.Text, out float result))
            {
                if (result > 0)
                {
                    var agent = agentbox.SelectedItem as Agent;
                    var client = clientbox.SelectedItem as Client;
                    var estate = estatebox.SelectedItem as RealEstate;
                    gotsupply.ClientId = client.Id;
                    gotsupply.AgentId = agent.Id;
                    gotsupply.RealEstateId = estate.Id;
                    gotsupply.Price = result;
                    context.Supplies.Update(gotsupply);
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

    private void CancelButton(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}