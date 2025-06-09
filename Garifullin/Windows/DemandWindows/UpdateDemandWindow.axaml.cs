using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.DataContracts;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Garifullin.Models;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace Garifullin;

public partial class UpdateDemandWindow : Window
{
    ContextDB.Context context;
    Demand gotdemand;
    public ObservableCollection<District> districts { get; }
    public ObservableCollection<Agent> agents { get; }
    public ObservableCollection<Client> clients { get; }
    public UpdateDemandWindow(Demand demand, ContextDB.Context context)
    {
        InitializeComponent();
        this.context = context;
        districts = new ObservableCollection<District>(context.Districts.ToList());
        agents = new ObservableCollection<Agent>(context.Agents.ToList());
        clients = new ObservableCollection<Client>(context.Clients.ToList());
        this.gotdemand = demand;
        combik.SelectedIndex = demand.TypeId-1;
        combik.IsEnabled = false;
        this.DataContext = this;
        citybox.Text = demand.AddressCity;
        streetbox.Text = demand.AddressStreet;
        housebox.Text = demand.AddressHouse.ToString();
        numberbox.Text = demand.AddressNumber.ToString();
        minpricebox.Text = demand.MinPrice.ToString();
        maxpricebox.Text = demand.MaxPrice.ToString();
        agentbox.IsEnabled = false;
        clientbox.IsEnabled = false;
        agentbox.SelectedItem = context.Agents.FirstOrDefault(r => r.Id == demand.AgentId);
        clientbox.SelectedItem = context.Clients.FirstOrDefault(r => r.Id == demand.ClientId);
        switch(demand.TypeId)
        {
            case 1:
                ApartmentDemand apartment = context.ApartmentDemands.FirstOrDefault(r => r.Id == demand.ApartmentDemandId);
                minareabox.Text = apartment.MinArea.ToString();
                maxareabox.Text = apartment.MaxArea.ToString();
                minroomsbox.Text = apartment.MinRooms.ToString();
                maxroomsbox.Text = apartment.MaxRooms.ToString();
                minfloorbox.Text = apartment.MinFloor.ToString();
                maxfloorbox.Text = apartment.MaxFloor.ToString();
                break;
            case 2:
                HouseDemand house = context.HouseDemands.FirstOrDefault(r => r.Id == demand.HouseDemandId);
                minareabox.Text = house.MinArea.ToString();
                maxareabox.Text = house.MaxArea.ToString();
                minroomsbox.Text = house.MinRooms.ToString();
                maxroomsbox.Text = house.MaxRooms.ToString();
                minfloorbox.Text = house.MinFloor.ToString();
                maxfloorbox.Text= house.MaxFloor.ToString();
                break;
            case 3:
                LandDemand land = context.LandDemands.FirstOrDefault(r => r.Id == demand.LandDemandId);
                minareabox.Text = land.MinArea.ToString();
                maxareabox.Text= land.MaxArea.ToString();
                break;
        }
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int index = combik.SelectedIndex;
        Agent agent = new Agent();
        Client client = new Client();
        int result;
        switch (index)
        {
            case 0:
                ApartmentDemand apartment = context.ApartmentDemands.FirstOrDefault(r => r.Id == gotdemand.ApartmentDemandId);
                gotdemand.AddressCity = citybox.Text;
                gotdemand.AddressStreet = streetbox.Text;
                if (!string.IsNullOrEmpty(housebox.Text))
                {
                    if (Int32.TryParse(housebox.Text, out result))
                    {
                        gotdemand.AddressHouse = result;
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

                        gotdemand.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер квартиры", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(minpricebox.Text))
                {
                    if (Int32.TryParse(minpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            gotdemand.MinPrice = result;
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
                if (!string.IsNullOrEmpty(maxpricebox.Text))
                {
                    if (Int32.TryParse(maxpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            gotdemand.MaxPrice = result;
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
                gotdemand.TypeId = index + 1;
                if (!string.IsNullOrEmpty(minroomsbox.Text))
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
                if (!string.IsNullOrEmpty(maxroomsbox.Text))
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
                if (!string.IsNullOrEmpty(minfloorbox.Text))
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
                if (!string.IsNullOrEmpty(maxfloorbox.Text))
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
                if (!string.IsNullOrEmpty(minareabox.Text))
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
                if (!string.IsNullOrEmpty(maxareabox.Text))
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
                context.ApartmentDemands.Update(apartment);
                context.SaveChanges();
                context.Demands.Update(gotdemand);
                context.SaveChanges();
                this.Close();
                break;
            case 1:
                HouseDemand house = context.HouseDemands.FirstOrDefault(r => r.Id == gotdemand.HouseDemandId);
                gotdemand.AddressCity = citybox.Text;
                gotdemand.AddressStreet = streetbox.Text;
                if (!string.IsNullOrEmpty(housebox.Text))
                {
                    if (Int32.TryParse(housebox.Text, out result))
                    {
                        gotdemand.AddressHouse = result;
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

                        gotdemand.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер квартиры", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(minpricebox.Text))
                {
                    if (Int32.TryParse(minpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            gotdemand.MinPrice = result;
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
                if (!string.IsNullOrEmpty(maxpricebox.Text))
                {
                    if (Int32.TryParse(maxpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            gotdemand.MaxPrice = result;
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
                gotdemand.TypeId = index + 1;
                if (!string.IsNullOrEmpty(minroomsbox.Text))
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
                if (!string.IsNullOrEmpty(maxroomsbox.Text))
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
                if (!string.IsNullOrEmpty(minfloorbox.Text))
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
                if (!string.IsNullOrEmpty(maxfloorbox.Text))
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
                if (!string.IsNullOrEmpty(minareabox.Text))
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
                if (!string.IsNullOrEmpty(maxareabox.Text))
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
                context.HouseDemands.Update(house);
                context.SaveChanges();
                gotdemand.HouseDemandId = house.Id;
                context.Demands.Add(gotdemand);
                context.SaveChanges();
                this.Close();
                break;
            case 2:
                LandDemand land = context.LandDemands.FirstOrDefault(r => r.Id == gotdemand.LandDemandId);
                gotdemand.AddressCity = citybox.Text;
                gotdemand.AddressStreet = streetbox.Text;
                if (!string.IsNullOrEmpty(housebox.Text))
                {
                    if (Int32.TryParse(housebox.Text, out result))
                    {
                        gotdemand.AddressHouse = result;
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

                        gotdemand.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер квартиры", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(minpricebox.Text))
                {
                    if (Int32.TryParse(minpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            gotdemand.MinPrice = result;
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
                if (!string.IsNullOrEmpty(maxpricebox.Text))
                {
                    if (Int32.TryParse(maxpricebox.Text, out result))
                    {
                        if (result > 0)
                        {
                            gotdemand.MaxPrice = result;
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
                gotdemand.TypeId = index + 1;
                if (string.IsNullOrEmpty(minareabox.Text))
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
                if (!string.IsNullOrEmpty(maxareabox.Text))
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
                context.LandDemands.Update(land);
                context.SaveChanges();
                context.Demands.Update(gotdemand);
                context.SaveChanges();
                this.Close();
                break;
        }
    }

    private void CancelClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}