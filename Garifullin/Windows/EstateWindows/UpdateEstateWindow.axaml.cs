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

public partial class UpdateEstateWindow : Window
{
    ContextDB.Context context;
    public ObservableCollection<District> districts { get; }
    RealEstate estate;
    Apartment apartment;
    House house;
    Land land;
    public UpdateEstateWindow(RealEstate estate, ContextDB.Context context)
    {
        InitializeComponent();
        this.context = context;
        this.estate = estate;
        districts = new ObservableCollection<District>(context.Districts.ToList());
        districtsbox.ItemsSource = districts;
        if(estate.ApartmentId != null)
        {
            combik.SelectedIndex = 0;
            roomsbox.IsVisible = true;
            floorbox.IsVisible = true;
            totalfloorsbox.IsVisible = false;
            apartment = context.Apartments.FirstOrDefault(r => r.Id == estate.ApartmentId);
            roomsbox.Text = apartment.Rooms.ToString(); 
            floorbox.Text = apartment.Floor.ToString();
            totalareabox.Text = apartment.TotalArea.ToString();
            citybox.Text = estate.AddressCity;
            streetbox.Text = estate.AddressStreet;
            housebox.Text = estate.AddressHouse.ToString();
            numberbox.Text = estate.AddressNumber.ToString();
            latitudebox.Text = estate.CoordinateLatitude.ToString();
            longitudebox.Text = estate.CoordinateLongitude.ToString();
            if(estate.DistrictId != null)
            {
                districtsbox.SelectedIndex = (int)estate.DistrictId - 1;
            }
        }
        if(estate.HouseId != null)
        {
            combik.SelectedIndex = 1;
            roomsbox.IsVisible = true;
            floorbox.IsVisible = false;
            totalfloorsbox.IsVisible = true;
            house = context.Houses.FirstOrDefault(r => r.Id == estate.HouseId);
            roomsbox.Text = house.Rooms.ToString();
            totalfloorsbox.Text = house.TotalFloors.ToString();
            totalareabox.Text = house.TotalArea.ToString();
            citybox.Text = estate.AddressCity;
            streetbox.Text = estate.AddressStreet;
            housebox.Text = estate.AddressHouse.ToString();
            numberbox.Text = estate.AddressNumber.ToString();
            latitudebox.Text = estate.CoordinateLatitude.ToString();
            longitudebox.Text = estate.CoordinateLongitude.ToString();
            if (estate.DistrictId != null)
            {
                districtsbox.SelectedIndex = (int)estate.DistrictId - 1;
            }

        }
        if(estate.LandId != null)
        {
            roomsbox.IsVisible = false;
            floorbox.IsVisible = false;
            totalfloorsbox.IsVisible = false;
            combik.SelectedIndex = 2;
            land = context.Lands.FirstOrDefault(r => r.Id == estate.LandId);
            totalareabox.Text = land.TotalArea.ToString();
            citybox.Text = estate.AddressCity;
            streetbox.Text = estate.AddressStreet;
            housebox.Text = estate.AddressHouse.ToString();
            numberbox.Text = estate.AddressNumber.ToString();
            latitudebox.Text = estate.CoordinateLatitude.ToString();
            longitudebox.Text = estate.CoordinateLongitude.ToString();
            if (estate.DistrictId != null)
            {
                districtsbox.SelectedIndex = (int)estate.DistrictId - 1;
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
        switch (index)
        {
            case 0:
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
                        apartment.Rooms = result;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректное число комнат", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                }
                if (!floorbox.Text.IsNullOrEmpty())
                {
                    if (Int32.TryParse(floorbox.Text, out int result))
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
                if (!totalareabox.Text.IsNullOrEmpty())
                {
                    if (float.TryParse(totalareabox.Text, out float result))
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
                context.Apartments.Update(apartment);
                context.SaveChanges();
                context.RealEstates.Update(estate);
                context.SaveChanges();
                this.Close();
                break;
            case 1:
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
                context.Houses.Update(house);
                context.SaveChanges();
                context.RealEstates.Update(estate);
                context.SaveChanges();
                this.Close();
                break;
            case 2:
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
                context.Lands.Update(land);
                context.SaveChanges();
                context.RealEstates.Update(estate);
                context.SaveChanges();
                this.Close();
                break;
        }
    }
}