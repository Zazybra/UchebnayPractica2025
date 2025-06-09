using System;
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

public partial class CreateDemandWindow : Window
{
    ContextDB.Context context;
    public ObservableCollection<District> districts { get; }
    public ObservableCollection<Agent> agents { get; }
    public ObservableCollection<Client> clients { get; }
    public CreateDemandWindow()
    {
        InitializeComponent();
        context = new ContextDB.Context();
        districts = new ObservableCollection<District>(context.Districts.ToList());
        agents = new ObservableCollection<Agent>(context.Agents.ToList());
        clients = new ObservableCollection<Client>(context.Clients.ToList());
        combik.SelectedIndex = 0;
        this.DataContext = this;
        clientbox.SelectedIndex = 0;
        agentbox.SelectedIndex = 0;
    }
    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (combik != null)
        {
            int index = combik.SelectedIndex;
            switch (index)
            {
                case 0:
                    minroomsbox.IsVisible = true;
                    maxroomsbox.IsVisible = true;
                    minfloorbox.IsVisible = true;
                    maxfloorbox.IsVisible = true;
                    break;
                case 1:
                    minroomsbox.IsVisible = true;
                    maxroomsbox.IsVisible = true;
                    minfloorbox.IsVisible = true;
                    maxfloorbox.IsVisible = true;
                    break;
                case 2:
                    minroomsbox.IsVisible = false;
                    maxroomsbox.IsVisible = false;
                    minfloorbox.IsVisible = false;
                    maxfloorbox.IsVisible = false;
                    break;
            }
        }
    }

    private void CancelClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }

    private async void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int index = combik.SelectedIndex;
        Agent agent = new Agent();
        Client client = new Client();
        Demand demand = new Demand();
        int result;
        switch (index)
        {
            case 0:
               ApartmentDemand apartment = new ApartmentDemand();
                demand = new Demand();
                demand.AddressCity = citybox.Text;
                demand.AddressStreet = streetbox.Text;
                if (!housebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(housebox.Text, out result))
                    {
                        demand.AddressHouse = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(numberbox.Text))
                {

                    if (Int32.TryParse(numberbox.Text, out result))
                    {

                        demand.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер квартиры", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!minpricebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            demand.MinPrice = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите минимальную цену больше нуля", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную минимальную цену", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxpricebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            demand.MaxPrice = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите максимальную цену больше нуля", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную максимальную цену", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                demand.TypeId = index + 1;
                agent = agentbox.SelectedItem as Agent;
                client = clientbox.SelectedItem as Client;
                if(agent != null)
                {
                    demand.AgentId = agent.Id;
                }
                if(client != null)
                {
                    demand.ClientId = client.Id;
                }
                if (!minroomsbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minroomsbox.Text, out result))
                    {
                        if (result > 0)
                        {
                            apartment.MinRooms = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите кол-во комнат больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное минимальное число комнат", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxroomsbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxroomsbox.Text, out result))
                    {
                        if (result > 0)
                        {
                            apartment.MaxRooms = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите кол-во комнат больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное максимальное число комнат", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if(!minfloorbox.Text.IsNullOrEmpty())
                if (Int32.TryParse(minfloorbox.Text, out result))
                {
                    if (result > 0)
                    {
                        apartment.MinFloor = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите минимальный этаж больше 0", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное минимальный этаж", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
                if (!maxfloorbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxfloorbox.Text, out result))
                    {
                        if (result > 0)
                        {
                            apartment.MaxFloor = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите этаж больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное максимально число этажа", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!minareabox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minareabox.Text, out result))
                    {
                        if (result > 0)
                        {
                            apartment.MinArea = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите минимальную площадь больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную минимальную площадь", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxareabox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxareabox.Text, out result))
                    {
                        if (result > 0)
                        {
                            apartment.MaxArea = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите максимальную площадь больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную максимальную площадь", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                context.ApartmentDemands.Add(apartment);
                context.SaveChanges();
                apartment = context.ApartmentDemands.OrderByDescending(a => a.Id).FirstOrDefault();
                demand.ApartmentDemandId = apartment.Id;
                context.Demands.Add(demand);
                context.SaveChanges();
                this.Close();
                break;
            case 1:
                HouseDemand house = new HouseDemand();
                demand = new Demand();
                demand.AddressCity = citybox.Text;
                demand.AddressStreet = streetbox.Text;
                if (!housebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(housebox.Text, out result))
                    {
                        demand.AddressHouse = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(numberbox.Text))
                {

                    if (Int32.TryParse(numberbox.Text, out result))
                    {

                        demand.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер квартиры", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!minpricebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            demand.MinPrice = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите минимальную цену больше нуля", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную минимальную цену", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxpricebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            demand.MaxPrice = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите максимальную цену больше нуля", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную максимальную цену", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                demand.TypeId = index + 1;
                agent = agentbox.SelectedItem as Agent;
                client = clientbox.SelectedItem as Client;
                if (agent != null)
                {
                    demand.AgentId = agent.Id;
                }
                if (client != null)
                {
                    demand.ClientId = client.Id;
                }
                if (!minroomsbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minroomsbox.Text, out result))
                    {
                        if (result > 0)
                        {
                            house.MinRooms = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите кол-во комнат больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное минимальное число комнат", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxroomsbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxroomsbox.Text, out result))
                    {
                        if (result > 0)
                        {
                            house.MaxRooms = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите кол-во комнат больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное максимальное число комнат", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!minfloorbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minfloorbox.Text, out result))
                    {
                        if (result > 0)
                        {
                            house.MinFloor = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите минимальный этаж больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное минимальный этаж", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxfloorbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxfloorbox.Text, out result))
                    {
                        if (result > 0)
                        {
                            house.MaxFloor = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите этаж больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное максимально число этажа", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!minareabox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minareabox.Text, out result))
                    {
                        if (result > 0)
                        {
                            house.MinArea = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите минимальную площадь больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную минимальную площадь", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxareabox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxareabox.Text, out result))
                    {
                        if (result > 0)
                        {
                            house.MaxArea = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите максимальную площадь больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную максимальную площадь", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                context.HouseDemands.Add(house);
                context.SaveChanges();
                house = context.HouseDemands.OrderByDescending(a => a.Id).FirstOrDefault();
                demand.HouseDemandId = house.Id;
                context.Demands.Add(demand);
                context.SaveChanges();
                this.Close();
                break;
            case 2:
                LandDemand land = new LandDemand();
                demand = new Demand();
                demand.AddressCity = citybox.Text;
                demand.AddressStreet = streetbox.Text;
                if (!housebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(housebox.Text, out result))
                    {
                        demand.AddressHouse = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(numberbox.Text))
                {

                    if (Int32.TryParse(numberbox.Text, out result))
                    {

                        demand.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер квартиры", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!minpricebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            demand.MinPrice = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите минимальную цену больше нуля", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную минимальную цену", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxpricebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            demand.MaxPrice = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите максимальную цену больше нуля", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную максимальную цену", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                demand.TypeId = index + 1;
                agent = agentbox.SelectedItem as Agent;
                client = clientbox.SelectedItem as Client;
                if (agent != null)
                {
                    demand.AgentId = agent.Id;
                }
                if (client != null)
                {
                    demand.ClientId = client.Id;
                }
                if (!minareabox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(minareabox.Text, out result))
                    {
                        if (result > 0)
                        {
                            land.MinArea = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите минимальную площадь больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную минимальную площадь", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!maxareabox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(maxareabox.Text, out result))
                    {
                        if (result > 0)
                        {
                            land.MaxArea = result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите максимальную площадь больше 0", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную максимальную площадь", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                context.LandDemands.Add(land);
                context.SaveChanges();
                land = context.LandDemands.OrderByDescending(a => a.Id).FirstOrDefault();
                demand.LandDemandId = land.Id;
                context.Demands.Add(demand);
                context.SaveChanges();
                this.Close();
                break;
        }
    }
}
    