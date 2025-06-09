using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Garifullin.Models;
using Microsoft.IdentityModel.Tokens;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace Garifullin.Windows.EstateWindows;

public partial class CreateEstateWindow : Window
{
    ContextDB.Context context;
    public ObservableCollection<District> districts { get; }
    public CreateEstateWindow()
    {
        InitializeComponent();
        context = new ContextDB.Context();
        districts = new ObservableCollection<District>(context.Districts.ToList());
        districtsbox.ItemsSource = districts;
        combik.SelectedIndex = 0;
    }

    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (combik != null)
        {
            int index = combik.SelectedIndex;
            switch (index)
            {
                case 0:
                    roomsbox.IsVisible = true;
                    floorbox.IsVisible = true;
                    totalfloorsbox.IsVisible = false;
                    break;
                case 1:
                    roomsbox.IsVisible = true;
                    floorbox.IsVisible = false;
                    totalfloorsbox.IsVisible = true;
                    break;
                case 2:
                    roomsbox.IsVisible = false;
                    floorbox.IsVisible = false;
                    totalfloorsbox.IsVisible = false;
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
        RealEstate estate = new RealEstate();
        switch (index)
        {
            case 0:
                Apartment apartment = new Apartment();
                if (!citybox.Text.IsNullOrEmpty())
                {
                    estate.AddressCity = citybox.Text;   
                }
                if(!streetbox.Text.IsNullOrEmpty())
                {
                    estate.AddressStreet = streetbox.Text;
                }
                if(!housebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(housebox.Text, out int result))
                    {
                        estate.AddressHouse = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if(!numberbox.Text.IsNullOrEmpty())
                {
                    if(Int32.TryParse(numberbox.Text,out int result))
                    {
                        estate.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if(!latitudebox.Text.IsNullOrEmpty())
                {
                    if(float.TryParse(latitudebox.Text, out float result))
                    {
                        if (result >= -90 && result <= 90)
                        {
                            estate.CoordinateLatitude = (decimal)result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Широта от -90 до 90", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную широту", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!longitudebox.Text.IsNullOrEmpty())
                {
                    if (float.TryParse(longitudebox.Text, out float result))
                    {
                        if (result >= -180 && result <= 180)
                        {
                            estate.CoordinateLongitude = (decimal)result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Долгота от -180 до 180", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную долготу", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!roomsbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(roomsbox.Text, out int result))
                    {
                        apartment.Rooms = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное число комнат", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if(!floorbox.Text.IsNullOrEmpty())
                {
                    if(Int32.TryParse(floorbox.Text,out int result))
                    {
                        apartment.Floor = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректно число этажей", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if(!totalareabox.Text.IsNullOrEmpty())
                {
                    if(float.TryParse(totalareabox.Text,out float result))
                    {
                        apartment.TotalArea = (decimal)result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Номер занят", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                context.Apartments.Add(apartment);
                context.SaveChanges();
                apartment = context.Apartments.OrderByDescending(a => a.Id).FirstOrDefault();
                estate.ApartmentId = apartment.Id;
                context.RealEstates.Add(estate);
                context.SaveChanges();
                this.Close();
                break;
            case 1:
                House house = new House();
                if (!citybox.Text.IsNullOrEmpty())
                {
                    estate.AddressCity = citybox.Text;
                }
                if (!streetbox.Text.IsNullOrEmpty())
                {
                    estate.AddressStreet = streetbox.Text;
                }
                if (!housebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(housebox.Text, out int result))
                    {
                        estate.AddressHouse = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!numberbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(numberbox.Text, out int result))
                    {
                        estate.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!latitudebox.Text.IsNullOrEmpty())
                {
                    if (float.TryParse(latitudebox.Text, out float result))
                    {
                        if (result >= -90 && result <= 90)
                        {
                            estate.CoordinateLatitude = (decimal)result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Широта от -90 до 90", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную широту", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!longitudebox.Text.IsNullOrEmpty())
                {
                    if (float.TryParse(longitudebox.Text, out float result))
                    {
                        if (result >= -180 && result <= 180)
                        {
                            estate.CoordinateLongitude = (decimal)result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Долгота от -180 до 180", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную долготу", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!roomsbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(roomsbox.Text, out int result))
                    {
                        house.Rooms = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное число комнат", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!totalfloorsbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(totalfloorsbox.Text, out int result))
                    {
                        house.TotalFloors = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректно число этажей", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!totalareabox.Text.IsNullOrEmpty())
                {
                    if (float.TryParse(totalareabox.Text, out float result))
                    {
                        house.TotalArea = (decimal)result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Номер занят", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                context.Houses.Add(house);
                context.SaveChanges();
                house = context.Houses.OrderByDescending(a => a.Id).FirstOrDefault();
                estate.HouseId = house.Id;
                context.RealEstates.Add(estate);
                context.SaveChanges();
                this.Close();
                break;
            case 2:
                Land land = new Land();
                if (!citybox.Text.IsNullOrEmpty())
                {
                    estate.AddressCity = citybox.Text;
                }
                if (!streetbox.Text.IsNullOrEmpty())
                {
                    estate.AddressStreet = streetbox.Text;
                }
                if (!housebox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(housebox.Text, out int result))
                    {
                        estate.AddressHouse = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!numberbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(numberbox.Text, out int result))
                    {
                        estate.AddressNumber = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный номер дома", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!latitudebox.Text.IsNullOrEmpty())
                {
                    if (float.TryParse(latitudebox.Text, out float result))
                    {
                        if (result >= -90 && result <= 90)
                        {
                            estate.CoordinateLatitude = (decimal)result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Широта от -90 до 90", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную широту", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!longitudebox.Text.IsNullOrEmpty())
                {
                    if (float.TryParse(longitudebox.Text, out float result))
                    {
                        if (result >= -180 && result <= 180)
                        {
                            estate.CoordinateLongitude = (decimal)result;
                        }
                        else
                        {
                            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Долгота от -180 до 180", ButtonEnum.Ok);
                            await errorBox.ShowWindowDialogAsync(this);
                            return;
                        }
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную долготу", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!totalareabox.Text.IsNullOrEmpty())
                {
                    if (float.TryParse(totalareabox.Text, out float result))
                    {
                        land.TotalArea = (decimal)result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Номер занят", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                context.Lands.Add(land);
                context.SaveChanges();
                land = context.Lands.OrderByDescending(a => a.Id).FirstOrDefault();
                estate.LandId = land.Id;
                context.RealEstates.Add(estate);
                context.SaveChanges();
                this.Close();
                break;
        }
    }
}