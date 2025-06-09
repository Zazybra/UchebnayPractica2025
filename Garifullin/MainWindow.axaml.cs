using System.Collections.ObjectModel;
using System;
using Avalonia.Controls;
using Garifullin.Models;
using System.Linq;
using Garifullin.Classes;
using System.Collections.Generic;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using Garifullin.Windows.EstateWindows;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Specialized;

namespace Garifullin
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Agent> Rieltors { get; set; }
        public ObservableCollection<DealList> deallist { get; set; }
        public ObservableCollection<RealEstate> Estates { get; set; }
        public ObservableCollection<ApartmentList> Apartments { get; set; }
        public ObservableCollection<HouseList> Houses { get; set; }
        public ObservableCollection<LandList> Lands { get; set; }
        public ObservableCollection<Supply> Supplies { get; set; }
        public ObservableCollection<Deal> Deals { get; set; }
        public ObservableCollection<Demand> Demands { get; set; }
        public ObservableCollection<ApartmentDemandList> ApartmentsDemands { get; set; }
        public ObservableCollection<HouseDemandList> HousesDemands { get; set; }
        public ObservableCollection<LandDemandList> LandsDemands { get; set; }
        private string _clientSearchText = "";
        private string _agentSearchText = "";
        private string _estateSearchText = "";
        public ObservableCollection<Client> FilteredClients { get; set; }
        public ObservableCollection<RealEstate> FilteredEstates { get; set; }
        public Client SelectedClient { get; set; }
        public ObservableCollection<Agent> Agents { get; set; }
        public ObservableCollection<Agent> FilteredAgents { get; set; }
        public Agent SelectedAgent { get; set; }

        List<ApartmentList> apart = new List<ApartmentList>();
        List<LandList> landLists = new List<LandList>();
        List<HouseList> houseLists = new List<HouseList>();
        List<ApartmentDemandList> apartdem = new List<ApartmentDemandList>();
        List<LandDemandList> landdemLists = new List<LandDemandList>();
        List<HouseDemandList> housedemLists = new List<HouseDemandList>();

        ContextDB.Context context;

        public MainWindow()
        {
            InitializeComponent();
            var deallistik = new List<DealList>();
            context = new ContextDB.Context();
            Clients = new ObservableCollection<Client>(context.Clients.ToList());
            Rieltors = new ObservableCollection<Agent>(context.Agents.ToList());
            Estates = new ObservableCollection<RealEstate>(context.RealEstates.ToList());
            Supplies = new ObservableCollection<Supply>(context.Supplies.ToList());
            Deals = new ObservableCollection<Deal>(context.Deals.ToList());
            Demands = new ObservableCollection<Demand> (context.Demands.ToList());
            foreach(var deal in Deals)
            {
                DealList item = new DealList();
                item.Id = deal.Id;
                item.DemandId = deal.DemandId;
                item.SupplyId = deal.SupplyId;
                Demand demand = context.Demands.FirstOrDefault(r => r.Id == item.DemandId);
                string demandadress = demand.AddressCity + " " + demand.AddressStreet + " " + demand.AddressHouse + " " + demand.AddressNumber;
                if (demandadress.TrimEnd().IsNullOrEmpty())
                {
                    demandadress = "Адресс отсуствует";
                }
                item.demandAddress = item.DemandId + " " + demandadress;
                Supply supply = context.Supplies.FirstOrDefault(r => r.Id == item.SupplyId);
                RealEstate estate = context.RealEstates.FirstOrDefault(R => R.Id == supply.RealEstateId);
                string estateadres = estate.AddressCity + " " + estate.AddressStreet + " " + estate.AddressHouse + " " + estate.AddressNumber;
                if(estateadres.TrimEnd().IsNullOrEmpty())
                {
                    estateadres = "Адресс отсутвует";
                }
                item.RealEstateAddress = item.SupplyId + " " + estateadres;
                deallistik.Add(item);
            }
            deallist = new ObservableCollection<DealList>(deallistik);
            foreach(var estate in Estates)
            {
                if(estate.ApartmentId != null)
                {
                    ApartmentList apartment = new ApartmentList();
                    apartment.Id = estate.Id;
                    apartment.AddressCity = estate.AddressCity;
                    apartment.AddressStreet = estate.AddressStreet;
                    apartment.AddressHouse = estate.AddressHouse;
                    apartment.AddressNumber = estate.AddressNumber;
                    Apartment gotap = context.Apartments.FirstOrDefault(r => r.Id == estate.ApartmentId);
                    if(gotap != null)
                    {
                        apartment.Rooms = gotap.Rooms;
                        apartment.TotalArea = gotap.TotalArea;
                        apartment.Floor = gotap.Floor;
                    }
                    apart.Add(apartment);
                }
                if(estate.HouseId != null)
                {
                    HouseList house = new HouseList();
                    house.Id = estate.Id;
                    house.AddressCity = estate.AddressCity;
                    house.AddressStreet = estate.AddressStreet;
                    house.AddressNumber = estate.AddressNumber;
                    house.AddressHouse = estate.AddressHouse;
                    House hose = context.Houses.FirstOrDefault(r => r.Id == estate.HouseId);
                    if(hose != null)
                    {
                        house.Rooms = hose.Rooms;
                        house.TotalArea = hose.TotalArea;
                        house.TotalFloors = hose.TotalFloors;
                    }
                    houseLists.Add(house);
                }
                if(estate.LandId!=null)
                {
                    LandList land = new LandList();
                    land.Id = estate.Id;
                    land.AddressCity = estate.AddressCity;
                    land.AddressStreet = estate.AddressStreet;
                    land.AddressHouse = estate.AddressHouse;
                    land.AddressNumber = estate.AddressNumber;
                    Land land1 = context.Lands.FirstOrDefault(r => r.Id == estate.LandId);
                    if(land1 != null)
                    {
                        land.TotalArea = land1.TotalArea;
                    }
                    landLists.Add(land);
                }
            }
            Apartments = new ObservableCollection<ApartmentList>(apart);
            Houses = new ObservableCollection<HouseList>(houseLists);
            Lands = new ObservableCollection<LandList>(landLists);
            foreach(var demand in Demands )
            {
                if(demand.TypeId == 1)
                {
                    ApartmentDemandList apartment = new ApartmentDemandList();
                    apartment.Id = demand.Id;
                    apartment.AddressStreet = demand.AddressStreet;
                    apartment.AddressCity = demand.AddressCity;
                    apartment.AddressNumber = demand.AddressNumber;
                    apartment.AddressHouse = demand.AddressHouse;
                    apartment.MinPrice = demand.MinPrice;
                    apartment.MaxPrice = demand.MaxPrice;
                    apartment.AgentId = demand.AgentId;
                    apartment.ClientId = demand.ClientId;
                    apartment.TypeId = demand.TypeId;
                    ApartmentDemand apsdemand = context.ApartmentDemands.FirstOrDefault(r => r.Id == demand.ApartmentDemandId);
                    if(apsdemand != null)
                    {
                        apartment.MinArea = apsdemand.MinArea;
                        apartment.MaxArea = apsdemand.MaxArea;
                        apartment.MinFloor = apsdemand.MinFloor;
                        apartment.MaxFloor = apsdemand.MaxFloor;
                        apartment.MinRooms = apsdemand.MinRooms;
                        apartment.MaxRooms = apsdemand.MaxRooms;
                    }
                    apartdem.Add(apartment);
                }
                if(demand.TypeId == 2)
                {
                    HouseDemandList apartment = new HouseDemandList();
                    apartment.Id = demand.Id;
                    apartment.AddressStreet = demand.AddressStreet;
                    apartment.AddressCity = demand.AddressCity;
                    apartment.AddressNumber = demand.AddressNumber;
                    apartment.AddressHouse = demand.AddressHouse;
                    apartment.MinPrice = demand.MinPrice;
                    apartment.MaxPrice = demand.MaxPrice;
                    apartment.AgentId = demand.AgentId;
                    apartment.ClientId = demand.ClientId;
                    apartment.TypeId = demand.TypeId;
                    HouseDemand house = context.HouseDemands.FirstOrDefault(r => r.Id == demand.HouseDemandId);
                    if (house != null)
                    {
                        apartment.MinArea = house.MinArea;
                        apartment.MaxArea = house.MaxArea;
                        apartment.MinFloor = house.MinFloor;
                        apartment.MaxFloor = house.MaxFloor;
                        apartment.MinRooms = house.MinRooms;
                        apartment.MaxRooms = house.MaxRooms;
                    }
                    housedemLists.Add(apartment);
                }
                if(demand.TypeId == 3)
                {
                    LandDemandList apartment = new LandDemandList();
                    apartment.Id = demand.Id;
                    apartment.AddressStreet = demand.AddressStreet;
                    apartment.AddressCity = demand.AddressCity;
                    apartment.AddressNumber = demand.AddressNumber;
                    apartment.AddressHouse = demand.AddressHouse;
                    apartment.MinPrice = demand.MinPrice;
                    apartment.MaxPrice = demand.MaxPrice;
                    apartment.AgentId = demand.AgentId;
                    apartment.ClientId = demand.ClientId;
                    apartment.TypeId = demand.TypeId;
                    LandDemand land = context.LandDemands.FirstOrDefault(r => r.Id == demand.LandDemandId);
                    if(land != null)
                    {
                        apartment.MinArea = land.MinArea;
                        apartment.MaxArea = land.MaxArea;
                    }
                    landdemLists.Add(apartment);
                }
            }
            ApartmentsDemands = new ObservableCollection<ApartmentDemandList>(apartdem);
            HousesDemands = new ObservableCollection<HouseDemandList>(housedemLists);
            LandsDemands = new ObservableCollection<LandDemandList>(landdemLists);
            this.DataContext = this;
        }

        private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var window = new CreateClientWindow();
            await window.ShowDialog(this);
            Clients = new ObservableCollection<Client>(context.Clients.ToList());
            ClientGrid.ItemsSource = Clients;
        }

        private async void DeleteClient(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (ClientGrid.SelectedItem != null)
            {
                Client client = ClientGrid.SelectedItem as Client;
                if (context.Supplies.FirstOrDefault(r => r.ClientId == client.Id) == null && context.Demands.FirstOrDefault(r => r.ClientId == client.Id) ==null)
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы хотите удалить пользователя ?", ButtonEnum.YesNo);
                    var result = await errorBox.ShowWindowDialogAsync(this);
                    if(result == ButtonResult.Yes)
                    {
                        context.Clients.Remove(client);
                        context.SaveChanges();
                        Clients = new ObservableCollection<Client>(context.Clients.ToList());
                        ClientGrid.ItemsSource = Clients;
                        return;
                    }
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Нельзя удалить пользователя состоящего в сделке", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите клиента!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        public async void UpdateClient(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (ClientGrid.SelectedItem != null)
            {
                var window = new UpdateClientWindow(ClientGrid.SelectedItem as Client,context);
                await window.ShowDialog(this);
                Clients = new ObservableCollection<Client>(context.Clients.ToList());
                ClientGrid.ItemsSource = Clients;
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите клиента!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        private async void CreateAgent(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var window = new CreateAgentWindow();
            await window.ShowDialog(this);
            Rieltors = new ObservableCollection<Agent>(context.Agents.ToList());
            AgentGrid.ItemsSource = Rieltors;
        }

        private async void UpdateAgent(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(AgentGrid.SelectedItem != null)
            {
                var window = new UpdateAgentWindow(AgentGrid.SelectedItem as Agent,context);
                await window.ShowDialog(this);
                Rieltors = new ObservableCollection<Agent>(context.Agents.ToList());
                AgentGrid.ItemsSource = Rieltors;
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите риелтора!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        private async void DeleteAgent(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (AgentGrid.SelectedItem != null)
            {
                Agent agent = AgentGrid.SelectedItem as Agent;
                if (context.Supplies.FirstOrDefault(r => r.AgentId == agent.Id) == null && context.Demands.FirstOrDefault(r => r.AgentId == agent.Id) == null)
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы хотите удалить риелтора ?", ButtonEnum.YesNo);
                    var result = await errorBox.ShowWindowDialogAsync(this);
                    if (result == ButtonResult.Yes)
                    {
                        context.Agents.Remove(agent);
                        context.SaveChanges();
                        Rieltors = new ObservableCollection<Agent>(context.Agents.ToList());
                        AgentGrid.ItemsSource = Rieltors;
                        return;
                    }
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Нельзя удалить риелтора состоящего в сделке", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите риелтора!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        private async void DeleteEstate(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TabItem selectedTabItem = (TabItem)EstateTab.SelectedItem;
            var estate = new RealEstate();
            switch(selectedTabItem.Header.ToString())
            {
                case "Вся недвижимость":
                    if(MainGrid.SelectedItem != null)
                    {
                        estate = MainGrid.SelectedItem as RealEstate;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Квартиры":
                    if(ApartmentGrid.SelectedItem != null)
                    {
                        ApartmentList item = ApartmentGrid.SelectedItem as ApartmentList;
                        estate = context.RealEstates.FirstOrDefault(r => r.Id == item.Id);
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Дома":
                    if(HouseGrid.SelectedItem != null)
                    {
                        HouseList item = HouseGrid.SelectedItem as HouseList;
                        estate = context.RealEstates.FirstOrDefault(r => r.Id == item.Id);
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Земля":
                    if(LandGrid.SelectedItem != null)
                    {
                        LandList item = LandGrid.SelectedItem as LandList;
                        estate = context.RealEstates.FirstOrDefault(r => r.Id == item.Id);
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
            }
            if (context.Supplies.FirstOrDefault(r => r.RealEstateId == estate.Id) == null)
            {
                var errorBoxik = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы хотите удалить недвижимость ?", ButtonEnum.YesNo);
                var result = await errorBoxik.ShowWindowDialogAsync(this);
                if (result == ButtonResult.Yes)
                {
                    if (estate.ApartmentId != null)
                    {
                        Apartment aps = context.Apartments.FirstOrDefault(r => r.Id == estate.ApartmentId);
                        context.RealEstates.Remove(estate);
                        context.Apartments.Remove(aps);
                        context.SaveChanges();
                        UpdateGridsEstate();
                    }
                    if (estate.HouseId != null)
                    {
                        House house = context.Houses.FirstOrDefault(r => r.Id == estate.HouseId);
                        context.RealEstates.Remove(estate);
                        context.Houses.Remove(house);
                        context.SaveChanges();
                        UpdateGridsEstate();
                    }
                    if (estate.LandId != null)
                    {
                        Land land = context.Lands.FirstOrDefault(r => r.Id == estate.LandId);
                        context.RealEstates.Remove(estate);
                        context.Lands.Remove(land);
                        context.SaveChanges();
                        UpdateGridsEstate();
                    }
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Нельзя удалить недвижимость связанную с предложением!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        private void UpdateGridsEstate()
        {
            Estates = new ObservableCollection<RealEstate>();
            Estates = new ObservableCollection<RealEstate>(context.RealEstates.ToList());
            List<ApartmentList> apart = new List<ApartmentList>();
            List<LandList> landLists = new List<LandList>();
            List<HouseList> houseLists = new List<HouseList>();
            foreach (var estate in Estates)
            {
                if (estate.ApartmentId != null)
                {
                    ApartmentList apartment = new ApartmentList();
                    apartment.Id = estate.Id;
                    apartment.AddressCity = estate.AddressCity;
                    apartment.AddressStreet = estate.AddressStreet;
                    apartment.AddressHouse = estate.AddressHouse;
                    apartment.AddressNumber = estate.AddressNumber;
                    Apartment gotap = context.Apartments.FirstOrDefault(r => r.Id == estate.ApartmentId);
                    if (gotap != null)
                    {
                        apartment.Rooms = gotap.Rooms;
                        apartment.TotalArea = gotap.TotalArea;
                        apartment.Floor = gotap.Floor;
                    }
                    apart.Add(apartment);
                }
                if (estate.HouseId != null)
                {
                    HouseList house = new HouseList();
                    house.Id = estate.Id;
                    house.AddressCity = estate.AddressCity;
                    house.AddressStreet = estate.AddressStreet;
                    house.AddressNumber = estate.AddressNumber;
                    house.AddressHouse = estate.AddressHouse;
                    House hose = context.Houses.FirstOrDefault(r => r.Id == estate.HouseId);
                    if (hose != null)
                    {
                        house.Rooms = hose.Rooms;
                        house.TotalArea = hose.TotalArea;
                        house.TotalFloors = hose.TotalFloors;
                    }
                    houseLists.Add(house);
                }
                if (estate.LandId != null)
                {
                    LandList land = new LandList();
                    land.Id = estate.Id;
                    land.AddressCity = estate.AddressCity;
                    land.AddressStreet = estate.AddressStreet;
                    land.AddressHouse = estate.AddressHouse;
                    land.AddressNumber = estate.AddressNumber;
                    Land land1 = context.Lands.FirstOrDefault(r => r.Id == estate.LandId);
                    if (land1 != null)
                    {
                        land.TotalArea = land1.TotalArea;
                    }
                    landLists.Add(land);
                }
            }
            Apartments = new ObservableCollection<ApartmentList>(apart);
            Houses = new ObservableCollection<HouseList>(houseLists);
            Lands = new ObservableCollection<LandList>(landLists);
            MainGrid.ItemsSource = Estates;
            ApartmentGrid.ItemsSource = Apartments;
            HouseGrid.ItemsSource = Houses;
            LandGrid.ItemsSource = Lands;
        }

        private async void CreateEstate(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var window = new CreateEstateWindow();
            await window.ShowDialog(this);
            UpdateGridsEstate();
        }

        private async void UpdateEstate(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TabItem selectedTabItem = (TabItem)EstateTab.SelectedItem;
            var estate = new RealEstate();
            switch (selectedTabItem.Header.ToString())
            {
                case "Вся недвижимость":
                    if (MainGrid.SelectedItem != null)
                    {
                        estate = MainGrid.SelectedItem as RealEstate;
                        var window = new UpdateEstateWindow(estate,context);
                        await window.ShowDialog(this);
                        UpdateGridsEstate();
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Квартиры":
                    if (ApartmentGrid.SelectedItem != null)
                    {
                        ApartmentList item = ApartmentGrid.SelectedItem as ApartmentList;
                        estate = context.RealEstates.FirstOrDefault(r => r.Id == item.Id);
                        var window = new UpdateEstateWindow(estate, context);
                        await window.ShowDialog(this);
                        UpdateGridsEstate();
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Дома":
                    if (HouseGrid.SelectedItem != null)
                    {
                        HouseList item = HouseGrid.SelectedItem as HouseList;
                        estate = context.RealEstates.FirstOrDefault(r => r.Id == item.Id);
                        var window = new UpdateEstateWindow(estate, context);
                        await window.ShowDialog(this);
                        UpdateGridsEstate();
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Земля":
                    if (LandGrid.SelectedItem != null)
                    {
                        LandList item = LandGrid.SelectedItem as LandList;
                        estate = context.RealEstates.FirstOrDefault(r => r.Id == item.Id);
                        var window = new UpdateEstateWindow(estate, context);
                        await window.ShowDialog(this);
                        UpdateGridsEstate();
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
            }
        }

        private async void CreateSupply(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var window = new CreateSupply();
            await window.ShowDialog(this);
            Supplies = new ObservableCollection<Supply>(context.Supplies.ToList());
            SupplyGrid.ItemsSource = Supplies;
        }

        private async void UpdateSupply(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (SupplyGrid.SelectedItem != null)
            {
                var window = new UpdateSupply(SupplyGrid.SelectedItem as Supply, context);
                await window.ShowDialog(this);
                Supplies = new ObservableCollection<Supply>(context.Supplies.ToList());
                SupplyGrid.ItemsSource = Supplies;
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите предложение!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        private async void DeleteSupply(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (SupplyGrid.SelectedItem != null)
            {
                Supply supply = SupplyGrid.SelectedItem as Supply;
                if (context.Deals.FirstOrDefault(r => r.SupplyId == supply.Id) == null)
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы хотите удалить риелтора ?", ButtonEnum.YesNo);
                    var result = await errorBox.ShowWindowDialogAsync(this);
                    if (result == ButtonResult.Yes)
                    {
                        context.Supplies.Remove(supply);
                        context.SaveChanges();
                        Supplies = new ObservableCollection<Supply>(context.Supplies.ToList());
                        SupplyGrid.ItemsSource = Supplies;
                        return;
                    }
                }
                else
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Нельзя удалить предложение состоящее в сделке", ButtonEnum.Ok);
                    await errorBox.ShowWindowDialogAsync(this);
                    return;
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите предложение!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        private async void CreateDeal(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var window = new CreateDealWindow();
            await window.ShowDialog(this);
            var deallistik = new List<DealList>();
            Deals = new ObservableCollection<Deal>(context.Deals.ToList());
            foreach (var deal in Deals)
            {
                DealList item = new DealList();
                item.Id = deal.Id;
                item.DemandId = deal.DemandId;
                item.SupplyId = deal.SupplyId;
                Demand demand = context.Demands.FirstOrDefault(r => r.Id == item.DemandId);
                string demandadress = demand.AddressCity + " " + demand.AddressStreet + " " + demand.AddressHouse + " " + demand.AddressNumber;
                if (demandadress.TrimEnd().IsNullOrEmpty())
                {
                    demandadress = "Адресс отсуствует";
                }
                item.demandAddress = item.DemandId + " " + demandadress;
                Supply supply = context.Supplies.FirstOrDefault(r => r.Id == item.SupplyId);
                RealEstate estate = context.RealEstates.FirstOrDefault(R => R.Id == supply.RealEstateId);
                string estateadres = estate.AddressCity + " " + estate.AddressStreet + " " + estate.AddressHouse + " " + estate.AddressNumber;
                if (estateadres.TrimEnd().IsNullOrEmpty())
                {
                    estateadres = "Адресс отсутвует";
                }
                item.RealEstateAddress = item.SupplyId + " " + estateadres;
                deallistik.Add(item);
            }
            deallist = new ObservableCollection<DealList>(deallistik);
            DealGrid.ItemsSource = deallist;
        }

        private async void UpdateDeal(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(DealGrid.SelectedItem != null)
            {
                var window = new UpdateDealWindow(DealGrid.SelectedItem as DealList, context);
                await window.ShowDialog(this);
                Deals = new ObservableCollection<Deal>(context.Deals.ToList());
                var deallistik = new List<DealList>();
                foreach (var deal in Deals)
                {
                    DealList item = new DealList();
                    item.Id = deal.Id;
                    item.DemandId = deal.DemandId;
                    item.SupplyId = deal.SupplyId;
                    Demand demand = context.Demands.FirstOrDefault(r => r.Id == item.DemandId);
                    string demandadress = demand.AddressCity + " " + demand.AddressStreet + " " + demand.AddressHouse + " " + demand.AddressNumber;
                    if (demandadress.TrimEnd().IsNullOrEmpty())
                    {
                        demandadress = "Адресс отсуствует";
                    }
                    item.demandAddress = item.DemandId + " " + demandadress;
                    Supply supply = context.Supplies.FirstOrDefault(r => r.Id == item.SupplyId);
                    RealEstate estate = context.RealEstates.FirstOrDefault(R => R.Id == supply.RealEstateId);
                    string estateadres = estate.AddressCity + " " + estate.AddressStreet + " " + estate.AddressHouse + " " + estate.AddressNumber;
                    if (estateadres.TrimEnd().IsNullOrEmpty())
                    {
                        estateadres = "Адресс отсутвует";
                    }
                    item.RealEstateAddress = item.SupplyId + " " + estateadres;
                    deallistik.Add(item);
                }
                deallist = new ObservableCollection<DealList>(deallistik);
                DealGrid.ItemsSource = deallist;
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите сделку!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }

        }

        private async void DeleatDeal(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(DealGrid.SelectedItem != null)
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы хотите удалить сделку ?", ButtonEnum.YesNo);
                var result = await errorBox.ShowWindowDialogAsync(this);
                if (result == ButtonResult.Yes)
                {
                    DealList dealo = DealGrid.SelectedItem as DealList;
                    context.Deals.Remove(context.Deals.FirstOrDefault(r => r.Id == dealo.Id));
                    context.SaveChanges();
                    Deals = new ObservableCollection<Deal>(context.Deals.ToList());
                    var deallistik = new List<DealList>();
                    foreach (var deal in Deals)
                    {
                        DealList item = new DealList();
                        item.Id = deal.Id;
                        item.DemandId = deal.DemandId;
                        item.SupplyId = deal.SupplyId;
                        Demand demand = context.Demands.FirstOrDefault(r => r.Id == item.DemandId);
                        string demandadress = demand.AddressCity + " " + demand.AddressStreet + " " + demand.AddressHouse + " " + demand.AddressNumber;
                        if (demandadress.TrimEnd().IsNullOrEmpty())
                        {
                            demandadress = "Адресс отсуствует";
                        }
                        item.demandAddress = item.DemandId + " " + demandadress;
                        Supply supply = context.Supplies.FirstOrDefault(r => r.Id == item.SupplyId);
                        RealEstate estate = context.RealEstates.FirstOrDefault(R => R.Id == supply.RealEstateId);
                        string estateadres = estate.AddressCity + " " + estate.AddressStreet + " " + estate.AddressHouse + " " + estate.AddressNumber;
                        if (estateadres.TrimEnd().IsNullOrEmpty())
                        {
                            estateadres = "Адресс отсутвует";
                        }
                        item.RealEstateAddress = item.SupplyId + " " + estateadres;
                        deallistik.Add(item);
                    }
                    deallist = new ObservableCollection<DealList>(deallistik);
                    DealGrid.ItemsSource = deallist;
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите сделку!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        private async void CreateDemand(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var window = new CreateDemandWindow();
            await window.ShowDialog(this);
            UpdateDemandGrid();
        }

        private void UpdateDemandGrid()
        {
            Demands = new ObservableCollection<Demand>(context.Demands.ToList());
            apartdem = new List<ApartmentDemandList>();
            housedemLists = new List<HouseDemandList>();
            landdemLists = new List<LandDemandList>();
            foreach (var demand in Demands)
            {
                if (demand.TypeId == 1)
                {
                    ApartmentDemandList apartment = new ApartmentDemandList();
                    apartment.Id = demand.Id;
                    apartment.AddressStreet = demand.AddressStreet;
                    apartment.AddressCity = demand.AddressCity;
                    apartment.AddressNumber = demand.AddressNumber;
                    apartment.AddressHouse = demand.AddressHouse;
                    apartment.MinPrice = demand.MinPrice;
                    apartment.MaxPrice = demand.MaxPrice;
                    apartment.AgentId = demand.AgentId;
                    apartment.ClientId = demand.ClientId;
                    apartment.TypeId = demand.TypeId;
                    ApartmentDemand apsdemand = context.ApartmentDemands.FirstOrDefault(r => r.Id == demand.ApartmentDemandId);
                    if (apsdemand != null)
                    {
                        apartment.MinArea = apsdemand.MinArea;
                        apartment.MaxArea = apsdemand.MaxArea;
                        apartment.MinFloor = apsdemand.MinFloor;
                        apartment.MaxFloor = apsdemand.MaxFloor;
                        apartment.MinRooms = apsdemand.MinRooms;
                        apartment.MaxRooms = apsdemand.MaxRooms;
                    }
                    apartdem.Add(apartment);
                }
                if (demand.TypeId == 2)
                {
                    HouseDemandList apartment = new HouseDemandList();
                    apartment.Id = demand.Id;
                    apartment.AddressStreet = demand.AddressStreet;
                    apartment.AddressCity = demand.AddressCity;
                    apartment.AddressNumber = demand.AddressNumber;
                    apartment.AddressHouse = demand.AddressHouse;
                    apartment.MinPrice = demand.MinPrice;
                    apartment.MaxPrice = demand.MaxPrice;
                    apartment.AgentId = demand.AgentId;
                    apartment.ClientId = demand.ClientId;
                    apartment.TypeId = demand.TypeId;
                    HouseDemand house = context.HouseDemands.FirstOrDefault(r => r.Id == demand.HouseDemandId);
                    if (house != null)
                    {
                        apartment.MinArea = house.MinArea;
                        apartment.MaxArea = house.MaxArea;
                        apartment.MinFloor = house.MinFloor;
                        apartment.MaxFloor = house.MaxFloor;
                        apartment.MinRooms = house.MinRooms;
                        apartment.MaxRooms = house.MaxRooms;
                    }
                    housedemLists.Add(apartment);
                }
                if (demand.TypeId == 3)
                {
                    LandDemandList apartment = new LandDemandList();
                    apartment.Id = demand.Id;
                    apartment.AddressStreet = demand.AddressStreet;
                    apartment.AddressCity = demand.AddressCity;
                    apartment.AddressNumber = demand.AddressNumber;
                    apartment.AddressHouse = demand.AddressHouse;
                    apartment.MinPrice = demand.MinPrice;
                    apartment.MaxPrice = demand.MaxPrice;
                    apartment.AgentId = demand.AgentId;
                    apartment.ClientId = demand.ClientId;
                    apartment.TypeId = demand.TypeId;
                    LandDemand land = context.LandDemands.FirstOrDefault(r => r.Id == demand.LandDemandId);
                    if (land != null)
                    {
                        apartment.MinArea = land.MinArea;
                        apartment.MaxArea = land.MaxArea;
                    }
                    landdemLists.Add(apartment);
                }
            }
            ApartmentsDemands = new ObservableCollection<ApartmentDemandList>(apartdem);
            HousesDemands = new ObservableCollection<HouseDemandList>(housedemLists);
            LandsDemands = new ObservableCollection<LandDemandList>(landdemLists);
            MainDemandGrid.ItemsSource = Demands;
            ApartmentDemandGrid.ItemsSource = ApartmentsDemands;
            HouseDemandGrid.ItemsSource = HousesDemands;
            LandDemandGrid.ItemsSource = LandsDemands;
        }

        private async void DeleteDemand(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TabItem selectedTabItem = (TabItem)DemandTab.SelectedItem;
            var demand = new Demand();
            switch (selectedTabItem.Header.ToString())
            {
                case "Вся недвижимость":
                    if (MainDemandGrid.SelectedItem != null)
                    {
                        demand = MainDemandGrid.SelectedItem as Demand;
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Квартиры":
                    if (ApartmentDemandGrid.SelectedItem != null)
                    {
                        ApartmentDemandList item = ApartmentDemandGrid.SelectedItem as ApartmentDemandList;
                        demand = context.Demands.FirstOrDefault(r => r.Id == item.Id);
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Дома":
                    if (HouseDemandGrid.SelectedItem != null)
                    {
                        HouseDemandList item = HouseDemandGrid.SelectedItem as HouseDemandList;
                        demand = context.Demands.FirstOrDefault(r => r.Id == item.Id);
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Земля":
                    if (LandDemandGrid.SelectedItem != null)
                    {
                        LandDemandList item = LandDemandGrid.SelectedItem as LandDemandList;
                        demand = context.Demands.FirstOrDefault(r => r.Id == item.Id);
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
            }
            if (context.Deals.FirstOrDefault(r => r.DemandId == demand.Id) == null)
            {
                var errorBoxik = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы хотите удалить недвижимость ?", ButtonEnum.YesNo);
                var result = await errorBoxik.ShowWindowDialogAsync(this);
                if (result == ButtonResult.Yes)
                {
                    if (demand.TypeId == 1)
                    {
                        ApartmentDemand aps = context.ApartmentDemands.FirstOrDefault(r => r.Id == demand.ApartmentDemandId);
                        context.Demands.Remove(demand);
                        context.ApartmentDemands.Remove(aps);
                        context.SaveChanges();
                        UpdateDemandGrid();
                    }
                    if (demand.TypeId == 2)
                    {
                        HouseDemand house = context.HouseDemands.FirstOrDefault(r => r.Id == demand.HouseDemandId);
                        context.Demands.Remove(demand);
                        context.HouseDemands.Remove(house);
                        context.SaveChanges();
                        UpdateDemandGrid();
                    }
                    if (demand.TypeId == 3)
                    {
                        LandDemand land = context.LandDemands.FirstOrDefault(r => r.Id == demand.LandDemandId);
                        context.Demands.Remove(demand);
                        context.LandDemands.Remove(land);
                        context.SaveChanges();
                        UpdateDemandGrid();
                    }
                }
            }
            else
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Нельзя удалить недвижимость связанную с предложением!", ButtonEnum.Ok);
                await errorBox.ShowWindowDialogAsync(this);
                return;
            }
        }

        private async void UpdateDemand(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TabItem selectedTabItem = (TabItem)DemandTab.SelectedItem;
            var demand = new Demand();
            switch (selectedTabItem.Header.ToString())
            {
                case "Вся недвижимость":
                    if (MainDemandGrid.SelectedItem != null)
                    {
                        demand = MainDemandGrid.SelectedItem as Demand;
                        var window = new UpdateDemandWindow(demand,context);
                        await window.ShowDialog(this);
                        UpdateDemandGrid();
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Квартиры":
                    if (ApartmentDemandGrid.SelectedItem != null)
                    {
                        ApartmentDemandList item = ApartmentDemandGrid.SelectedItem as ApartmentDemandList;
                        demand = context.Demands.FirstOrDefault(r => r.Id == item.Id);
                        var window = new UpdateDemandWindow(demand,context);
                        await window.ShowDialog(this);
                        UpdateDemandGrid();
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Дома":
                    if (HouseDemandGrid.SelectedItem != null)
                    {
                        HouseDemandList item = HouseDemandGrid.SelectedItem as HouseDemandList;
                        demand = context.Demands.FirstOrDefault(r => r.Id == item.Id);
                        var window = new UpdateDemandWindow(demand,context);
                        await window.ShowDialog(this);
                        UpdateDemandGrid();
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
                case "Земля":
                    if (LandDemandGrid.SelectedItem != null)
                    {
                        LandDemandList item = LandDemandGrid.SelectedItem as LandDemandList;
                        demand = context.Demands.FirstOrDefault(r => r.Id == item.Id);
                        var window = new UpdateDemandWindow(demand,context);
                        await window.ShowDialog(this);
                        UpdateDemandGrid();
                    }
                    else
                    {
                        var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите недвижимость!", ButtonEnum.Ok);
                        await errorBox.ShowWindowDialogAsync(this);
                        return;
                    }
                    break;
            }
        }
        private bool IsMatch(string? fieldValue, string[] searchTerms)
        {
            if (string.IsNullOrEmpty(fieldValue))
                return false;

            foreach (var term in searchTerms)
            {
                if (LevenshteinDistance.Calculate(fieldValue.ToLower(), term.ToLower()) <= 3)
                {
                    return true;
                }
            }

            return false;
        }
        private void ClientSearchBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (sender is TextBox searchBox)
            {
                _clientSearchText = searchBox.Text?.Trim() ?? "";
                FilterClients();
            }
        }

        private void AgentSearchBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (sender is TextBox searchBox)
            {
                _agentSearchText = searchBox.Text?.Trim() ?? "";
                FilterAgents();
            }
        }
        private void FilterClients()
        {
            if (string.IsNullOrWhiteSpace(_clientSearchText))
            {
                FilteredClients = new ObservableCollection<Client>(Clients);
            }
            else
            {
                var searchTerms = _clientSearchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var filtered = Clients.Where(client =>
                    IsMatch(client.FirstName, searchTerms) ||
                    IsMatch(client.MiddleName, searchTerms) ||
                    IsMatch(client.LastName, searchTerms));

                FilteredClients = new ObservableCollection<Client>(filtered);
            }

            ClientGrid.ItemsSource = FilteredClients;
        }

        private void FilterAgents()
        {
            if (string.IsNullOrWhiteSpace(_agentSearchText))
            {
                FilteredAgents = new ObservableCollection<Agent>(Rieltors);
            }
            else
            {
                var searchTerms = _agentSearchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var filtered = Rieltors.Where(agent =>
                    IsMatch(agent.FirstName, searchTerms) ||
                    IsMatch(agent.MiddleName, searchTerms) ||
                    IsMatch(agent.LastName, searchTerms));

                FilteredAgents = new ObservableCollection<Agent>(filtered);
            }

            AgentGrid.ItemsSource = FilteredAgents;
        }

        private void FilterEstates()
        {
            if (string.IsNullOrWhiteSpace(_estateSearchText))
            {
                FilteredEstates = new ObservableCollection<RealEstate>(Estates);
            }
            else
            {
                var searchTerms = _estateSearchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var filtered = Estates.Where(estate =>
                    IsMatch(estate.AddressCity, searchTerms) ||
                    IsMatch(estate.AddressStreet, searchTerms));

                FilteredEstates = new ObservableCollection<RealEstate>(filtered);
            }

            MainGrid.ItemsSource = FilteredEstates;
        }

        private void EstateSearchBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            if (sender is TextBox searchBox)
            {
                _estateSearchText = searchBox.Text?.Trim() ?? "";
                FilterEstates();
            }
        }
    }
} 