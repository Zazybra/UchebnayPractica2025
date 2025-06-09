using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Garifullin.ContextDB;
using Garifullin.Models;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Collections.ObjectModel;
using System.Linq;
using Garifullin.Classes;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace Garifullin;

public partial class UpdateDealWindow : Window
{
    ContextDB.Context context;
    public ObservableCollection<Demand> demands { get; }
    public ObservableCollection<Supply> supplies { get; }
    public ObservableCollection<SupplyAddress> suppliesadresses { get; }
    public ObservableCollection<DemandAdrres> demandadresses {  get; }
    DealList gotdeal;
    public UpdateDealWindow(DealList deal, ContextDB.Context context)
    {
        InitializeComponent();
        this.context = context;
        this.gotdeal = deal;
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
            if(item.Address.TrimEnd().IsNullOrEmpty())
            {
                item.Address = "Адресс отсуствует";
            }
            sup.Add(item);
        }
        suppliesadresses = new ObservableCollection<SupplyAddress>(sup);
        demandadresses = new ObservableCollection<DemandAdrres>(dem);
        this.DataContext = this;
        demandbox.SelectedItem = demandadresses.FirstOrDefault(r => r.Id == deal.DemandId);
        supplybox.SelectedItem = suppliesadresses.FirstOrDefault(r => r.Id == deal.SupplyId);
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (demandbox.SelectedItem != null && supplybox.SelectedItem != null)
        {
            DemandAdrres demand = demandbox.SelectedItem as DemandAdrres;
            SupplyAddress supply = supplybox.SelectedItem as SupplyAddress;
            if (context.Deals.FirstOrDefault(r => r.SupplyId == supply.Id && r.Id != gotdeal.Id) == null)
            {
                if (context.Deals.FirstOrDefault(r => r.DemandId == demand.Id && r.Id != gotdeal.Id) == null)
                {
                    var deal = context.Deals.FirstOrDefault(r => r.Id == gotdeal.Id);
                    deal.SupplyId = supply.Id;
                    deal.DemandId = demand.Id;
                    context.Deals.Update(deal);
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