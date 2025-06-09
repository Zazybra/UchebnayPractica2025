using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Garifullin.Models;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using Garifullin.Classes;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Garifullin;

public partial class CreateDealWindow : Window
{
    ContextDB.Context context;
    public ObservableCollection<Demand> demands { get; }
    public ObservableCollection<Supply> supplies { get; }
    public ObservableCollection<SupplyAddress> suppliesadresses { get; }
    public ObservableCollection<DemandAdrres> demandadresses { get; }
    DealList gotdeal;
    public CreateDealWindow()
    {
        InitializeComponent();
        context = new ContextDB.Context();
        demands = new ObservableCollection<Demand>(context.Demands.ToList());
        supplies = new ObservableCollection<Supply>(context.Supplies.ToList());
        var sup = new List<SupplyAddress>();
        var dem = new List<DemandAdrres>();
        foreach (Demand demand in demands)
        {
            var item = new DemandAdrres();
            item.Id = demand.Id;
            item.Address = demand.AddressCity + " " + demand.AddressStreet + " " + demand.AddressHouse + " " + demand.AddressNumber;
            if (item.Address.TrimEnd().IsNullOrEmpty())
            {
                item.Address = "Адресс отсуствует";
            }
            dem.Add(item);
        }
        foreach (Supply supply in supplies)
        {
            RealEstate estate = context.RealEstates.FirstOrDefault(r => r.Id == supply.RealEstateId);
            var item = new SupplyAddress();
            item.Id = supply.Id;
            item.Address = estate.AddressCity + " " + estate.AddressStreet + " " + estate.AddressHouse + " " + estate.AddressNumber;
            if (item.Address.TrimEnd().IsNullOrEmpty())
            {
                item.Address = "Адресс отсуствует";
            }
            sup.Add(item);
        }
        suppliesadresses = new ObservableCollection<SupplyAddress>(sup);
        demandadresses = new ObservableCollection<DemandAdrres>(dem);
        this.DataContext = this;
        demandbox.SelectedIndex = 0;
        supplybox.SelectedIndex = 0;
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (demandbox.SelectedItem != null && supplybox.SelectedItem != null)
        {
            DemandAdrres demand = demandbox.SelectedItem as DemandAdrres;
            SupplyAddress supply = supplybox.SelectedItem as SupplyAddress;
            if (context.Deals.FirstOrDefault(r => r.SupplyId == supply.Id) == null)
            {
                if (context.Deals.FirstOrDefault(r => r.DemandId == demand.Id) == null)
                {
                    var deal = new Deal();
                    deal.SupplyId = supply.Id;
                    deal.DemandId = demand.Id;
                    context.Deals.Add(deal);
                    context.SaveChanges();
                    this.Close();
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Потребность уже закрыта", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Предложение закрыто", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }
    }

    private void CancelClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}